using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUI : MonoBehaviour
{
    public static PauseUI Instance;

    public GameObject pausePanel;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        pausePanel.SetActive(false);
    }

    // Called by Pause Button
    public void PauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    // Called by Resume Button
    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    // Called by Restart Button
    public void RestartLevel()
    {
        Time.timeScale = 1f;
        LevelManager_1.Instance.RestartLevel();
        pausePanel.SetActive(false);
    }

    // Called by Home Button
    public void BackToHome()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("ChapterSelectScene");
    }
}
