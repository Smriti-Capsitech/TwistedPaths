// using UnityEngine;
// using UnityEngine.SceneManagement;
 
// public class LevelCompleteUI_1 : MonoBehaviour
// {
//     public static LevelCompleteUI_1 Instance;
//     public GameObject panel;
 
//     void Awake()
//     {
//         if (Instance != null && Instance != this)
//             Destroy(Instance.gameObject);
 
//         Instance = this;
 
//         if (panel == null)
//             panel = gameObject;
 
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
 
//         int currentLevel =
//             PlayerPrefs.GetInt("CURRENT_LEVEL", 0);
 
//         PlayerPrefs.SetInt("CURRENT_LEVEL", currentLevel + 1);
 
//         if (LevelManager_1.Instance.IsLastLevel())
//         {
//             SceneManager.LoadScene("ChapterSelectScene");
//             return;
//         }
 
//         LevelManager_1.Instance.NextLevel();
//     }
 
//     public void OnRestart()
//     {
//         Hide();
//         LevelManager_1.Instance.RestartLevel();
//     }
 
//     public void OnBackToHome()
//     {
//         Time.timeScale = 1f;
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

        if (panel == null)
            panel = gameObject;

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
        SceneManager.LoadScene("ChapterSelectScene");
    }
}
