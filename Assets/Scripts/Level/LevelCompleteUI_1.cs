
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

    // =========================
    // SHOW LEVEL COMPLETE
    // =========================
    public void Show()
    {
        panel.SetActive(true);

        // ❌ Banner must NEVER appear here
        if (AdManager.Instance != null)
            AdManager.Instance.HideBanner();

        // ⛔ Pause gameplay
        Time.timeScale = 0f;
    }

    // =========================
    // HIDE LEVEL COMPLETE
    // =========================
    public void Hide()
    {
        panel.SetActive(false);

        // ✅ Resume gameplay first
        Time.timeScale = 1f;

        // ✅ Count level completion AFTER resume
        if (AdManager.Instance != null)
            AdManager.Instance.OnLevelComplete();
    }

    // =========================
    // NEXT LEVEL
    // =========================
    public void OnNext()
    {
        Hide();

        int chapter = 2; // 🔥 Chapter 2 ONLY
        int currentLevel = PlayerPrefs.GetInt("CURRENT_LEVEL", 0);

        // ⭐ Save stars
        PlayerPrefs.SetInt($"CH{chapter}_LEVEL_STARS_{currentLevel}", 3);

        // 🔓 Unlock next level
        int unlocked = PlayerPrefs.GetInt($"CH{chapter}_UNLOCKED_LEVEL", 0);
        if (currentLevel + 1 > unlocked)
            PlayerPrefs.SetInt($"CH{chapter}_UNLOCKED_LEVEL", currentLevel + 1);

        PlayerPrefs.Save();

        // 🛑 Last level → Chapter Select
        if (LevelManager_1.Instance.IsLastLevel())
        {
            SceneManager.LoadScene("ChapterSelectScene");
            return;
        }

        // ▶ Load next level
        PlayerPrefs.SetInt("CURRENT_LEVEL", currentLevel + 1);
        LevelManager_1.Instance.NextLevel();
    }

    // =========================
    // RESTART LEVEL
    // =========================
    public void OnRestart()
    {
        Hide();
        LevelManager_1.Instance.RestartLevel();
    }

    // =========================
    // BACK TO HOME
    // =========================
    public void OnBackToHome()
    {
        Time.timeScale = 1f;

        if (LevelManager_1.Instance != null)
            Destroy(LevelManager_1.Instance.gameObject);

        SceneManager.LoadScene("ChapterSelectScene");
    }
}
