using UnityEngine;

public class LevelManager_1 : MonoBehaviour
{
    public static LevelManager_1 Instance;

    public LevelData_1[] levels;
    public GameObject slotPrefab;
    public GameObject nodePrefab;

    GameObject levelParent;
    int currentLevelIndex;

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(Instance.gameObject);

        Instance = this;
    }

    void Start()
    {
        // Tell system this is Chapter 2
        PlayerPrefs.SetInt("ACTIVE_CHAPTER", 2);

        currentLevelIndex = PlayerPrefs.GetInt("CURRENT_LEVEL", 0);
        LoadLevel(currentLevelIndex);
    }

    public void LoadLevel(int index)
    {
        Time.timeScale = 1f;

        // Show banner during gameplay
        if (AdManager.Instance != null)
            AdManager.Instance.ShowBanner();

        if (LevelCompleteUI_1.Instance != null)
            LevelCompleteUI_1.Instance.Hide();

        ClearLevel();
        GameManager.Instance.ResetState();

        levelParent = new GameObject("Level");

        LevelData_1 data = levels[index];

        // Update level text
        if (LevelUI.Instance != null)
            LevelUI.Instance.SetLevel(index + 1);

        GameManager.Instance.currentLevelData = data;
        GameManager.Instance.SetMoveLimit(data.maxMoves);

        // ---------- PART A SLOTS ----------
        foreach (var pos in data.partASlotPositions)
        {
            GameObject slotObj = Instantiate(
                slotPrefab,
                pos,
                Quaternion.identity,
                levelParent.transform
            );

            Slot slot = slotObj.GetComponent<Slot>();
            if (slot != null)
                slot.slotType = SlotType.PartA;
        }

        // ---------- PART B SLOTS ----------
        foreach (var pos in data.partBSlotPositions)
        {
            GameObject slotObj = Instantiate(
                slotPrefab,
                pos,
                Quaternion.identity,
                levelParent.transform
            );

            Slot slot = slotObj.GetComponent<Slot>();
            if (slot != null)
                slot.slotType = SlotType.PartB;
        }

        // ---------- ROPES ----------
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

        GameManager.Instance.RegisterRopes(
            levelParent.GetComponentsInChildren<RopeController_1>()
        );
    }

    public void RestartLevel()
    {
        LoadLevel(currentLevelIndex);
    }

    public void NextLevel()
    {
        currentLevelIndex++;

        if (currentLevelIndex >= levels.Length)
            currentLevelIndex = 0;   // loop back if needed

        PlayerPrefs.SetInt("CURRENT_LEVEL", currentLevelIndex);

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
