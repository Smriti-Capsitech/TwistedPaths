// using UnityEngine;
// using UnityEngine.SceneManagement;

// public class LevelCompleteButtons : MonoBehaviour
// {
//     // ▶ NEXT LEVEL BUTTON
//     public void NextLevel()
//     {
//         // If LevelManager exists (same scene, no scene switch)
//         if (LevelManager.Instance != null)
//         {
//             LevelManager.Instance.NextLevel();
//         }
//         else
//         {
//             // ✅ We are in LevelCompleteScene
//             // Increase saved level index
//             int currentLevel = PlayerPrefs.GetInt("CURRENT_LEVEL", 0);
//             PlayerPrefs.SetInt("CURRENT_LEVEL", currentLevel + 1);

//             // Load gameplay scene
//             SceneManager.LoadScene("SampleScene");
//         }
//     }

//     // 🏠 MAIN MENU BUTTON
//     public void MainMenu()
//     {
//         // Reset progress when going to menu
//         PlayerPrefs.DeleteKey("CURRENT_LEVEL");
//         SceneManager.LoadScene("ChapterSelectScene");
//     }
// }
// using UnityEngine;
// using UnityEngine.SceneManagement;

// public class LevelCompleteButtons : MonoBehaviour
// {
//     // ▶ NEXT LEVEL BUTTON
//     public void NextLevel()
//     {
//         int currentLevel = PlayerPrefs.GetInt("CURRENT_LEVEL", 0);
//         // Level index:
//         // Level 1 = 0
//         // Level 2 = 1
//         // Level 3 = 2

//         // =========================
//         // 🔥 AFTER LEVEL 3 → GAME OVER
//         // =========================
//         if (currentLevel >= 2)
//         {
//             SceneManager.LoadScene("GameOverScene");
//             return;
//         }

//         // =========================
//         // ▶ NORMAL FLOW
//         // =========================

//         // If LevelManager exists (same scene, no scene switch)
//         if (LevelManager.Instance != null)
//         {
//             LevelManager.Instance.NextLevel();
//         }
//         else
//         {
//             // ✅ We are in LevelCompleteScene
//             // Increase saved level index
//             PlayerPrefs.SetInt("CURRENT_LEVEL", currentLevel + 1);

//             // Load gameplay scene
//             SceneManager.LoadScene("SampleScene");
//         }
//     }

//     // 🏠 MAIN MENU BUTTON
//     public void MainMenu()
//     {
//         // Reset progress when going to menu
//         PlayerPrefs.DeleteKey("CURRENT_LEVEL");
//         SceneManager.LoadScene("ChapterSelectScene");
//     }
// }
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompleteButtons : MonoBehaviour
{


public void NextLevel()
{
    int currentLevel = PlayerPrefs.GetInt("CURRENT_LEVEL", 0);

    // ONLY move to next level
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

        // ✅ Destroy ONLY Chapter 2 LevelManager
        if (LevelManager_1.Instance != null)
        {
            Destroy(LevelManager_1.Instance.gameObject);
        }

        // ✅ Load chapter select
        SceneManager.LoadScene("ChapterSelectScene");
    }
}
