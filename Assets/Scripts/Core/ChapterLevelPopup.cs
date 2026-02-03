
// using UnityEngine;
// using TMPro;

// public class ChapterLevelPopup : MonoBehaviour
// {
//     [Header("UI References")]
//     public GameObject chapterSelectPanel;
//     public TextMeshProUGUI progressText;
//     public Transform levelGrid;
//     public GameObject levelButtonPrefab;

//     [Header("Backgrounds")]
//     public GameObject chapter1Background;
//     public GameObject chapter2Background;

//     private const int TOTAL_LEVELS = 30;

//     void Start()
//     {
//         // 🔥 THIS IS THE MISSING PIECE
//         if (PlayerPrefs.GetInt("OPEN_CHAPTER_POPUP", 0) == 1)
//         {
//             PlayerPrefs.DeleteKey("OPEN_CHAPTER_POPUP");

//             // force enable self BEFORE opening
//             gameObject.SetActive(true);

//             OpenPopup();
//         }
//     }

//     public void OpenPopup()
//     {
//         Debug.Log("Popup opened");

//         if (chapterSelectPanel != null)
//             chapterSelectPanel.SetActive(false);

//         gameObject.SetActive(true);
//         transform.SetAsLastSibling();

//         ApplyChapterBackground();
//         PopulateLevels();
//     }

//     void ApplyChapterBackground()
//     {
//         int chapter = PlayerPrefs.GetInt("ACTIVE_CHAPTER", 1);

//         if (chapter1Background != null)
//             chapter1Background.SetActive(chapter == 1);

//         if (chapter2Background != null)
//             chapter2Background.SetActive(chapter == 2);
//     }

//     public void ClosePopup()
//     {
//         gameObject.SetActive(false);

//         if (chapterSelectPanel != null)
//             chapterSelectPanel.SetActive(true);
//     }

//     void PopulateLevels()
//     {
//         foreach (Transform child in levelGrid)
//             Destroy(child.gameObject);

//         int chapter = PlayerPrefs.GetInt("ACTIVE_CHAPTER", 1);

//         // 🔥 chapter-aware unlock key
//         string unlockKey = chapter == 1
//             ? "UNLOCKED_LEVEL"
//             : $"CH{chapter}_UNLOCKED_LEVEL";

//         int unlocked = PlayerPrefs.GetInt(unlockKey, 0);

//         for (int i = 0; i < TOTAL_LEVELS; i++)
//         {
//             GameObject btnObj = Instantiate(levelButtonPrefab, levelGrid);
//             LevelButton lb = btnObj.GetComponent<LevelButton>();

//             bool isUnlocked = i <= unlocked;
//             lb.Setup(i + 1, isUnlocked);
//         }
//     }
// }
using UnityEngine;
using TMPro;

public class ChapterLevelPopup : MonoBehaviour
{
    [Header("UI References")]
    public GameObject chapterSelectPanel;
    public TextMeshProUGUI progressText;
    public Transform levelGrid;
    public GameObject levelButtonPrefab;

    [Header("Backgrounds")]
    public GameObject chapter1Background;
    public GameObject chapter2Background;

    private const int TOTAL_LEVELS = 30;

    void Start()
    {
        if (PlayerPrefs.GetInt("OPEN_CHAPTER_POPUP", 0) == 1)
        {
            PlayerPrefs.DeleteKey("OPEN_CHAPTER_POPUP");
            gameObject.SetActive(true);
            OpenPopup();
        }
    }

    public void OpenPopup()
    {
        Debug.Log("Popup opened");

        if (chapterSelectPanel != null)
            chapterSelectPanel.SetActive(false);

        gameObject.SetActive(true);
        transform.SetAsLastSibling();

        ApplyChapterBackground();
        PopulateLevels();
    }

    void ApplyChapterBackground()
    {
        int chapter = PlayerPrefs.GetInt("ACTIVE_CHAPTER", 1);

        if (chapter1Background != null)
            chapter1Background.SetActive(chapter == 1);

        if (chapter2Background != null)
            chapter2Background.SetActive(chapter == 2);
    }

    public void ClosePopup()
    {
        gameObject.SetActive(false);

        if (chapterSelectPanel != null)
            chapterSelectPanel.SetActive(true);
    }

    void PopulateLevels()
    {
        foreach (Transform child in levelGrid)
            Destroy(child.gameObject);

        int chapter = PlayerPrefs.GetInt("ACTIVE_CHAPTER", 1);

        string unlockKey = chapter == 1
            ? "UNLOCKED_LEVEL"
            : $"CH{chapter}_UNLOCKED_LEVEL";

        int unlocked = PlayerPrefs.GetInt(unlockKey, 0);

        for (int i = 0; i < TOTAL_LEVELS; i++)
        {
            GameObject btnObj = Instantiate(levelButtonPrefab, levelGrid);
            LevelButton lb = btnObj.GetComponent<LevelButton>();

            // ✅ FIX IS HERE
            bool isUnlocked = i <= unlocked + 1;

            lb.Setup(i + 1, isUnlocked);
        }
    }
}
