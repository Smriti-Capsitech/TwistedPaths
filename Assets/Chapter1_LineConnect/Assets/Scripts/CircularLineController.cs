
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class CircularLineController : MonoBehaviour
{
    public static CircularLineController Instance;

    [Header("Prefabs")]
    public GameObject ropeNodePrefab;

    [Header("References")]
    public CircularDotGenerator dotGenerator;
    public Transform innerGrid;
    public SpriteRenderer board;

    [Header("Line Visual")]
    public Material lightningMaterial;

    [Header("Settings")]
    public float snapRadius = 0.4f;
    public float ropeClickTolerance = 0.35f;

    private LineRenderer line;
    private readonly List<RopeNode> nodes = new();
    private RopeNode tempNode;

    private Vector3 boardCenter;
    private float boardRadius;

    private int initialNodeCount = 0;

    // =========================
    // UNITY
    // =========================
    void Awake()
    {
        Instance = this;

        line = gameObject.AddComponent<LineRenderer>();
        line.useWorldSpace = true;
        line.material = lightningMaterial != null
            ? lightningMaterial
            : new Material(Shader.Find("Sprites/Default"));

        line.startWidth = 0.25f;
        line.endWidth = 0.25f;
        line.sortingOrder = 999;
        line.textureMode = LineTextureMode.Tile;
        line.alignment = LineAlignment.View;
        line.positionCount = 0;
    }

    IEnumerator Start()
    {
        yield return new WaitUntil(() =>
            dotGenerator != null &&
            dotGenerator.dots.Count > 0
        );

        boardCenter = board.transform.position;
        boardRadius = board.bounds.size.x * 0.5f;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) StartDrag();
        if (Input.GetMouseButton(0)) Drag();
        if (Input.GetMouseButtonUp(0)) EndDrag();
    }

    // =========================
    // INITIAL ROPE (FIXED)
    // =========================
    public void CreateInitialRope(int[] indices)
    {
        ResetCompletely();

        InnerGridGenerator inner = FindAnyObjectByType<InnerGridGenerator>();

        foreach (int i in indices)
        {
            RopeNode rn = null;

            // -------- OUTER DOTS (0–11)
            if (i >= 0 && i < dotGenerator.dots.Count)
            {
                CircleDot dot = dotGenerator.dots[i];
                dot.isOccupied = true;

                GameObject obj = Instantiate(
                    ropeNodePrefab,
                    dot.transform.position,
                    Quaternion.identity
                );

                rn = obj.GetComponent<RopeNode>();
                rn.dot = dot;
                rn.innerNode = null;
            }
            // -------- INNER DOTS (12–16)
            else if (i >= 12 && inner != null)
            {
                InnerSnapNode found = inner.nodes.Find(n => n.index == i);
                if (found == null) continue;

                found.isOccupied = true;

                GameObject obj = Instantiate(
                    ropeNodePrefab,
                    found.transform.position,
                    Quaternion.identity
                );

                rn = obj.GetComponent<RopeNode>();
                rn.dot = null;
                rn.innerNode = found;
            }

            if (rn != null)
                nodes.Add(rn);
        }

        initialNodeCount = nodes.Count;
        UpdateLine();
    }

    // =========================
    // DRAG
    // =========================
    void StartDrag()
    {
        if (nodes.Count < 2) return;
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;

        Vector3 mousePos = GetMouseWorld();
        if (Vector2.Distance(mousePos, boardCenter) > boardRadius) return;
        if (!IsMouseNearRope(mousePos)) return;

        GameObject obj = Instantiate(ropeNodePrefab, mousePos, Quaternion.identity);
        tempNode = obj.GetComponent<RopeNode>();
        tempNode.dot = null;
        tempNode.innerNode = null;

        int insertIndex = FindClosestSegment(mousePos);
        nodes.Insert(insertIndex, tempNode);

        UpdateLine();
    }

    void Drag()
    {
        if (tempNode == null) return;

        Vector3 pos = GetMouseWorld();
        Vector2 dir = pos - boardCenter;

        if (dir.magnitude > boardRadius)
            pos = boardCenter + (Vector3)(dir.normalized * boardRadius);

        tempNode.transform.position = pos;
        UpdateLine();
    }

    void EndDrag()
    {
        if (tempNode == null) return;

        Transform snap = GetNearestSnapPoint(tempNode.transform.position);

        if (snap != null)
        {
            tempNode.transform.position = snap.position;

            CircleDot d = snap.GetComponent<CircleDot>();
            InnerSnapNode inner = snap.GetComponent<InnerSnapNode>();

            if (d != null)
            {
                d.isOccupied = true;
                tempNode.dot = d;
            }
            else if (inner != null)
            {
                inner.isOccupied = true;
                tempNode.innerNode = inner;
            }
        }
        else
        {
            nodes.Remove(tempNode);
            Destroy(tempNode.gameObject);
        }

        tempNode = null;
        UpdateLine();

        FindAnyObjectByType<LevelCompleteChecker>()?.CheckNow();
    }

    // =========================
    // LINE
    // =========================
    void UpdateLine()
    {
        if (nodes.Count < 2)
        {
            line.positionCount = 0;
            return;
        }

        line.positionCount = nodes.Count;
        for (int i = 0; i < nodes.Count; i++)
            line.SetPosition(i, nodes[i].transform.position);
    }

    // =========================
    // SNAP HELPERS
    // =========================
    bool IsMouseNearRope(Vector2 mousePos)
    {
        for (int i = 0; i < nodes.Count - 1; i++)
        {
            float d = DistancePointToSegment(
                mousePos,
                nodes[i].transform.position,
                nodes[i + 1].transform.position
            );

            if (d <= ropeClickTolerance)
                return true;
        }
        return false;
    }

    int FindClosestSegment(Vector2 p)
    {
        float min = float.MaxValue;
        int index = 1;

        for (int i = 0; i < nodes.Count - 1; i++)
        {
            float d = DistancePointToSegment(
                p,
                nodes[i].transform.position,
                nodes[i + 1].transform.position
            );

            if (d < min)
            {
                min = d;
                index = i + 1;
            }
        }
        return index;
    }

    float DistancePointToSegment(Vector2 p, Vector2 a, Vector2 b)
    {
        Vector2 ab = b - a;
        float t = Mathf.Clamp01(Vector2.Dot(p - a, ab) / ab.sqrMagnitude);
        return Vector2.Distance(p, a + t * ab);
    }

    Transform GetNearestSnapPoint(Vector3 pos)
    {
        Transform nearest = null;
        float min = snapRadius;

        foreach (CircleDot d in dotGenerator.dots)
        {
            if (d.isOccupied) continue;

            float dist = Vector2.Distance(pos, d.transform.position);
            if (dist < min)
            {
                min = dist;
                nearest = d.transform;
            }
        }

        if (innerGrid != null)
        {
            foreach (Transform t in innerGrid)
            {
                InnerSnapNode inner = t.GetComponent<InnerSnapNode>();
                if (inner != null && inner.isOccupied) continue;

                float dist = Vector2.Distance(pos, t.position);
                if (dist < min)
                {
                    min = dist;
                    nearest = t;
                }
            }
        }

        return nearest;
    }

    Vector3 GetMouseWorld()
    {
        Vector3 p = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        p.z = 0;
        return p;
    }

    // =========================
    // 🔥 IMPORTANT FOR LEVEL CHECK
    // =========================
    // public List<int> GetSnappedNodes()
    // {
    //     List<int> result = new();

    //     foreach (var n in nodes)
    //     {
    //         if (n.dot != null)
    //             result.Add(n.dot.index);
    //         else if (n.innerNode != null)
    //             result.Add(n.innerNode.index);
    //     }

    //     return result;
    // }
    public List<int> GetSnappedNodes()
{
    List<int> result = new();

    foreach (var n in nodes)
    {
        // ❌ Ignore temp / unsnapped nodes
        if (n == null) continue;

        // OUTER
        if (n.dot != null)
        {
            result.Add(n.dot.index);
        }
        // INNER
        else if (n.innerNode != null)
        {
            result.Add(n.innerNode.index);
        }
    }

    return result;
}



    // =========================
    // RESET
    // =========================
    public void ResetCompletely()
    {
        foreach (var n in nodes)
            Destroy(n.gameObject);

        nodes.Clear();
        tempNode = null;

        foreach (CircleDot d in dotGenerator.dots)
            d.isOccupied = false;

        if (innerGrid != null)
        {
            foreach (Transform t in innerGrid)
            {
                InnerSnapNode inner = t.GetComponent<InnerSnapNode>();
                if (inner != null)
                    inner.isOccupied = false;
            }
        }

        line.positionCount = 0;
        initialNodeCount = 0;
    }
    // =========================
// REQUIRED FOR LEVEL CHECK
// =========================
public bool IsClosed()
{
    if (nodes.Count < 3) return false;

    RopeNode first = nodes[0];
    RopeNode last = nodes[^1];

    if (first.dot != null && last.dot != null)
        return first.dot.index == last.dot.index;

    if (first.innerNode != null && last.innerNode != null)
        return first.innerNode.index == last.innerNode.index;

    return false;
}

public bool PlayerModifiedRope()
{
    return nodes.Count > initialNodeCount;
}


    // =========================
    // DEBUG HELPERS
    // =========================
    public int LineCount => line.positionCount;
    public Vector3 GetPoint(int i) => line.GetPosition(i);
}
