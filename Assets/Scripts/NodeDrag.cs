
using UnityEngine;
using System.Collections;
 
public enum NodeType
{
    PartA,
    PartB
}
 
public class NodeDrag : MonoBehaviour
{
    // ==================================================
    // INSPECTOR
    // ==================================================
    [Header("Node Identity")]
    public NodeType nodeType;
 
    [Header("Snap Settings")]
    public float snapRadius = 0.5f;
 
    // ==================================================
    // STATE
    // ==================================================
    private Slot currentSlot;
    private RopeController_1 rope;
 
    private bool isDragging;
    private Vector3 dragOffset;
    private Slot dragStartSlot;
 
 
    // 🔒 Cached slots (stable & safe)
    private static Slot[] cachedSlots;
 
    // ==================================================
    // UNITY LIFECYCLE
    // ==================================================
    void Awake()
    {
        CacheSlots();
    }
 
    void Start()
    {
        FindOwningRope();
        StartCoroutine(WaitAndAssignSlot());
    }
 
    void OnEnable()
    {
        // On restart / re-enable → wait safely
        StartCoroutine(WaitAndAssignSlot());
    }
 
    void LateUpdate()
    {
        // 🔐 HARD RULE: Node must always sit in its slot
        if (!isDragging && currentSlot != null)
        {
            transform.position = currentSlot.transform.position;
        }
    }
 
    // ==================================================
    // INPUT
    // ==================================================
    void OnMouseDown()
    {
        if (rope == null || !rope.isMovable)
            return;
 
        dragStartSlot = currentSlot;   // 🔑 remember original slot
 
        if (GameManager.Instance != null)
            GameManager.Instance.PromoteRope(rope);
 
        Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dragOffset = transform.position - new Vector3(mouse.x, mouse.y, 0f);
 
        isDragging = true;
    }
 
 
    void OnMouseDrag()
    {
        if (!isDragging)
            return;
 
        if (rope == null || !rope.isMovable)
        {
            isDragging = false;
            return;
        }
 
        Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mouse.x, mouse.y, 0f) + dragOffset;
    }
 
    void OnMouseUp()
    {
        if (!isDragging)
            return;
 
        isDragging = false;
 
        if (GameManager.Instance == null)
            return;
 
        Slot newSlot = FindSnapSlot();
 
        if (newSlot != null)
        {
            AttachToSlot(newSlot);
 
            // ✅ Count move ONLY if slot actually changed
            if (dragStartSlot != newSlot)
            {
                GameManager.Instance.RegisterMove();
            }
        }
        else
        {
            // 🔒 Invalid drop → revert to original slot
            AttachToSlot(dragStartSlot);
        }
 
        GameManager.Instance.UpdateTopOrderMovability();
        GameManager.Instance.CheckLevelComplete();
    }
 
 
    // ==================================================
    // SLOT ASSIGNMENT (SINGLE SOURCE OF TRUTH)
    // ==================================================
    void AttachToSlot(Slot newSlot)
    {
        if (newSlot == null)
            return;
 
        if (newSlot == currentSlot)
        {
            transform.position = newSlot.transform.position;
            return;
        }
 
        // Clear old slot
        if (currentSlot != null && currentSlot.currentNode == this)
            currentSlot.currentNode = null;
 
        // Slot already occupied → reject
        if (newSlot.currentNode != null && newSlot.currentNode != this)
            return;
 
        newSlot.currentNode = this;
        currentSlot = newSlot;
        transform.position = newSlot.transform.position;
    }
 
    // ==================================================
    // SAFE ASSIGNMENT (NO RACE CONDITIONS)
    // ==================================================
    IEnumerator WaitAndAssignSlot()
    {
        // Wait until slots exist
        while (cachedSlots == null || cachedSlots.Length == 0)
        {
            CacheSlots();
            yield return null;
        }
 
        // Wait one extra frame for Slot.OnEnable() cleanup
        yield return null;
 
        if (currentSlot == null)
            yield return RetryAssignSlot();
    }
 
    IEnumerator RetryAssignSlot()
    {
        float timeout = 2f;
        float timer = 0f;
 
        while (timer < timeout)
        {
            Slot slot = FindNearestCompatibleSlot();
            if (slot != null)
            {
                AttachToSlot(slot);
                yield break;
            }
 
            timer += Time.deltaTime;
            yield return null;
        }
 
        Debug.LogError($"❌ Node {name} FAILED to find compatible slot!");
    }
 
    // ==================================================
    // SLOT SEARCH
    // ==================================================
    Slot FindSnapSlot()
    {
        foreach (Slot slot in cachedSlots)
        {
            if (slot == null) continue;
            if (!IsCompatible(slot)) continue;
            if (slot.currentNode != null && slot.currentNode != this) continue;
 
            float dist = Vector2.Distance(transform.position, slot.transform.position);
            if (dist <= snapRadius)
                return slot;
        }
 
        return null;
    }
 
    Slot FindNearestCompatibleSlot()
    {
        Slot best = null;
        float minDist = Mathf.Infinity;
 
        foreach (Slot slot in cachedSlots)
        {
            if (slot == null) continue;
            if (!IsCompatible(slot)) continue;
            if (slot.currentNode != null && slot.currentNode != this) continue;
 
            float dist = Vector2.Distance(transform.position, slot.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                best = slot;
            }
        }
 
        return best;
    }
 
    bool IsCompatible(Slot slot)
    {
        if (slot.slotType == SlotType.PartA && nodeType != NodeType.PartA)
            return false;
 
        if (slot.slotType == SlotType.PartB && nodeType != NodeType.PartB)
            return false;
 
        return true;
    }
 
    // ==================================================
    // ROPE
    // ==================================================
    void FindOwningRope()
    {
        RopeController_1[] ropes =
            Object.FindObjectsByType<RopeController_1>(FindObjectsSortMode.None);
 
        foreach (var r in ropes)
        {
            if (r.nodeA == transform || r.nodeB == transform)
            {
                rope = r;
                return;
            }
        }
 
        Debug.LogError($"❌ Node {name} has NO rope reference!");
    }
 
    // ==================================================
    // SLOT CACHE
    // ==================================================
    void CacheSlots()
    {
        cachedSlots = Object.FindObjectsByType<Slot>(FindObjectsSortMode.None);
    }
 
#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, snapRadius);
    }
#endif
}