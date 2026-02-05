
// using UnityEngine;
// using UnityEngine.SceneManagement;

// public class LevelCompleteButtons : MonoBehaviour
// {
//     // ▶ NEXT LEVEL
//     // ▶ NEXT LEVEL
// public void NextLevel()
// {
//     int currentLevel = PlayerPrefs.GetInt("CURRENT_LEVEL", 0);
//     int activeChapter = PlayerPrefs.GetInt("ACTIVE_CHAPTER", 1);

//     // 🔥 ADD THIS (CHAPTER 1 UNLOCK SAVE)
//     string unlockKey = $"CH{activeChapter}_UNLOCKED_LEVEL";
//     int unlocked = PlayerPrefs.GetInt(unlockKey, 0);

//     if (currentLevel + 1 > unlocked)
//         PlayerPrefs.SetInt(unlockKey, currentLevel + 1);

//     // Chapter 1 ends at level 20 (index 19)
//     if (activeChapter == 1 && currentLevel >= 19)
//     {
//         OpenChapterPopup();
//         return;
//     }

//     PlayerPrefs.SetInt("CURRENT_LEVEL", currentLevel + 1);
//     PlayerPrefs.Save();

//     SceneManager.LoadScene("SampleScene");
// }


//     // 🔙 BACK BUTTON
//     public void OnBack()
//     {
//         OpenChapterPopup();
//     }

//     // =========================
//     // 🔥 SCENE-SAFE BACK LOGIC (FIXED)
//     // =========================
//     void OpenChapterPopup()
//     {
//         Time.timeScale = 1f;

//         // ✅ VERY IMPORTANT
//         // Preserve current chapter so popup knows what to show
//         int activeChapter = PlayerPrefs.GetInt("ACTIVE_CHAPTER", 1);
//         PlayerPrefs.SetInt("ACTIVE_CHAPTER", activeChapter);

//         // ✅ Tell ChapterSelectScene to open popup
//         PlayerPrefs.SetInt("OPEN_CHAPTER_POPUP", 1);

//         SceneManager.LoadScene("ChapterSelectScene");
//     }
// }
using UnityEngine;
using UnityEngine.SceneManagement;
 
public class LevelCompleteButtons : MonoBehaviour
{
    // ▶ NEXT LEVEL
    public void NextLevel()
    {
        int currentLevel = PlayerPrefs.GetInt("CURRENT_LEVEL", 0);
        int activeChapter = PlayerPrefs.GetInt("ACTIVE_CHAPTER", 1);
 
        // Chapter 1 ends at level 20 (index 19)
        if (activeChapter == 1 && currentLevel >= 19)
        {
            OpenChapterPopup();
            return;
        }
 
        PlayerPrefs.SetInt("CURRENT_LEVEL", currentLevel + 1);
        PlayerPrefs.Save();
 
        SceneManager.LoadScene("SampleScene");
    }
 
    // 🔙 BACK BUTTON
    public void OnBack()
    {
        OpenChapterPopup();
    }
 
    // =========================
    // 🔥 SCENE-SAFE BACK LOGIC (FIXED)
    // =========================
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
 