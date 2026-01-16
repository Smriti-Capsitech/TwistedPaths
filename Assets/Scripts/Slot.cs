// using UnityEngine;

// public enum SlotType
// {
//     PartA,
//     PartB
// }

// public class Slot : MonoBehaviour
// {
//     public SlotType slotType;   // 👈 SET PER SLOT INSTANCE
//     public NodeDrag currentNode;

//     public bool IsFree() => currentNode == null;
// }
using UnityEngine;
 
public enum SlotType
{
    PartA,
    PartB
}
 
public class Slot : MonoBehaviour
{
    [Header("Slot Identity")]
    public SlotType slotType;   // 👈 Set per slot in Inspector
 
    [HideInInspector]
    public NodeDrag currentNode;
 
    // ==================================================
    // UNITY LIFECYCLE
    // ==================================================
    void OnEnable()
    {
        // 🔥 CRITICAL: clear stale references on restart / reload
        currentNode = null;
    }
 
    void OnDisable()
    {
        // 🔒 Safety cleanup
        currentNode = null;
    }
 
    // ==================================================
    // SLOT STATE
    // ==================================================
    public bool IsFree()
    {
        return currentNode == null;
    }
 
    // ==================================================
    // SAFETY VALIDATION (OPTIONAL BUT RECOMMENDED)
    // ==================================================
#if UNITY_EDITOR
    void OnValidate()
    {
        // Prevent wrong prefab edits
        if (!Application.isPlaying)
        {
            currentNode = null;
        }
    }
#endif
}