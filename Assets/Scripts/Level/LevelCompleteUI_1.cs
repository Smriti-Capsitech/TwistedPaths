
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

//     public void Show()
//     {
//         panel.SetActive(true);
//         Time.timeScale = 0f;
//     }

//     public void Hide()
//     {
//         panel.SetActive(false);
//         Time.timeScale = 1f;
//     }

//     public void OnNext()
//     {
//         Hide();

//         int chapter = PlayerPrefs.GetInt("ACTIVE_CHAPTER", 1);
//         int currentLevel = PlayerPrefs.GetInt("CURRENT_LEVEL", 0);

//         string unlockKey = $"CH{chapter}_UNLOCKED_LEVEL";
//         int unlocked = PlayerPrefs.GetInt(unlockKey, 0);

//         if (currentLevel + 1 > unlocked)
//             PlayerPrefs.SetInt(unlockKey, currentLevel + 1);

//         PlayerPrefs.Save();

//         if (LevelManager_1.Instance.IsLastLevel())
//         {
//             PlayerPrefs.SetInt("OPEN_CHAPTER_POPUP", 1);
//             SceneManager.LoadScene("ChapterSelectScene");
//             return;
//         }

//         PlayerPrefs.SetInt("CURRENT_LEVEL", currentLevel + 1);
//         LevelManager_1.Instance.NextLevel();
//         Debug.Log($"Unlocked level: {currentLevel + 1} for CH{chapter}");

//     }

//     public void OnRestart()
//     {
//         Hide();
//         LevelManager_1.Instance.RestartLevel();
//     }

//     public void OnBackToHome()
//     {
//         Time.timeScale = 1f;
//         PlayerPrefs.SetInt("OPEN_CHAPTER_POPUP", 1);
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

        // 🔒 FORCE CHAPTER 2
        int chapter = 2;
        PlayerPrefs.SetInt("ACTIVE_CHAPTER", chapter);

        int currentLevel = PlayerPrefs.GetInt("CURRENT_LEVEL", 0);

        string unlockKey = "CH2_UNLOCKED_LEVEL";
        int unlocked = PlayerPrefs.GetInt(unlockKey, 0);

        if (currentLevel + 1 > unlocked)
            PlayerPrefs.SetInt(unlockKey, currentLevel + 1);

        PlayerPrefs.Save();

        Debug.Log($"🔓 [Chapter 2 UI] Unlocked level: {currentLevel + 1}");

        if (LevelManager_1.Instance.IsLastLevel())
        {
            PlayerPrefs.SetInt("OPEN_CHAPTER_POPUP", 1);
            SceneManager.LoadScene("ChapterSelectScene");
            return;
        }

        PlayerPrefs.SetInt("CURRENT_LEVEL", currentLevel + 1);
        LevelManager_1.Instance.NextLevel();
    }

    public void OnRestart()
    {
        Hide();
        LevelManager_1.Instance.RestartLevel();
    }

    public void OnBackToHome()
    {
        Time.timeScale = 1f;
        PlayerPrefs.SetInt("OPEN_CHAPTER_POPUP", 1);
        SceneManager.LoadScene("ChapterSelectScene");
    }
}
