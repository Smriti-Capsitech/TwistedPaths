
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverButtons : MonoBehaviour
{
    void OnEnable()
    {
        // 🔥 GAME OVER INTERSTITIAL (EVERY 3 TIMES)
        if (AdManager.Instance != null)
            AdManager.Instance.OnGameOver();
    }

    // 🔁 RETRY BUTTON
    public void Retry()
    {
        SceneManager.LoadScene("SampleScene");
    }

    // ▶ NEXT LEVEL BUTTON
    public void NextLevel()
    {
        int currentLevel = PlayerPrefs.GetInt("CURRENT_LEVEL", 0);

        // ✅ AFTER LEVEL 3 → GAME OVER
        if (currentLevel >= 20)   // Level 3 completed
        {
            SceneManager.LoadScene("GameOverScene");
            return;
        }

        // ▶ Otherwise go to next level
        PlayerPrefs.SetInt("CURRENT_LEVEL", currentLevel + 1);
        SceneManager.LoadScene("SampleScene");
    }
    public void OnBack()
    {
        OpenChapterPopup();
    }


    // 🏠 MAIN MENU BUTTON
     void OpenChapterPopup()
    {
        Time.timeScale = 1f;

        // ✅ VERY IMPORTANT
        // Preserve current chapter so popup knows what to show
        int activeChapter = PlayerPrefs.GetInt("ACTIVE_CHAPTER", 1);
        PlayerPrefs.SetInt("ACTIVE_CHAPTER", activeChapter);

        // ✅ Tell ChapterSelectScene to open popup
        PlayerPrefs.SetInt("OPEN_CHAPTER_POPUP", 1);

        SceneManager.LoadScene("ChapterSelectScene");
    }
}
