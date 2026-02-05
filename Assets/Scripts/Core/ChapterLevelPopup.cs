
// using UnityEngine;
// using TMPro;

// public class ChapterLevelPopup : MonoBehaviour
// {
//     public GameObject chapterSelectPanel;
//     public Transform levelGrid;
//     public GameObject levelButtonPrefab;

//     public GameObject chapter1Background;
//     public GameObject chapter2Background;

//     const int TOTAL_LEVELS = 30;

//     public void OpenPopup()
//     {
//         // Hide chapter select panel if exists
//         if (chapterSelectPanel != null)
//             chapterSelectPanel.SetActive(false);

//         gameObject.SetActive(true);
//         transform.SetAsLastSibling();

//         int chapter = PlayerPrefs.GetInt("ACTIVE_CHAPTER", 1);

//         if (chapter1Background)
//             chapter1Background.SetActive(chapter == 1);

//         if (chapter2Background)
//             chapter2Background.SetActive(chapter == 2);

//         PopulateLevels(chapter);
//     }

//     void PopulateLevels(int chapter)
//     {
//         // 🔥 clear old buttons
//         for (int i = levelGrid.childCount - 1; i >= 0; i--)
//             Destroy(levelGrid.GetChild(i).gameObject);

//         string unlockKey = $"CH{chapter}_UNLOCKED_LEVEL";
//         int unlocked = PlayerPrefs.GetInt(unlockKey, 0);

//         // Debug safety (you can remove later)
//         Debug.Log($"[Chapter {chapter}] Unlocked up to level index: {unlocked}");

//         for (int i = 0; i < TOTAL_LEVELS; i++)
//         {
//             GameObject b = Instantiate(levelButtonPrefab, levelGrid);

//             bool isUnlocked = i <= unlocked; // 🔓 key logic
//             b.GetComponent<LevelButton>().Setup(i + 1, isUnlocked);
//         }
//     }
// }
using UnityEngine;
using TMPro;

public class ChapterLevelPopup : MonoBehaviour
{
    public GameObject chapterSelectPanel;
    public Transform levelGrid;
    public GameObject levelButtonPrefab;

    public GameObject chapter1Background;
    public GameObject chapter2Background;

    const int TOTAL_LEVELS = 30;

    public void OpenPopup()
    {
        if (chapterSelectPanel != null)
            chapterSelectPanel.SetActive(false);

        gameObject.SetActive(true);
        transform.SetAsLastSibling();

        int chapter = PlayerPrefs.GetInt("ACTIVE_CHAPTER", 1);

        if (chapter1Background) chapter1Background.SetActive(chapter == 1);
        if (chapter2Background) chapter2Background.SetActive(chapter == 2);

        PopulateLevels(chapter);
    }

    void PopulateLevels(int chapter)
    {
        for (int i = levelGrid.childCount - 1; i >= 0; i--)
            Destroy(levelGrid.GetChild(i).gameObject);

        string unlockKey = $"CH{chapter}_UNLOCKED_LEVEL";
        int unlocked = PlayerPrefs.GetInt(unlockKey, 0);

        Debug.Log($"[Chapter {chapter}] Unlocked up to level index: {unlocked}");

        for (int i = 0; i < TOTAL_LEVELS; i++)
        {
            GameObject b = Instantiate(levelButtonPrefab, levelGrid);
            b.GetComponent<LevelButton>().Setup(i + 1, i <= unlocked);
        }
    }
}
