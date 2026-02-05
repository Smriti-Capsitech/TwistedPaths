




// using UnityEngine;

// public class LevelManager_1 : MonoBehaviour
// {
//     public static LevelManager_1 Instance;

//     [Header("Level Data")]
//     public LevelData_1[] levels;

//     [Header("Prefabs")]
//     public GameObject slotPrefab;
//     public GameObject nodePrefab;

//     private GameObject levelParent;
//     private int currentLevelIndex;

//     void Awake()
//     {
//         if (Instance != null && Instance != this)
//         {
//             Destroy(gameObject);
//             return;
//         }

//         Instance = this;
//     }

//     void Start()
//     {
//         currentLevelIndex = PlayerPrefs.GetInt("CURRENT_LEVEL", 0);
//         LoadLevel(currentLevelIndex);
//     }

//     // =========================================
//     // LOAD LEVEL
//     // =========================================
//     public void LoadLevel(int index)
//     {
//         Time.timeScale = 1f;

//         if (LevelCompleteUI_1.Instance != null)
//             LevelCompleteUI_1.Instance.Hide();

//         ClearLevel();

//         // Reset gameplay state
//         if (GameManager.Instance != null)
//             GameManager.Instance.ResetState();

//         levelParent = new GameObject("Level");

//         LevelData_1 data = levels[index];

//         // Updatze Level Text
//         if (LevelUI.Instance != null)
//             LevelUI.Instance.SetLevel(index + 1);

//         // Store current level
//         if (GameManager.Instance != null)
//         {
//             GameManager.Instance.currentLevelData = data;
//             GameManager.Instance.SetMoveLimit(data.maxMoves);
//         }

//         // Init Move UI
//         if (MoveCounterUI.Instance != null)
//             MoveCounterUI.Instance.ResetMoves(data.maxMoves);

//         // -------------------------
//         // SPAWN PART A SLOTS
//         // -------------------------
//         foreach (var pos in data.partASlotPositions)
//         {
//             GameObject slot = Instantiate(
//                 slotPrefab,
//                 pos,
//                 Quaternion.identity,
//                 levelParent.transform
//             );

//             slot.GetComponent<Slot>().slotType = SlotType.PartA;
//         }

//         // -------------------------
//         // SPAWN PART B SLOTS
//         // -------------------------
//         foreach (var pos in data.partBSlotPositions)
//         {
//             GameObject slot = Instantiate(
//                 slotPrefab,
//                 pos,
//                 Quaternion.identity,
//                 levelParent.transform
//             );

//             slot.GetComponent<Slot>().slotType = SlotType.PartB;
//         }

//         // -------------------------
//         // SPAWN NODES & ROPES
//         // -------------------------
//         foreach (var rope in data.ropes)
//         {
//             GameObject a = Instantiate(
//                 nodePrefab,
//                 rope.nodeAPosition,
//                 Quaternion.identity,
//                 levelParent.transform
//             );
//             a.GetComponent<NodeDrag>().nodeType = NodeType.PartA;

//             GameObject b = Instantiate(
//                 nodePrefab,
//                 rope.nodeBPosition,
//                 Quaternion.identity,
//                 levelParent.transform
//             );
//             b.GetComponent<NodeDrag>().nodeType = NodeType.PartB;

//             GameObject r = Instantiate(
//                 rope.ropePrefab,
//                 Vector3.zero,
//                 Quaternion.identity,
//                 levelParent.transform
//             );

//             RopeController_1 rc = r.GetComponent<RopeController_1>();
//             rc.nodeA = a.transform;
//             rc.nodeB = b.transform;
//             rc.ropeID = rope.ropeID;
//         }

//         // Register ropes to GameManager
//         if (GameManager.Instance != null)
//         {
//             GameManager.Instance.RegisterRopes(
//                 levelParent.GetComponentsInChildren<RopeController_1>()
//             );
//         }
//     }

//     // =========================================
//     // LEVEL CONTROL
//     // =========================================
//     public void RestartLevel()
//     {
//         LoadLevel(currentLevelIndex);
//     }

//     public void NextLevel()
//     {
//         currentLevelIndex++;

//         if (currentLevelIndex >= levels.Length)
//             currentLevelIndex = 0;

//         PlayerPrefs.SetInt("CURRENT_LEVEL", currentLevelIndex);
//         LoadLevel(currentLevelIndex);
//     }

//     public bool IsLastLevel()
//     {
//         return currentLevelIndex >= levels.Length - 1;
//     }

//     // =========================================
//     // CLEANUP
//     // =========================================
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

    [Header("Level Data")]
    public LevelData_1[] levels;

    [Header("Prefabs")]
    public GameObject slotPrefab;
    public GameObject nodePrefab;

    private GameObject levelParent;
    private int currentLevelIndex;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        // 🔒 FORCE CHAPTER 2
        PlayerPrefs.SetInt("ACTIVE_CHAPTER", 2);
        PlayerPrefs.Save();

        currentLevelIndex = PlayerPrefs.GetInt("CURRENT_LEVEL", 0);
        LoadLevel(currentLevelIndex);
    }

    // =========================================
    // LOAD LEVEL
    // =========================================
    public void LoadLevel(int index)
    {
        Time.timeScale = 1f;

        if (LevelCompleteUI_1.Instance != null)
            LevelCompleteUI_1.Instance.Hide();

        ClearLevel();

        if (GameManager.Instance != null)
            GameManager.Instance.ResetState();

        levelParent = new GameObject("Level");

        LevelData_1 data = levels[index];

        if (LevelUI.Instance != null)
            LevelUI.Instance.SetLevel(index + 1);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.currentLevelData = data;
            GameManager.Instance.SetMoveLimit(data.maxMoves);
        }

        if (MoveCounterUI.Instance != null)
            MoveCounterUI.Instance.ResetMoves(data.maxMoves);

        foreach (var pos in data.partASlotPositions)
        {
            GameObject slot = Instantiate(slotPrefab, pos, Quaternion.identity, levelParent.transform);
            slot.GetComponent<Slot>().slotType = SlotType.PartA;
        }

        foreach (var pos in data.partBSlotPositions)
        {
            GameObject slot = Instantiate(slotPrefab, pos, Quaternion.identity, levelParent.transform);
            slot.GetComponent<Slot>().slotType = SlotType.PartB;
        }

        foreach (var rope in data.ropes)
        {
            GameObject a = Instantiate(nodePrefab, rope.nodeAPosition, Quaternion.identity, levelParent.transform);
            a.GetComponent<NodeDrag>().nodeType = NodeType.PartA;

            GameObject b = Instantiate(nodePrefab, rope.nodeBPosition, Quaternion.identity, levelParent.transform);
            b.GetComponent<NodeDrag>().nodeType = NodeType.PartB;

            GameObject r = Instantiate(rope.ropePrefab, Vector3.zero, Quaternion.identity, levelParent.transform);
            RopeController_1 rc = r.GetComponent<RopeController_1>();
            rc.nodeA = a.transform;
            rc.nodeB = b.transform;
            rc.ropeID = rope.ropeID;
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.RegisterRopes(
                levelParent.GetComponentsInChildren<RopeController_1>()
            );
        }
    }

    // =========================================
    // LEVEL CONTROL (🔥 REAL FIX)
    // =========================================
    public void RestartLevel()
    {
        LoadLevel(currentLevelIndex);
    }

    public void NextLevel()
    {
        // 🔓 UNLOCK CURRENT LEVEL FOR CHAPTER 2
        string unlockKey = "CH2_UNLOCKED_LEVEL";
        int unlocked = PlayerPrefs.GetInt(unlockKey, 0);

        if (currentLevelIndex + 1 > unlocked)
        {
            PlayerPrefs.SetInt(unlockKey, currentLevelIndex + 1);
            PlayerPrefs.Save();
        }

        Debug.Log($"🔓 [Chapter 2 Manager] Unlocked up to level index: {currentLevelIndex + 1}");

        currentLevelIndex++;

        if (currentLevelIndex >= levels.Length)
            currentLevelIndex = 0;

        PlayerPrefs.SetInt("CURRENT_LEVEL", currentLevelIndex);
        LoadLevel(currentLevelIndex);
    }

    public bool IsLastLevel()
    {
        return currentLevelIndex >= levels.Length - 1;
    }

    // =========================================
    // CLEANUP
    // =========================================
    void ClearLevel()
    {
        if (levelParent != null)
            Destroy(levelParent);
    }
}
