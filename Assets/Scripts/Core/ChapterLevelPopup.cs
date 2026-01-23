
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
