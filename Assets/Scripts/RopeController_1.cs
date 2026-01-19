
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RopeController_1 : MonoBehaviour
{
    public Transform nodeA;
    public Transform nodeB;

    [HideInInspector] public bool isMovable = true;
    [HideInInspector] public int moveOrder;

    private LineRenderer lr;

    private bool isStuck = false;
    private Vector3 stuckPoint;

    [Header("Visuals")]
    public Color normalColor = Color.white;
    public Color highlightColor = Color.yellow;
    public float normalWidth = 0.15f;
    public float highlightWidth = 0.25f;

    [Header("Rope Feel")]
    public int segments = 20;
    public float sagAmount = 0.5f;

    // ==================================================
    // UNITY
    // ==================================================
    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.useWorldSpace = true;
    }

    void LateUpdate()
    {
        if (nodeA == null || nodeB == null) return;
        if (GameManager.Instance == null) return;
        if (GameManager.Instance.ropes == null) return;

        isStuck = false;

        // ----------------------------------
        // INTERSECTION CHECK (ORDER-BASED)
        // ----------------------------------
        foreach (var other in GameManager.Instance.ropes)
        {
            if (other == null || other == this) continue;
            if (other.nodeA == null || other.nodeB == null) continue;

            // Only higher-order ropes can block this rope
            if (other.GetOrder() <= GetOrder())
                continue;

            if (GameManager.Instance.TryGetIntersection(
                nodeA.position,
                nodeB.position,
                other.nodeA.position,
                other.nodeB.position,
                out Vector2 hit))
            {
                isStuck = true;
                stuckPoint = new Vector3(
                    hit.x,
                    hit.y,
                    (nodeA.position.z + nodeB.position.z) * 0.5f
                );
                break;
            }
        }

        DrawRealisticRope();
    }

    // ==================================================
    // RENDERING
    // ==================================================
    void DrawRealisticRope()
    {
        lr.positionCount = segments;

        for (int i = 0; i < segments; i++)
        {
            float t = i / (float)(segments - 1);
            Vector3 pos;

            if (isStuck)
            {
                if (t <= 0.5f)
                    pos = Vector3.Lerp(nodeA.position, stuckPoint, t * 2f);
                else
                    pos = Vector3.Lerp(stuckPoint, nodeB.position, (t - 0.5f) * 2f);
            }
            else
            {
                Vector3 straight = Vector3.Lerp(nodeA.position, nodeB.position, t);
                float sag = 4f * sagAmount * t * (1f - t);
                pos = straight + Vector3.down * sag;
            }

            lr.SetPosition(i, pos);
        }
    }

    // ==================================================
    // ORDER & MOVEMENT CONTROL
    // ==================================================
    public void SetOrder(int order)
    {
        moveOrder = order;
        lr.sortingOrder = order;
    }

    public int GetOrder() => moveOrder;

    /// <summary>
    /// Highest Y value of this rope
    /// </summary>
    public float GetTopY()
    {
        return Mathf.Max(nodeA.position.y, nodeB.position.y);
    }

    /// <summary>
    /// Logic-only movement lock
    /// </summary>
    public void SetMovable(bool movable)
    {
        isMovable = movable;
        SetHighlight(movable);
    }

    // ==================================================
    // VISUAL FEEDBACK
    // ==================================================
    public void SetHighlight(bool active)
    {
        lr.startColor = active ? highlightColor : normalColor;
        lr.endColor = active ? highlightColor : normalColor;
        lr.startWidth = active ? highlightWidth : normalWidth;
        lr.endWidth = active ? highlightWidth : normalWidth;
    }
}
