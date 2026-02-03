// using UnityEngine;
 
// public class LevelManager_1 : MonoBehaviour
// {
//     public static LevelManager_1 Instance;
 
//     public LevelData_1[] levels;
//     public GameObject slotPrefab;
//     public GameObject nodePrefab;
 
//     GameObject levelParent;
//     int currentLevelIndex;
 
//     void Awake()
//     {
//         if (Instance != null && Instance != this)
//             Destroy(Instance.gameObject);
 
//         Instance = this;
//     }
 
//     void Start()
//     {
//         currentLevelIndex = PlayerPrefs.GetInt("CURRENT_LEVEL", 0);
//         LoadLevel(currentLevelIndex);
//     }
 
//     public void LoadLevel(int index)
//     {
//         Time.timeScale = 1f;
 
//         if (LevelCompleteUI_1.Instance != null)
//             LevelCompleteUI_1.Instance.Hide();
 
//         ClearLevel();
//         GameManager.Instance.ResetState();
 
//         levelParent = new GameObject("Level");
 
//         LevelData_1 data = levels[index];
 
//         // ✅ UPDATE LEVEL TEXT
//         if (LevelUI.Instance != null)
//             LevelUI.Instance.SetLevel(index + 1);
 
//         GameManager.Instance.currentLevelData = data;
//         GameManager.Instance.SetMoveLimit(data.maxMoves);
 
//         foreach (var pos in data.partASlotPositions)
//             Instantiate(slotPrefab, pos, Quaternion.identity, levelParent.transform)
//                 .GetComponent<Slot>().slotType = SlotType.PartA;
 
//         foreach (var pos in data.partBSlotPositions)
//             Instantiate(slotPrefab, pos, Quaternion.identity, levelParent.transform)
//                 .GetComponent<Slot>().slotType = SlotType.PartB;
 
//         foreach (var rope in data.ropes)
//         {
//             GameObject a = Instantiate(nodePrefab, rope.nodeAPosition,
//                 Quaternion.identity, levelParent.transform);
//             a.GetComponent<NodeDrag>().nodeType = NodeType.PartA;
 
//             GameObject b = Instantiate(nodePrefab, rope.nodeBPosition,
//                 Quaternion.identity, levelParent.transform);
//             b.GetComponent<NodeDrag>().nodeType = NodeType.PartB;
 
//             GameObject r = Instantiate(rope.ropePrefab,
//                 Vector3.zero, Quaternion.identity, levelParent.transform);
 
//             RopeController_1 rc = r.GetComponent<RopeController_1>();
//             rc.nodeA = a.transform;
//             rc.nodeB = b.transform;
//             rc.ropeID = rope.ropeID;
//         }
 
//         GameManager.Instance.RegisterRopes(
//             levelParent.GetComponentsInChildren<RopeController_1>());
//     }
 
//     public void RestartLevel()
//     {
//         LoadLevel(currentLevelIndex);
//     }
 
//     public void NextLevel()
//     {
//         currentLevelIndex++;
 
//         if (currentLevelIndex >= levels.Length)
//             currentLevelIndex = 0;   // loop or you can stop
 
//         PlayerPrefs.SetInt("CURRENT_LEVEL", currentLevelIndex);
 
//         LoadLevel(currentLevelIndex);
//     }
 
//     public bool IsLastLevel()
//     {
//         return currentLevelIndex >= levels.Length - 1;
//     }
 
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

        // Reset gameplay state
        if (GameManager.Instance != null)
            GameManager.Instance.ResetState();

        levelParent = new GameObject("Level");

        LevelData_1 data = levels[index];

        // Update Level Text
        if (LevelUI.Instance != null)
            LevelUI.Instance.SetLevel(index + 1);

        // Store current level
        if (GameManager.Instance != null)
        {
            GameManager.Instance.currentLevelData = data;
            GameManager.Instance.SetMoveLimit(data.maxMoves);
        }

        // Init Move UI
        if (MoveCounterUI.Instance != null)
            MoveCounterUI.Instance.ResetMoves(data.maxMoves);

        // -------------------------
        // SPAWN PART A SLOTS
        // -------------------------
        foreach (var pos in data.partASlotPositions)
        {
            GameObject slot = Instantiate(
                slotPrefab,
                pos,
                Quaternion.identity,
                levelParent.transform
            );

            slot.GetComponent<Slot>().slotType = SlotType.PartA;
        }

        // -------------------------
        // SPAWN PART B SLOTS
        // -------------------------
        foreach (var pos in data.partBSlotPositions)
        {
            GameObject slot = Instantiate(
                slotPrefab,
                pos,
                Quaternion.identity,
                levelParent.transform
            );

            slot.GetComponent<Slot>().slotType = SlotType.PartB;
        }

        // -------------------------
        // SPAWN NODES & ROPES
        // -------------------------
        foreach (var rope in data.ropes)
        {
            GameObject a = Instantiate(
                nodePrefab,
                rope.nodeAPosition,
                Quaternion.identity,
                levelParent.transform
            );
            a.GetComponent<NodeDrag>().nodeType = NodeType.PartA;

            GameObject b = Instantiate(
                nodePrefab,
                rope.nodeBPosition,
                Quaternion.identity,
                levelParent.transform
            );
            b.GetComponent<NodeDrag>().nodeType = NodeType.PartB;

            GameObject r = Instantiate(
                rope.ropePrefab,
                Vector3.zero,
                Quaternion.identity,
                levelParent.transform
            );

            RopeController_1 rc = r.GetComponent<RopeController_1>();
            rc.nodeA = a.transform;
            rc.nodeB = b.transform;
            rc.ropeID = rope.ropeID;
        }

        // Register ropes to GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RegisterRopes(
                levelParent.GetComponentsInChildren<RopeController_1>()
            );
        }
    }

    // =========================================
    // LEVEL CONTROL
    // =========================================
    public void RestartLevel()
    {
        LoadLevel(currentLevelIndex);
    }

    public void NextLevel()
    {
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
