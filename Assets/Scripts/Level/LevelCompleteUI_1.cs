// using UnityEngine;
// using UnityEngine.SceneManagement;

// public class LevelCompleteUI_1 : MonoBehaviour
// {
//     public static LevelCompleteUI_1 Instance;
//     public GameObject panel;

//     void Awake()
//     {
//         if (Instance != null && Instance != this)
//         {
//             Destroy(gameObject);
//             return;
//         }

//         Instance = this;
//         panel.SetActive(false);
//     }

//     // =========================
//     // SHOW LEVEL COMPLETE
//     // =========================
//     public void Show()
//     {
//         panel.SetActive(true);

//         if (AdManager.Instance != null)
//             AdManager.Instance.HideBanner();

//         Time.timeScale = 0f;
//     }

//     // =========================
//     // HIDE LEVEL COMPLETE
//     // =========================
//     public void Hide()
//     {
//         panel.SetActive(false);
//         Time.timeScale = 1f;

//         if (AdManager.Instance != null)
//             AdManager.Instance.OnLevelComplete();
//     }

//     // =========================
//     // NEXT LEVEL  ✅ FIXED
//     // =========================
//     public void OnNext()
//     {
//         Hide();

//         int chapter = PlayerPrefs.GetInt("ACTIVE_CHAPTER", 1); // ✅ FIX
//         int currentLevel = PlayerPrefs.GetInt("CURRENT_LEVEL", 0);

//         // ⭐ Save stars
//         PlayerPrefs.SetInt($"CH{chapter}_LEVEL_STARS_{currentLevel}", 3);

//         // 🔓 Unlock next level
//         string unlockKey = $"CH{chapter}_UNLOCKED_LEVEL";
//         int unlocked = PlayerPrefs.GetInt(unlockKey, 0);

//         if (currentLevel + 1 > unlocked)
//             PlayerPrefs.SetInt(unlockKey, currentLevel + 1);

//         PlayerPrefs.Save();

//         // 🛑 Last level → Chapter Select
//         if (LevelManager_1.Instance.IsLastLevel())
//         {
//             PlayerPrefs.SetInt("OPEN_CHAPTER_POPUP", 1);
//             SceneManager.LoadScene("ChapterSelectScene");
//             return;
//         }

//         // ▶ Load next level
//         PlayerPrefs.SetInt("CURRENT_LEVEL", currentLevel + 1);
//         LevelManager_1.Instance.NextLevel();
//     }

//     // =========================
//     // RESTART LEVEL
//     // =========================
//     public void OnRestart()
//     {
//         Hide();
//         LevelManager_1.Instance.RestartLevel();
//     }

//     // =========================
//     // BACK TO HOME
//     // =========================
//     public void OnBackToHome()
//     {
//         Time.timeScale = 1f;

//         if (LevelManager_1.Instance != null)
//             Destroy(LevelManager_1.Instance.gameObject);

//         SceneManager.LoadScene("ChapterSelectScene");
//     }
// }
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompleteUI_1 : MonoBehaviour
{
    public static LevelCompleteUI_1 Instance;
    public GameObject panel;

    void Awake()
    {
        if (Instance != null && Instance != this)
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

        if (AdManager.Instance != null)
            AdManager.Instance.HideBanner();

        Time.timeScale = 0f;
    }

    // =========================
    // HIDE LEVEL COMPLETE
    // =========================
    public void Hide()
    {
        panel.SetActive(false);
        Time.timeScale = 1f;

        if (AdManager.Instance != null)
            AdManager.Instance.OnLevelComplete();
    }

    // =========================
    // NEXT LEVEL
    // =========================
    public void OnNext()
{
    Hide();

    int chapter = PlayerPrefs.GetInt("ACTIVE_CHAPTER", 1);
    int currentLevel = PlayerPrefs.GetInt("CURRENT_LEVEL", 0);

    string unlockKey = $"CH{chapter}_UNLOCKED_LEVEL";
    int unlocked = PlayerPrefs.GetInt(unlockKey, 0);

    // 🔥 FINAL FIX
    if (currentLevel > unlocked)
        PlayerPrefs.SetInt(unlockKey, currentLevel);

    PlayerPrefs.Save();

    if (LevelManager_1.Instance.IsLastLevel())
    {
        PlayerPrefs.SetInt("OPEN_CHAPTER_POPUP", 1);
        SceneManager.LoadScene("ChapterSelectScene");
        return;
    }

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
