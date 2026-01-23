
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompleteButtons : MonoBehaviour
{
    public void NextLevel()
    {
        int currentLevel = PlayerPrefs.GetInt("CURRENT_LEVEL", 0);

        // ✅ Chapter 1 ends at Level 20 (index 19)
        if (currentLevel >= 19)
        {
            SceneManager.LoadScene("ChapterSelectScene");
            return;
        }

        // ✅ Otherwise go to next level
        PlayerPrefs.SetInt("CURRENT_LEVEL", currentLevel + 1);
        PlayerPrefs.Save();

        SceneManager.LoadScene("SampleScene");
    }

    // 🏠 MAIN MENU BUTTON
    public void MainMenu()
    {
        SceneManager.LoadScene("ChapterSelectScene");
    }

    public void OnBackToHome()
    {
        // ✅ Always restore time
        Time.timeScale = 1f;

        // ✅ Destroy ONLY Chapter 2 LevelManager (safe)
        if (LevelManager_1.Instance != null)
        {
            Destroy(LevelManager_1.Instance.gameObject);
        }

        SceneManager.LoadScene("ChapterSelectScene");
    }
}
