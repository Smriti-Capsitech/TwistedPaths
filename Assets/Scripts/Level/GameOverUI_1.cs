




// using UnityEngine;
// using UnityEngine.SceneManagement;

// public class GameOverUI_1 : MonoBehaviour
// {
//     public static GameOverUI_1 Instance;

//     [SerializeField] private GameObject panel;

//     void Awake()
//     {
//         if (Instance != null)
//         {
//             Destroy(gameObject);
//             return;
//         }

//         Instance = this;

//         // KEEP this object across scene reloads
//         DontDestroyOnLoad(gameObject);

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

//     public void OnRestart()
//     {
//         Hide();
//         LevelManager_1.Instance.RestartLevel();
//     }
//     public void MainMenu()
//     {
//         // Reset progress when going to menu
//         PlayerPrefs.DeleteKey("CURRENT_LEVEL");
//         SceneManager.LoadScene("ChapterSelectScene");
//     }
// }
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI_1 : MonoBehaviour
{
    public static GameOverUI_1 Instance;

    [SerializeField] private GameObject panel;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        panel.SetActive(false);

        // 🔥 listen for scene changes
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // 🔥 AUTO-HIDE ON NEW SCENE
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        panel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Show()
    {
        panel.SetActive(true);
        Time.timeScale = 0f;
        if (AdManager.Instance != null)
        AdManager.Instance.OnGameOver();

    }

    public void Hide()
    {
        panel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OnRestart()
    {
        Hide();
        LevelManager_1.Instance.RestartLevel();
    }

    public void MainMenu()
    {
        // 🔥 FULL RESET
        Time.timeScale = 1f;

        PlayerPrefs.DeleteKey("CURRENT_LEVEL");
        PlayerPrefs.DeleteKey("CURRENT_CHAPTER");
        PlayerPrefs.DeleteKey("CHAPTER_INDEX");
        PlayerPrefs.DeleteKey("CHAPTER_TYPE");
        PlayerPrefs.Save();

        // 🔥 DESTROY THIS UI (CRITICAL)
        Destroy(gameObject);

        SceneManager.LoadScene("ChapterSelectScene");
    }
}
