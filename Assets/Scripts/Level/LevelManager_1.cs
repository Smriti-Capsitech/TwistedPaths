



// using UnityEngine;

// public class LevelManager_1 : MonoBehaviour
// {
//     public static LevelManager_1 Instance;

//     [Header("Levels")]
//     public LevelData_1[] levels;

//     [Header("Prefabs")]
//     public GameObject slotPrefab;
//     public GameObject nodePrefab;
//     public GameObject ropePrefab;

//     private GameObject levelParent;
//     private int currentLevelIndex = 0;

//     // ==================================================
//     // UNITY LIFECYCLE
//     // ==================================================
//     void Awake()
//     {
//         if (Instance != null)
//         {
//             Destroy(gameObject);
//             return;
//         }
//         Instance = this;
//     }

//     void Start()
//     {
//         if (AdManager.Instance != null)
//         AdManager.Instance.ShowBanner();
//         currentLevelIndex = PlayerPrefs.GetInt("CURRENT_LEVEL", 0);
//         LoadLevel(currentLevelIndex);
//     }

//     // ==================================================
//     // LOAD LEVEL
//     // ==================================================
//     public void LoadLevel(int index)
//     {
//         Time.timeScale = 1f;

//         if (LevelCompleteUI_1.Instance != null)
//             LevelCompleteUI_1.Instance.Hide();

//         // -----------------------------
//         // CLEAR PREVIOUS LEVEL
//         // -----------------------------
//         ClearLevel();
//         GameManager.Instance.ResetState();

//         levelParent = new GameObject("Level");

//         LevelData_1 data = levels[index];

//         // 🔥 REQUIRED FOR MOVEMENT RULES
//         GameManager.Instance.currentLevelData = data;

//         // 🔥 SET MOVE LIMIT
//         GameManager.Instance.SetMoveLimit(data.maxMoves);

//         // ==================================================
//         // CREATE PART A SLOTS
//         // ==================================================
//         foreach (var pos in data.partASlotPositions)
//         {
//             GameObject slotObj =
//                 Instantiate(slotPrefab, pos, Quaternion.identity, levelParent.transform);

//             Slot slot = slotObj.GetComponent<Slot>();
//             if (slot != null)
//                 slot.slotType = SlotType.PartA;
//         }

//         // ==================================================
//         // CREATE PART B SLOTS
//         // ==================================================
//         foreach (var pos in data.partBSlotPositions)
//         {
//             GameObject slotObj =
//                 Instantiate(slotPrefab, pos, Quaternion.identity, levelParent.transform);

//             Slot slot = slotObj.GetComponent<Slot>();
//             if (slot != null)
//                 slot.slotType = SlotType.PartB;
//         }

//         // ==================================================
//         // CREATE ROPES + NODES
//         // ==================================================
//         foreach (var rope in data.ropes)
//         {
//             // NODE A
//             GameObject nodeAObj =
//                 Instantiate(nodePrefab, rope.nodeAPosition, Quaternion.identity, levelParent.transform);

//             NodeDrag nodeA = nodeAObj.GetComponent<NodeDrag>();
//             if (nodeA != null)
//                 nodeA.nodeType = NodeType.PartA;

//             // NODE B
//             GameObject nodeBObj =
//                 Instantiate(nodePrefab, rope.nodeBPosition, Quaternion.identity, levelParent.transform);

//             NodeDrag nodeB = nodeBObj.GetComponent<NodeDrag>();
//             if (nodeB != null)
//                 nodeB.nodeType = NodeType.PartB;

//             // ROPE
//             GameObject ropeObj =
//                 Instantiate(ropePrefab, Vector3.zero, Quaternion.identity, levelParent.transform);

//             RopeController_1 rc = ropeObj.GetComponent<RopeController_1>();
//             if (rc == null) continue;

//             rc.nodeA = nodeAObj.transform;
//             rc.nodeB = nodeBObj.transform;
//         }

//         // ==================================================
//         // REGISTER ROPES
//         // ==================================================
//         GameManager.Instance.RegisterRopes(
//             levelParent.GetComponentsInChildren<RopeController_1>()
//         );

//         // 🔥 MUST BE CALLED AFTER REGISTER
//         GameManager.Instance.UpdateTopOrderMovability();

//         if (LevelUI.Instance != null)
//             LevelUI.Instance.SetLevel(currentLevelIndex + 1);
//     }

//     // ==================================================
//     // LEVEL CONTROL
//     // ==================================================
//     public void RestartLevel()
//     {
//         LoadLevel(currentLevelIndex);


//     }

//     public void NextLevel()
//     {
//         currentLevelIndex++;
//         if (currentLevelIndex >= levels.Length) return;
//         LoadLevel(currentLevelIndex);
//     }

//     public bool IsLastLevel()
//     {
//         return currentLevelIndex >= levels.Length - 1;
//     }

//     // ==================================================
//     // CLEANUP
//     // ==================================================
//     void ClearLevel()
//     {
//         if (levelParent != null)
//             Destroy(levelParent);
//     }
// }
using UnityEngine;

public class LevelManager_1 : MonoBehaviour
{
    public static LevelManager_1 Instance;

    [Header("Levels")]
    public LevelData_1[] levels;

    [Header("Prefabs")]
    public GameObject slotPrefab;
    public GameObject nodePrefab;
    public GameObject ropePrefab;

    private GameObject levelParent;
    private int currentLevelIndex = 0;

    // ==================================================
    // UNITY LIFECYCLE
    // ==================================================
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        if (AdManager.Instance != null)
            AdManager.Instance.ShowBanner();   // OK

        currentLevelIndex = PlayerPrefs.GetInt("CURRENT_LEVEL", 0);
        LoadLevel(currentLevelIndex);
    }

    // ==================================================
    // LOAD LEVEL
    // ==================================================
    public void LoadLevel(int index)
    {
        Time.timeScale = 1f;

        // ✅ FIX: ENSURE BANNER RETURNS FOR EVERY LEVEL
        if (AdManager.Instance != null)
            AdManager.Instance.ShowBanner();

        if (LevelCompleteUI_1.Instance != null)
            LevelCompleteUI_1.Instance.Hide();

        // -----------------------------
        // CLEAR PREVIOUS LEVEL
        // -----------------------------
        ClearLevel();
        GameManager.Instance.ResetState();

        levelParent = new GameObject("Level");

        LevelData_1 data = levels[index];

        GameManager.Instance.currentLevelData = data;
        GameManager.Instance.SetMoveLimit(data.maxMoves);

        // ==================================================
        // CREATE PART A SLOTS
        // ==================================================
        foreach (var pos in data.partASlotPositions)
        {
            GameObject slotObj =
                Instantiate(slotPrefab, pos, Quaternion.identity, levelParent.transform);

            Slot slot = slotObj.GetComponent<Slot>();
            if (slot != null)
                slot.slotType = SlotType.PartA;
        }

        // ==================================================
        // CREATE PART B SLOTS
        // ==================================================
        foreach (var pos in data.partBSlotPositions)
        {
            GameObject slotObj =
                Instantiate(slotPrefab, pos, Quaternion.identity, levelParent.transform);

            Slot slot = slotObj.GetComponent<Slot>();
            if (slot != null)
                slot.slotType = SlotType.PartB;
        }

        // ==================================================
        // CREATE ROPES + NODES
        // ==================================================
        foreach (var rope in data.ropes)
        {
            GameObject nodeAObj =
                Instantiate(nodePrefab, rope.nodeAPosition, Quaternion.identity, levelParent.transform);

            NodeDrag nodeA = nodeAObj.GetComponent<NodeDrag>();
            if (nodeA != null)
                nodeA.nodeType = NodeType.PartA;

            GameObject nodeBObj =
                Instantiate(nodePrefab, rope.nodeBPosition, Quaternion.identity, levelParent.transform);

            NodeDrag nodeB = nodeBObj.GetComponent<NodeDrag>();
            if (nodeB != null)
                nodeB.nodeType = NodeType.PartB;

            GameObject ropeObj =
                Instantiate(ropePrefab, Vector3.zero, Quaternion.identity, levelParent.transform);

            RopeController_1 rc = ropeObj.GetComponent<RopeController_1>();
            if (rc == null) continue;

            rc.nodeA = nodeAObj.transform;
            rc.nodeB = nodeBObj.transform;
        }

        GameManager.Instance.RegisterRopes(
            levelParent.GetComponentsInChildren<RopeController_1>()
        );

        GameManager.Instance.UpdateTopOrderMovability();

        if (LevelUI.Instance != null)
            LevelUI.Instance.SetLevel(currentLevelIndex + 1);
    }

    public void RestartLevel()
    {
        LoadLevel(currentLevelIndex);
    }

    public void NextLevel()
    {
        currentLevelIndex++;
        if (currentLevelIndex >= levels.Length) return;
        LoadLevel(currentLevelIndex);
    }

    public bool IsLastLevel()
    {
        return currentLevelIndex >= levels.Length - 1;
    }

    void ClearLevel()
    {
        if (levelParent != null)
            Destroy(levelParent);
    }
}
