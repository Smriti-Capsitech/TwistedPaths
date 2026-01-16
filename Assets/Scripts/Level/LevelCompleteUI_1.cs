
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompleteUI_1 : MonoBehaviour
{
    public static LevelCompleteUI_1 Instance;
    public GameObject panel;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        panel.SetActive(false);
    }

    public void Show()
    {
        panel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Hide()
    {
        panel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OnNext()
{
    Hide();

    int chapter = 2; // 🔥 Chapter 2 ONLY
    int currentLevel = PlayerPrefs.GetInt("CURRENT_LEVEL", 0);

    // ⭐ SAVE STARS
    PlayerPrefs.SetInt($"CH{chapter}_LEVEL_STARS_{currentLevel}", 3);

    // 🔓 UNLOCK NEXT LEVEL
    int unlocked = PlayerPrefs.GetInt($"CH{chapter}_UNLOCKED_LEVEL", 0);
    if (currentLevel + 1 > unlocked)
        PlayerPrefs.SetInt($"CH{chapter}_UNLOCKED_LEVEL", currentLevel + 1);

    PlayerPrefs.Save();

    // 🛑 LAST LEVEL CHECK
    if (LevelManager_1.Instance.IsLastLevel())
    {
        SceneManager.LoadScene("ChapterSelectScene");
        return;
    }

    // ▶ GO TO NEXT LEVEL
    PlayerPrefs.SetInt("CURRENT_LEVEL", currentLevel + 1);
    LevelManager_1.Instance.NextLevel();
}

    

    public void OnRestart()
    {
        Hide();
        LevelManager_1.Instance.RestartLevel();
    }

    // 🔙 BACK TO CHAPTER SELECT (SAFE)
    public void OnBackToHome()
    {
        // ✅ Always restore time
        Time.timeScale = 1f;

        // ✅ Destroy ONLY Chapter 2 LevelManager
        if (LevelManager_1.Instance != null)
        {
            Destroy(LevelManager_1.Instance.gameObject);
        }

        // ✅ Load chapter select
        SceneManager.LoadScene("ChapterSelectScene");
    }
}
