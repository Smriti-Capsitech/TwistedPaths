using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompleteUI_1 : MonoBehaviour
{
    public static LevelCompleteUI_1 Instance;
    public GameObject panel;

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(Instance.gameObject);

        Instance = this;

        if (panel == null)
            panel = gameObject;

        panel.SetActive(false);
    }

    public void Show()
    {
        panel.SetActive(true);

        if (AdManager.Instance != null)
            AdManager.Instance.HideBanner();

        Time.timeScale = 0f;
    }

    public void Hide()
    {
        panel.SetActive(false);

        Time.timeScale = 1f;

        if (AdManager.Instance != null)
            AdManager.Instance.OnLevelComplete();
    }

    public void OnNext()
    {
        Hide();

        int chapter = 2;
        int currentLevel = PlayerPrefs.GetInt("CURRENT_LEVEL", 0);

        PlayerPrefs.SetInt($"CH{chapter}_LEVEL_STARS_{currentLevel}", 3);

        int unlocked = PlayerPrefs.GetInt($"CH{chapter}_UNLOCKED_LEVEL", 0);
        if (currentLevel + 1 > unlocked)
            PlayerPrefs.SetInt($"CH{chapter}_UNLOCKED_LEVEL", currentLevel + 1);

        PlayerPrefs.Save();

        if (LevelManager_1.Instance.IsLastLevel())
        {
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
        SceneManager.LoadScene("ChapterSelectScene");
    }
}
