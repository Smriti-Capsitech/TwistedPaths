
// using UnityEngine;
// using System.Collections;
// using System.Collections.Generic;

// public class CircularLineController : MonoBehaviour
// {
//     public static CircularLineController Instance;

//     [Header("Prefabs")]
//     public GameObject ropeNodePrefab;

//     [Header("References")]
//     public CircularDotGenerator dotGenerator;
//     public Transform innerGrid;
//     public SpriteRenderer board;

//     [Header("Line Visual")]
//     public Material lightningMaterial;

//     [Header("Settings")]
//     public float snapRadius = 0.4f;
//     public float dragStartDistance = 0.15f;

//     private LineRenderer line;
//     private readonly List<RopeNode> nodes = new();
//     private RopeNode tempNode;

//     private Vector3 boardCenter;
//     private float boardRadius;

//     private int initialNodeCount = 0;

//     private bool isDragging = false;
//     private Vector3 dragStartPos;

//     // =========================
//     // UNITY
//     // =========================
//     void Awake()
//     {
//         Instance = this;

//         line = gameObject.AddComponent<LineRenderer>();
//         line.useWorldSpace = true;
//         line.material = lightningMaterial != null
//             ? lightningMaterial
//             : new Material(Shader.Find("Sprites/Default"));

//         line.startWidth = 0.13f;
//         line.endWidth = 0.13f;
//         line.sortingOrder = 999;
//         line.textureMode = LineTextureMode.Stretch;
//         line.alignment = LineAlignment.View;
//         line.positionCount = 0;
//     }

//     IEnumerator Start()
//     {
//         yield return new WaitUntil(() =>
//             dotGenerator != null &&
//             dotGenerator.dots.Count > 0
//         );

//         boardCenter = board.transform.position;
//         boardRadius = board.bounds.size.x * 0.5f;
//     }

//     void Update()
//     {
//         if (Input.GetMouseButtonDown(0))
//             BeginInput();

//         if (Input.GetMouseButton(0))
//             UpdateDrag();

//         if (Input.GetMouseButtonUp(0))
//             EndDrag();
//     }

//     // =========================
//     // INPUT
//     // =========================
//     void BeginInput()
//     {
//         dragStartPos = GetMouseWorld();
//         isDragging = false;
//     }

//     void UpdateDrag()
//     {
//         Vector3 currentPos = GetMouseWorld();

//         if (!isDragging)
//         {
//             if (Vector3.Distance(dragStartPos, currentPos) < dragStartDistance)
//                 return;

//             if (!StartDrag(dragStartPos))
//                 return;

//             isDragging = true;
//         }

//         Drag(currentPos);
//     }

//     void EndDrag()
//     {
//         if (!isDragging) return;

//         FinalizeDrag();
//         isDragging = false;

//         StartCoroutine(CheckCompletionNextFrame());
//     }

//     // =========================
//     // DRAG CORE
//     // =========================
//     bool StartDrag(Vector3 pos)
//     {
//         if (nodes.Count < 2) return false;
//         if (Vector2.Distance(pos, boardCenter) > boardRadius) return false;

//         int insertIndex = FindClosestSegment(pos);
//         if (insertIndex < 0) return false;

//         GameObject obj = Instantiate(ropeNodePrefab, pos, Quaternion.identity);
//         obj.SetActive(true);
//         tempNode = obj.GetComponent<RopeNode>();
//         tempNode.dot = null;
//         tempNode.innerNode = null;

//         nodes.Insert(insertIndex, tempNode);
//         UpdateLine();
//         return true;
//     }

//     void Drag(Vector3 pos)
//     {
//         if (tempNode == null) return;

//         Vector2 dir = pos - boardCenter;
//         if (dir.magnitude > boardRadius)
//             pos = boardCenter + (Vector3)(dir.normalized * boardRadius);

//         tempNode.transform.position = pos;
//         UpdateLine();
//     }

//     void FinalizeDrag()
//     {
//         if (tempNode == null) return;

//         Transform snap = GetNearestSnapPoint(tempNode.transform.position);

//         if (snap != null)
//         {
//             tempNode.transform.position = snap.position;

//             CircleDot d = snap.GetComponent<CircleDot>();
//             InnerSnapNode inner = snap.GetComponent<InnerSnapNode>();

//             tempNode.dot = null;
//             tempNode.innerNode = null;

//             if (d != null)
//             {
//                 d.isOccupied = true;
//                 tempNode.dot = d;
//             }
//             else if (inner != null)
//             {
//                 inner.isOccupied = true;
//                 tempNode.innerNode = inner;
//             }
//         }
//         else
//         {
//             nodes.Remove(tempNode);
//             Destroy(tempNode.gameObject);
//         }

//         tempNode = null;
//         UpdateLine();
//     }

//     // =========================
//     // INITIAL ROPE (FIXED)
//     // =========================
//     public void CreateInitialRope(int[] indices)
//     {
//         ResetCompletely();

//         InnerGridGenerator inner = FindAnyObjectByType<InnerGridGenerator>();

//         foreach (int i in indices)
//         {
//             RopeNode rn;

//             if (i >= 0 && i <= 11)
//             {
//                 CircleDot dot = dotGenerator.dots[i];
//                 dot.isOccupied = true;

//                 rn = Instantiate(
//                     ropeNodePrefab,
//                     dot.transform.position,
//                     Quaternion.identity
//                 ).GetComponent<RopeNode>();

//                 rn.dot = dot;
//                 rn.innerNode = null;
//             }
//             else
//             {
//                 InnerSnapNode found = inner.nodes.Find(n => n.index == i);
//                 if (found == null) continue;

//                 found.isOccupied = true;

//                 rn = Instantiate(
//                     ropeNodePrefab,
//                     found.transform.position,
//                     Quaternion.identity
//                 ).GetComponent<RopeNode>();

//                 rn.dot = null;
//                 rn.innerNode = found;
//             }

//             // 🔥🔥🔥 THE FIX (DO NOT REMOVE)
//             rn.gameObject.SetActive(true);

//             nodes.Add(rn);
//         }

//         initialNodeCount = nodes.Count;
//         UpdateLine();

//         StartCoroutine(CheckCompletionNextFrame());
//     }

//     // =========================
//     // COMPLETION CHECK
//     // =========================
//     IEnumerator CheckCompletionNextFrame()
//     {
//         yield return null;
//         FindAnyObjectByType<LevelCompleteChecker>()?.CheckNow();
//     }

//     // =========================
//     // LINE
//     // =========================
//     void UpdateLine()
//     {
//         line.positionCount = nodes.Count;
//         for (int i = 0; i < nodes.Count; i++)
//             line.SetPosition(i, nodes[i].transform.position);
//     }

//     // =========================
//     // SNAP (INNER FIRST)
//     // =========================
//     Transform GetNearestSnapPoint(Vector3 pos)
//     {
//         Transform nearest = null;
//         float min = snapRadius;

//         if (innerGrid != null)
//         {
//             foreach (Transform t in innerGrid)
//             {
//                 InnerSnapNode inner = t.GetComponent<InnerSnapNode>();
//                 if (inner == null || inner.isOccupied) continue;

//                 float d = Vector2.Distance(pos, t.position);
//                 if (d < min) { min = d; nearest = t; }
//             }
//         }

//         foreach (CircleDot d in dotGenerator.dots)
//         {
//             if (d.isOccupied) continue;

//             float dist = Vector2.Distance(pos, d.transform.position);
//             if (dist < min) { min = dist; nearest = d.transform; }
//         }

//         return nearest;
//     }

//     int FindClosestSegment(Vector2 p)
//     {
//         float min = float.MaxValue;
//         int index = -1;

//         for (int i = 0; i < nodes.Count - 1; i++)
//         {
//             float d = DistancePointToSegment(
//                 p,
//                 nodes[i].transform.position,
//                 nodes[i + 1].transform.position
//             );
//             if (d < min) { min = d; index = i + 1; }
//         }
//         return index;
//     }

//     float DistancePointToSegment(Vector2 p, Vector2 a, Vector2 b)
//     {
//         Vector2 ab = b - a;
//         float t = Mathf.Clamp01(Vector2.Dot(p - a, ab) / ab.sqrMagnitude);
//         return Vector2.Distance(p, a + t * ab);
//     }

//     Vector3 GetMouseWorld()
//     {
//         Vector3 p = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//         p.z = 0;
//         return p;
//     }

//     // =========================
//     // REQUIRED
//     // =========================
//     public List<int> GetSnappedNodes()
//     {
//         List<int> result = new();
//         foreach (var n in nodes)
//         {
//             if (n.innerNode != null) result.Add(n.innerNode.index);
//             else if (n.dot != null) result.Add(n.dot.index);
//         }
//         return result;
//     }

//     // =========================
//     // RESET
//     // =========================
//     public void ResetCompletely()
//     {
//         foreach (var n in nodes)
//         {
//             if (n != null)
//             {
//                 n.dot = null;
//                 n.innerNode = null;
//                 Destroy(n.gameObject);
//             }
//         }

//         nodes.Clear();
//         tempNode = null;

//         foreach (CircleDot d in dotGenerator.dots)
//             d.isOccupied = false;

//         if (innerGrid != null)
//             foreach (Transform t in innerGrid)
//                 t.GetComponent<InnerSnapNode>().isOccupied = false;

//         line.positionCount = 0;
//         initialNodeCount = 0;
//     }
// }

// using UnityEngine;
// using System.Collections;
// using System.Collections.Generic;

// public class CircularLineController : MonoBehaviour
// {
//     public static CircularLineController Instance;

//     [Header("Prefabs")]
//     public GameObject ropeNodePrefab;

//     [Header("References")]
//     public CircularDotGenerator dotGenerator;
//     public Transform innerGrid;
//     public SpriteRenderer board;

//     [Header("Line Visual")]
//     public Material lightningMaterial;

//     [Header("Settings")]
//     public float snapRadius = 0.4f;
//     public float dragStartDistance = 0.15f;

//     private LineRenderer line;
//     private readonly List<RopeNode> nodes = new();
//     private RopeNode tempNode;

//     private Vector3 boardCenter;
//     private float boardRadius;

//     private int initialNodeCount = 0;

//     private bool isDragging = false;
//     private Vector3 dragStartPos;

//     // =========================
//     // 🔁 GRAPH UNDO MEMORY
//     // =========================
//     struct UndoStep
//     {
//         public RopeNode node;
//     }

//     private Stack<UndoStep> undoStack = new Stack<UndoStep>();

//     // =========================
//     // UNITY
//     // =========================
//     void Awake()
//     {
//         Instance = this;

//         line = gameObject.AddComponent<LineRenderer>();
//         line.useWorldSpace = true;
//         line.material = lightningMaterial != null
//             ? lightningMaterial
//             : new Material(Shader.Find("Sprites/Default"));

//         line.startWidth = 0.13f;
//         line.endWidth = 0.13f;
//         line.sortingOrder = 999;
//         line.textureMode = LineTextureMode.Stretch;
//         line.alignment = LineAlignment.View;
//         line.positionCount = 0;
//     }

//     IEnumerator Start()
//     {
//         yield return new WaitUntil(() =>
//             dotGenerator != null &&
//             dotGenerator.dots.Count > 0
//         );

//         boardCenter = board.transform.position;
//         boardRadius = board.bounds.size.x * 0.5f;
//     }

//     void Update()
//     {
//         if (Input.GetMouseButtonDown(0))
//             BeginInput();

//         if (Input.GetMouseButton(0))
//             UpdateDrag();

//         if (Input.GetMouseButtonUp(0))
//             EndDrag();
//     }

//     // =========================
//     // INPUT
//     // =========================
//     void BeginInput()
//     {
//         dragStartPos = GetMouseWorld();
//         isDragging = false;
//     }

//     void UpdateDrag()
//     {
//         Vector3 currentPos = GetMouseWorld();

//         if (!isDragging)
//         {
//             if (Vector3.Distance(dragStartPos, currentPos) < dragStartDistance)
//                 return;

//             if (!StartDrag(dragStartPos))
//                 return;

//             isDragging = true;
//         }

//         Drag(currentPos);
//     }

//     void EndDrag()
//     {
//         if (!isDragging) return;

//         FinalizeDrag();
//         isDragging = false;

//         StartCoroutine(CheckCompletionNextFrame());
//     }

//     // =========================
//     // DRAG CORE
//     // =========================
//     bool StartDrag(Vector3 pos)
//     {
//         if (nodes.Count < 2) return false;
//         if (Vector2.Distance(pos, boardCenter) > boardRadius) return false;

//         int insertIndex = FindClosestSegment(pos);
//         if (insertIndex < 0) return false;

//         GameObject obj = Instantiate(ropeNodePrefab, pos, Quaternion.identity);
//         obj.SetActive(true);
//         tempNode = obj.GetComponent<RopeNode>();
//         tempNode.dot = null;
//         tempNode.innerNode = null;

//         nodes.Insert(insertIndex, tempNode);
//         UpdateLine();
//         return true;
//     }

//     void Drag(Vector3 pos)
//     {
//         if (tempNode == null) return;

//         Vector2 dir = pos - boardCenter;
//         if (dir.magnitude > boardRadius)
//             pos = boardCenter + (Vector3)(dir.normalized * boardRadius);

//         tempNode.transform.position = pos;
//         UpdateLine();
//     }

//     void FinalizeDrag()
//     {
//         if (tempNode == null) return;

//         Transform snap = GetNearestSnapPoint(tempNode.transform.position);

//         if (snap != null)
//         {
//             tempNode.transform.position = snap.position;

//             CircleDot d = snap.GetComponent<CircleDot>();
//             InnerSnapNode inner = snap.GetComponent<InnerSnapNode>();

//             if (d != null)
//             {
//                 d.isOccupied = true;
//                 tempNode.dot = d;
//             }
//             else if (inner != null)
//             {
//                 inner.isOccupied = true;
//                 tempNode.innerNode = inner;
//             }

//             // 🔥 RECORD GRAPH UNDO STEP
//             undoStack.Push(new UndoStep { node = tempNode });
//         }
//         else
//         {
//             nodes.Remove(tempNode);
//             Destroy(tempNode.gameObject);
//         }

//         tempNode = null;
//         UpdateLine();
//     }

//     // =========================
//     // 🔁 GRAPH UNDO (EXACTLY AS YOU DESCRIBED)
//     // =========================
//     public void UndoLastMove()
//     {
//         if (undoStack.Count == 0)
//             return;

//         UndoStep step = undoStack.Pop();
//         RopeNode node = step.node;

//         if (node == null)
//             return;

//         if (node.dot != null)
//             node.dot.isOccupied = false;

//         if (node.innerNode != null)
//             node.innerNode.isOccupied = false;

//         nodes.Remove(node);
//         Destroy(node.gameObject);

//         UpdateLine();
//     }

//     // =========================
//     // INITIAL ROPE
//     // =========================
//     public void CreateInitialRope(int[] indices)
//     {
//         ResetCompletely();

//         InnerGridGenerator inner = FindAnyObjectByType<InnerGridGenerator>();

//         foreach (int i in indices)
//         {
//             RopeNode rn;

//             if (i >= 0 && i <= 11)
//             {
//                 CircleDot dot = dotGenerator.dots[i];
//                 dot.isOccupied = true;

//                 rn = Instantiate(
//                     ropeNodePrefab,
//                     dot.transform.position,
//                     Quaternion.identity
//                 ).GetComponent<RopeNode>();

//                 rn.dot = dot;
//             }
//             else
//             {
//                 InnerSnapNode found = inner.nodes.Find(n => n.index == i);
//                 if (found == null) continue;

//                 found.isOccupied = true;

//                 rn = Instantiate(
//                     ropeNodePrefab,
//                     found.transform.position,
//                     Quaternion.identity
//                 ).GetComponent<RopeNode>();

//                 rn.innerNode = found;
//             }

//             rn.gameObject.SetActive(true);
//             nodes.Add(rn);
//         }

//         initialNodeCount = nodes.Count;
//         UpdateLine();
//         StartCoroutine(CheckCompletionNextFrame());
//     }

//     IEnumerator CheckCompletionNextFrame()
//     {
//         yield return null;
//         FindAnyObjectByType<LevelCompleteChecker>()?.CheckNow();
//     }

//     // =========================
//     // LINE
//     // =========================
//     void UpdateLine()
//     {
//         line.positionCount = nodes.Count;
//         for (int i = 0; i < nodes.Count; i++)
//             line.SetPosition(i, nodes[i].transform.position);
//     }

//     // =========================
//     // SNAP
//     // =========================
//     Transform GetNearestSnapPoint(Vector3 pos)
//     {
//         Transform nearest = null;
//         float min = snapRadius;

//         if (innerGrid != null)
//         {
//             foreach (Transform t in innerGrid)
//             {
//                 InnerSnapNode inner = t.GetComponent<InnerSnapNode>();
//                 if (inner == null || inner.isOccupied) continue;

//                 float d = Vector2.Distance(pos, t.position);
//                 if (d < min) { min = d; nearest = t; }
//             }
//         }

//         foreach (CircleDot d in dotGenerator.dots)
//         {
//             if (d.isOccupied) continue;

//             float dist = Vector2.Distance(pos, d.transform.position);
//             if (dist < min) { min = dist; nearest = d.transform; }
//         }

//         return nearest;
//     }

//     int FindClosestSegment(Vector2 p)
//     {
//         float min = float.MaxValue;
//         int index = -1;

//         for (int i = 0; i < nodes.Count - 1; i++)
//         {
//             float d = DistancePointToSegment(
//                 p,
//                 nodes[i].transform.position,
//                 nodes[i + 1].transform.position
//             );
//             if (d < min) { min = d; index = i + 1; }
//         }
//         return index;
//     }

//     float DistancePointToSegment(Vector2 p, Vector2 a, Vector2 b)
//     {
//         Vector2 ab = b - a;
//         float t = Mathf.Clamp01(Vector2.Dot(p - a, ab) / ab.sqrMagnitude);
//         return Vector2.Distance(p, a + t * ab);
//     }

//     Vector3 GetMouseWorld()
//     {
//         Vector3 p = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//         p.z = 0;
//         return p;
//     }

//     // =========================
//     // REQUIRED BY LevelCompleteChecker
//     // =========================
//     public List<int> GetSnappedNodes()
//     {
//         List<int> result = new();
//         foreach (var n in nodes)
//         {
//             if (n.innerNode != null) result.Add(n.innerNode.index);
//             else if (n.dot != null) result.Add(n.dot.index);
//         }
//         return result;
//     }

//     // =========================
//     // RESET
//     // =========================
//     public void ResetCompletely()
//     {
//         foreach (var n in nodes)
//             if (n != null) Destroy(n.gameObject);

//         nodes.Clear();
//         undoStack.Clear();
//         tempNode = null;

//         foreach (CircleDot d in dotGenerator.dots)
//             d.isOccupied = false;

//         if (innerGrid != null)
//             foreach (Transform t in innerGrid)
//                 t.GetComponent<InnerSnapNode>().isOccupied = false;

//         line.positionCount = 0;
//         initialNodeCount = 0;
//     }
// }
using UnityEngine;
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
    public float dragStartDistance = 0.15f;

    private LineRenderer line;
    private readonly List<RopeNode> nodes = new();
    private RopeNode tempNode;

    private Vector3 boardCenter;
    private float boardRadius;

    private int initialNodeCount = 0;

    private bool isDragging = false;
    private Vector3 dragStartPos;

    // =========================
    // 🔁 UNDO DATA (ADDED)
    // =========================
    struct UndoStep
    {
        public RopeNode node;
    }

    private Stack<UndoStep> undoStack = new Stack<UndoStep>();

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

        line.startWidth = 0.13f;
        line.endWidth = 0.13f;
        line.sortingOrder = 999;
        line.textureMode = LineTextureMode.Stretch;
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
        if (Input.GetMouseButtonDown(0))
            BeginInput();

        if (Input.GetMouseButton(0))
            UpdateDrag();

        if (Input.GetMouseButtonUp(0))
            EndDrag();
    }

    // =========================
    // INPUT
    // =========================
    void BeginInput()
    {
        dragStartPos = GetMouseWorld();
        isDragging = false;
    }

    void UpdateDrag()
    {
        Vector3 currentPos = GetMouseWorld();

        if (!isDragging)
        {
            if (Vector3.Distance(dragStartPos, currentPos) < dragStartDistance)
                return;

            if (!StartDrag(dragStartPos))
                return;

            isDragging = true;
        }

        Drag(currentPos);
    }

    void EndDrag()
    {
        if (!isDragging) return;

        FinalizeDrag();
        isDragging = false;

        StartCoroutine(CheckCompletionNextFrame());
    }

    // =========================
    // DRAG CORE
    // =========================
    bool StartDrag(Vector3 pos)
    {
        if (nodes.Count < 2) return false;
        if (Vector2.Distance(pos, boardCenter) > boardRadius) return false;

        int insertIndex = FindClosestSegment(pos);
        if (insertIndex < 0) return false;

        GameObject obj = Instantiate(ropeNodePrefab, pos, Quaternion.identity);
        obj.SetActive(true);
        tempNode = obj.GetComponent<RopeNode>();
        tempNode.dot = null;
        tempNode.innerNode = null;

        nodes.Insert(insertIndex, tempNode);
        UpdateLine();
        return true;
    }

    void Drag(Vector3 pos)
    {
        if (tempNode == null) return;

        Vector2 dir = pos - boardCenter;
        if (dir.magnitude > boardRadius)
            pos = boardCenter + (Vector3)(dir.normalized * boardRadius);

        tempNode.transform.position = pos;
        UpdateLine();
    }

    void FinalizeDrag()
    {
        if (tempNode == null) return;

        Transform snap = GetNearestSnapPoint(tempNode.transform.position);

        if (snap != null)
        {
            tempNode.transform.position = snap.position;

            CircleDot d = snap.GetComponent<CircleDot>();
            InnerSnapNode inner = snap.GetComponent<InnerSnapNode>();

            tempNode.dot = null;
            tempNode.innerNode = null;

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

            // =========================
            // 🔁 SAVE UNDO STEP (ADDED)
            // =========================
            undoStack.Push(new UndoStep { node = tempNode });
        }
        else
        {
            nodes.Remove(tempNode);
            Destroy(tempNode.gameObject);
        }

        tempNode = null;
        UpdateLine();
    }

    // =========================
    // 🔁 UNDO BUTTON (ADDED)
    // =========================
    public void UndoLastMove()
    {
        if (undoStack.Count == 0)
            return;

        UndoStep step = undoStack.Pop();
        RopeNode node = step.node;

        if (node == null) return;

        if (node.dot != null)
            node.dot.isOccupied = false;

        if (node.innerNode != null)
            node.innerNode.isOccupied = false;

        nodes.Remove(node);
        Destroy(node.gameObject);

        UpdateLine();
    }

    // =========================
    // INITIAL ROPE (UNCHANGED)
    // =========================
    public void CreateInitialRope(int[] indices)
    {
        ResetCompletely();

        InnerGridGenerator inner = FindAnyObjectByType<InnerGridGenerator>();

        foreach (int i in indices)
        {
            RopeNode rn;

            if (i >= 0 && i <= 11)
            {
                CircleDot dot = dotGenerator.dots[i];
                dot.isOccupied = true;

                rn = Instantiate(
                    ropeNodePrefab,
                    dot.transform.position,
                    Quaternion.identity
                ).GetComponent<RopeNode>();

                rn.dot = dot;
                rn.innerNode = null;
            }
            else
            {
                InnerSnapNode found = inner.nodes.Find(n => n.index == i);
                if (found == null) continue;

                found.isOccupied = true;

                rn = Instantiate(
                    ropeNodePrefab,
                    found.transform.position,
                    Quaternion.identity
                ).GetComponent<RopeNode>();

                rn.dot = null;
                rn.innerNode = found;
            }

            rn.gameObject.SetActive(true);
            nodes.Add(rn);
        }

        initialNodeCount = nodes.Count;
        UpdateLine();

        StartCoroutine(CheckCompletionNextFrame());
    }

    // =========================
    // COMPLETION CHECK
    // =========================
    IEnumerator CheckCompletionNextFrame()
    {
        yield return null;
        FindAnyObjectByType<LevelCompleteChecker>()?.CheckNow();
    }

    // =========================
    // LINE
    // =========================
    void UpdateLine()
    {
        line.positionCount = nodes.Count;
        for (int i = 0; i < nodes.Count; i++)
            line.SetPosition(i, nodes[i].transform.position);
    }

    // =========================
    // SNAP
    // =========================
    Transform GetNearestSnapPoint(Vector3 pos)
    {
        Transform nearest = null;
        float min = snapRadius;

        if (innerGrid != null)
        {
            foreach (Transform t in innerGrid)
            {
                InnerSnapNode inner = t.GetComponent<InnerSnapNode>();
                if (inner == null || inner.isOccupied) continue;

                float d = Vector2.Distance(pos, t.position);
                if (d < min) { min = d; nearest = t; }
            }
        }

        foreach (CircleDot d in dotGenerator.dots)
        {
            if (d.isOccupied) continue;

            float dist = Vector2.Distance(pos, d.transform.position);
            if (dist < min) { min = dist; nearest = d.transform; }
        }

        return nearest;
    }

    int FindClosestSegment(Vector2 p)
    {
        float min = float.MaxValue;
        int index = -1;

        for (int i = 0; i < nodes.Count - 1; i++)
        {
            float d = DistancePointToSegment(
                p,
                nodes[i].transform.position,
                nodes[i + 1].transform.position
            );
            if (d < min) { min = d; index = i + 1; }
        }
        return index;
    }

    float DistancePointToSegment(Vector2 p, Vector2 a, Vector2 b)
    {
        Vector2 ab = b - a;
        float t = Mathf.Clamp01(Vector2.Dot(p - a, ab) / ab.sqrMagnitude);
        return Vector2.Distance(p, a + t * ab);
    }

    Vector3 GetMouseWorld()
    {
        Vector3 p = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        p.z = 0;
        return p;
    }

    // =========================
    // REQUIRED
    // =========================
    public List<int> GetSnappedNodes()
    {
        List<int> result = new();
        foreach (var n in nodes)
        {
            if (n.innerNode != null) result.Add(n.innerNode.index);
            else if (n.dot != null) result.Add(n.dot.index);
        }
        return result;
    }

    // =========================
    // RESET
    // =========================
    public void ResetCompletely()
    {
        foreach (var n in nodes)
        {
            if (n != null)
                Destroy(n.gameObject);
        }

        nodes.Clear();
        tempNode = null;
        undoStack.Clear();

        foreach (CircleDot d in dotGenerator.dots)
            d.isOccupied = false;

        if (innerGrid != null)
            foreach (Transform t in innerGrid)
                t.GetComponent<InnerSnapNode>().isOccupied = false;

        line.positionCount = 0;
        initialNodeCount = 0;
    }
}
