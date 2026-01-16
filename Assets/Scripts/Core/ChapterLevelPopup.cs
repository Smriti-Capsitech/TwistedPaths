// using UnityEngine;
// using TMPro;

// public class ChapterLevelPopup : MonoBehaviour
// {
//     public TextMeshProUGUI progressText;
//     public Transform levelGrid;
//     public GameObject levelButtonPrefab;

//     private const int TOTAL_LEVELS = 30;

//     // 🔥 OPEN POPUP
//     public void OpenPopup()
//     {
//         Debug.Log("Popup opened");

//         PopulateLevels();
       
//     }

//     // ❌ CLOSE BUTTON
//     public void ClosePopup()
//     {
//         gameObject.SetActive(false);

//         // OPTIONAL: go back to chapter select
//         FindAnyObjectByType<ChapterSelectController>()
//             ?.transform.GetChild(0).gameObject.SetActive(true);
//     }

//     void PopulateLevels()
//     {
//         foreach (Transform child in levelGrid)
//             Destroy(child.gameObject);

//         int unlockedLevel = PlayerPrefs.GetInt("CURRENT_LEVEL", 0);

//         for (int i = 0; i < TOTAL_LEVELS; i++)
//         {
//             GameObject btn = Instantiate(levelButtonPrefab, levelGrid);
//             LevelButton lb = btn.GetComponent<LevelButton>();

//             if (lb != null)
//                 lb.Setup(i + 1, i <= unlockedLevel);
//         }
//     }
// }

// using UnityEngine;
// using TMPro;

// public class ChapterLevelPopup : MonoBehaviour
// {
//     [Header("UI References")]
//     public GameObject chapterSelectPanel;   // back panel
    
//     public Transform levelGrid;
//     public GameObject levelButtonPrefab;

//     private const int TOTAL_LEVELS = 30;

//     // 🔥 OPEN POPUP
//     public void OpenPopup()
//     {
//         Debug.Log("Popup opened");

//         chapterSelectPanel.SetActive(false); // hide chapter select
//         gameObject.SetActive(true);
//         transform.SetAsLastSibling();        // popup on top

//         PopulateLevels();
        
//     }

//     // ❌ CLOSE BUTTON
//     public void ClosePopup()
//     {
//         Debug.Log("Popup closed");

//         gameObject.SetActive(false);         // hide popup
//         chapterSelectPanel.SetActive(true);  // show chapter select
//     }

//     // =========================
//     // LEVEL BUTTONS
//     // =========================
// //    void PopulateLevels()
// // {
// //     foreach (Transform child in levelGrid)
// //         Destroy(child.gameObject);

// //     int maxUnlocked = PlayerPrefs.GetInt("MAX_UNLOCKED_LEVEL", 0);

// //     for (int i = 0; i < TOTAL_LEVELS; i++)
// //     {
// //         GameObject btnObj = Instantiate(levelButtonPrefab, levelGrid);
// //         LevelButton lb = btnObj.GetComponent<LevelButton>();

// //         bool unlocked = i <= maxUnlocked;
// //         lb.Setup(i + 1, unlocked);
// //     }
// // }
// void PopulateLevels()
// {
//     foreach (Transform child in levelGrid)
//         Destroy(child.gameObject);

//     int unlocked = PlayerPrefs.GetInt("UNLOCKED_LEVEL", 0);

//     for (int i = 0; i < TOTAL_LEVELS; i++)
//     {
//         GameObject btnObj = Instantiate(levelButtonPrefab, levelGrid);
//         LevelButton lb = btnObj.GetComponent<LevelButton>();

//         bool isUnlocked = i <= unlocked;
//         lb.Setup(i + 1, isUnlocked);
//     }
// }
// }

// using UnityEngine;
// using TMPro;

// public class ChapterLevelPopup : MonoBehaviour
// {
//     [Header("UI References")]
//     public GameObject chapterSelectPanel;
    
//     public Transform levelGrid;
//     public GameObject levelButtonPrefab;

//     private const int TOTAL_LEVELS = 30;

//     // 🔥 OPEN POPUP
//     public void OpenPopup()
//     {
//         Debug.Log("✅ ChapterLevelPopup.OpenPopup CALLED");

//         // 🔥 FORCE ACTIVATE
//         gameObject.SetActive(true);

//         if (chapterSelectPanel != null)
//             chapterSelectPanel.SetActive(false);

//         transform.SetAsLastSibling();

//         PopulateLevels();
//     }

//     public void ClosePopup()
//     {
//         Debug.Log("❌ Popup closed");

//         gameObject.SetActive(false);

//         if (chapterSelectPanel != null)
//             chapterSelectPanel.SetActive(true);
//     }

//     void PopulateLevels()
//     {
//         foreach (Transform child in levelGrid)
//             Destroy(child.gameObject);

//         int unlocked = PlayerPrefs.GetInt("UNLOCKED_LEVEL", 0);

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

    public void OpenPopup()
    {
        Debug.Log("Popup opened");

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
        chapterSelectPanel.SetActive(true);
    }

    void PopulateLevels()
    {
        foreach (Transform child in levelGrid)
            Destroy(child.gameObject);

        int unlocked = PlayerPrefs.GetInt("UNLOCKED_LEVEL", 0);

        for (int i = 0; i < TOTAL_LEVELS; i++)
        {
            GameObject btnObj = Instantiate(levelButtonPrefab, levelGrid);
            LevelButton lb = btnObj.GetComponent<LevelButton>();

            bool isUnlocked = i <= unlocked;
            lb.Setup(i + 1, isUnlocked);
        }
    }
}
