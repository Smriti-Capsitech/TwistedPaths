// using UnityEngine;
// using UnityEngine.SceneManagement;

// public class GameOverButtons : MonoBehaviour
// {
//     // 🔁 RETRY BUTTON
//     public void Retry()
//     {
//         // Reload same gameplay scene (current level is already saved)
//         SceneManager.LoadScene("SampleScene");
//     }

//     // 🏠 MAIN MENU BUTTON
//     public void MainMenu()
//     {
//         // Reset level progress
//         PlayerPrefs.DeleteKey("CURRENT_LEVEL");
//         SceneManager.LoadScene("ChapterSelectScene");
//     }
// }
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverButtons : MonoBehaviour
{
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
        if (currentLevel >= 2)   // Level 3 completed
        {
            SceneManager.LoadScene("GameOverScene");
            return;
        }

        // ▶ Otherwise go to next level
        PlayerPrefs.SetInt("CURRENT_LEVEL", currentLevel + 1);
        SceneManager.LoadScene("SampleScene");
    }

    // 🏠 MAIN MENU BUTTON
    public void MainMenu()
    {
        PlayerPrefs.DeleteKey("CURRENT_LEVEL");
        SceneManager.LoadScene("ChapterSelectScene");
    }
}
