using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class RopeController_1 : MonoBehaviour
{
    public Transform nodeA;
    public Transform nodeB;

    [HideInInspector] public bool isMovable = true;
    [HideInInspector] public int moveOrder;

    private LineRenderer lr;
    private Rigidbody2D rb;
    private CapsuleCollider2D col;

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
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();

        lr.useWorldSpace = true;

        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.simulated = true;
    }

    void LateUpdate()
    {
        if (nodeA == null || nodeB == null) return;

        DrawRealisticRope();
        UpdateCollider();
    }

    // ==================================================
    // PHYSICS COLLISION
    // ==================================================
    void OnCollisionStay2D(Collision2D collision)
    {
        var other = collision.collider.GetComponent<RopeController_1>();
        if (other == null) return;

        // Bottom rope is locked
        if (IsBelow(other) && GetOrder() < other.GetOrder())
        {
            SetMovable(false);

            ContactPoint2D cp = collision.contacts[0];
            stuckPoint = new Vector3(cp.point.x, cp.point.y, transform.position.z);
            isStuck = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        var other = collision.collider.GetComponent<RopeController_1>();
        if (other == null) return;

        isStuck = false;
        SetMovable(true);
    }

    // ==================================================
    // COLLIDER UPDATE
    // ==================================================
    void UpdateCollider()
    {
        Vector3 mid = (nodeA.position + nodeB.position) * 0.5f;
        transform.position = mid;

        Vector3 dir = nodeB.position - nodeA.position;
        float length = dir.magnitude;

        col.size = new Vector2(length, normalWidth);
        transform.right = dir.normalized;
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
    // ORDER & LOGIC (RESTORED)
    // ==================================================
    public void SetOrder(int order)
    {
        moveOrder = order;
        lr.sortingOrder = order;
    }

    public int GetOrder()
    {
        return moveOrder;
    }

    public bool IsBelow(RopeController_1 other)
    {
        return Mathf.Max(nodeA.position.y, nodeB.position.y) <
               Mathf.Max(other.nodeA.position.y, other.nodeB.position.y);
    }

    public void SetMovable(bool movable)
    {
        isMovable = movable;
        SetHighlight(movable);
    }

    // ==================================================
    // VISUAL FEEDBACK
    // ==================================================
    void SetHighlight(bool active)
    {
        lr.startColor = active ? highlightColor : normalColor;
        lr.endColor = active ? highlightColor : normalColor;
        lr.startWidth = active ? highlightWidth : normalWidth;
        lr.endWidth = active ? highlightWidth : normalWidth;
    }
}
