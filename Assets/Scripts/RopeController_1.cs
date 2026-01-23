


// using UnityEngine;

// [RequireComponent(typeof(LineRenderer))]
// public class RopeController_1 : MonoBehaviour
// {
//     public Transform nodeA;
//     public Transform nodeB;

//     // Used by NodeDrag & GameManager
//     [HideInInspector] public bool isMovable = true;
//     [HideInInspector] public int moveOrder;

//     [Header("Rope Shape")]
//     public int segments = 25;
//     public float sagAmount = 0.6f;

//     private LineRenderer lr;

//     void Awake()
//     {
//         lr = GetComponent<LineRenderer>();
//         lr.useWorldSpace = true;
//     }

//     void LateUpdate()
//     {
//         if (nodeA == null || nodeB == null) return;
//         DrawCurvedRope();
//     }

//     void DrawCurvedRope()
//     {
//         lr.positionCount = segments;

//         for (int i = 0; i < segments; i++)
//         {
//             float t = i / (segments - 1f);

//             Vector3 straight = Vector3.Lerp(nodeA.position, nodeB.position, t);
//             float sag = 4f * sagAmount * t * (1f - t);

//             lr.SetPosition(i, straight + Vector3.down * sag);
//         }
//     }

//     // 🔥 THIS is the truth source for ALL logic
//     public Vector3[] GetRopePoints()
//     {
//         Vector3[] pts = new Vector3[lr.positionCount];
//         lr.GetPositions(pts);
//         return pts;
//     }

//     // Order helpers (used by NodeDrag / GameManager)
//     public void SetOrder(int order)
//     {
//         moveOrder = order;
//         lr.sortingOrder = order;
//     }

//     public int GetOrder()
//     {
//         return moveOrder;
//     }

//     public void SetMovable(bool movable)
//     {
//         isMovable = movable;
//     }
// }


using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RopeController_1 : MonoBehaviour
{
    public Transform nodeA;
    public Transform nodeB;

    [HideInInspector] public bool isMovable = true;
    [HideInInspector] public int moveOrder;

    [Header("Rope Shape")]
    public int segments = 25;
    public float sagAmount = 0.6f;

    private LineRenderer lr;

    // 🔥 Cache ALL renderers under nodes
    private SpriteRenderer[] nodeARenderers;
    private SpriteRenderer[] nodeBRenderers;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.useWorldSpace = true;

        CacheNodeRenderers();
    }

    void LateUpdate()
    {
        if (nodeA == null || nodeB == null) return;
        DrawCurvedRope();
    }

    void CacheNodeRenderers()
    {
        if (nodeA != null)
            nodeARenderers = nodeA.GetComponentsInChildren<SpriteRenderer>(true);

        if (nodeB != null)
            nodeBRenderers = nodeB.GetComponentsInChildren<SpriteRenderer>(true);
    }

    void DrawCurvedRope()
    {
        lr.positionCount = segments;

        for (int i = 0; i < segments; i++)
        {
            float t = i / (segments - 1f);
            Vector3 straight = Vector3.Lerp(nodeA.position, nodeB.position, t);
            float sag = 4f * sagAmount * t * (1f - t);
            lr.SetPosition(i, straight + Vector3.down * sag);
        }
    }

    // 🔥 Truth source for ALL logic
    public Vector3[] GetRopePoints()
    {
        Vector3[] pts = new Vector3[lr.positionCount];
        lr.GetPositions(pts);
        return pts;
    }

    // ==================================================
    // ORDER = ROPE + ALL NODE VISUALS
    // ==================================================
    public void SetOrder(int order)
    {
        moveOrder = order;

        // Rope visual order
        lr.sortingOrder = order;

        // 🔥 Apply order to ALL node sprites
        ApplyOrder(nodeARenderers, order);
        ApplyOrder(nodeBRenderers, order);
    }

    void ApplyOrder(SpriteRenderer[] renderers, int order)
    {
        if (renderers == null) return;

        foreach (var r in renderers)
        {
            if (r != null)
                r.sortingOrder = order;
        }
    }

    public int GetOrder()
    {
        return moveOrder;
    }

    public void SetMovable(bool movable)
    {
        isMovable = movable;
    }
}
