
// using UnityEngine;
// using UnityEngine.SceneManagement;


// public class ChapterSelectController : MonoBehaviour
// {
//     [Header("Popup Reference")]
//     public ChapterLevelPopup chapterLevelPopup;

//     public void LoadChapter1()
//     {
//         Debug.Log("Chapter 1 button clicked");

//         // 🔥 SET CHAPTER DATA (keep this)
//         GameManager_1.Instance.SetChapter(ChapterType.Chapter1_LineConnect);
//         PlayerPrefs.SetInt("CURRENT_LEVEL", 0);

//         // ❌ DO NOT LOAD SCENE HERE
//         // SceneManager.LoadScene("SampleScene");

//         // ✅ OPEN CHAPTER LEVEL POPUP
//         if (chapterLevelPopup != null)
//         {
//             chapterLevelPopup.OpenPopup();
//         }
//         else
//         {
//             Debug.LogError("❌ ChapterLevelPopup reference missing");
//         }
//     }

//     public void LoadChapter2()
//     {
//         Debug.Log("Chapter 2 button clicked");

//         PlayerPrefs.SetInt("CURRENT_LEVEL", 0);

//         // 🚫 Leave Chapter 2 as scene load (as you wanted)
//         SceneManager.LoadScene("Chapter_2");
//     }
// }
using UnityEngine;

public class ChapterSelectController : MonoBehaviour
{
    public ChapterLevelPopup chapterLevelPopup;

    public void LoadChapter1()
    {
        Debug.Log("Chapter 1 button clicked");

        PlayerPrefs.SetInt("ACTIVE_CHAPTER", 1);
        PlayerPrefs.SetInt("CURRENT_LEVEL", 0);

        GameManager_1.Instance.SetChapter(ChapterType.Chapter1_LineConnect);

        chapterLevelPopup.OpenPopup();
    }

    public void LoadChapter2()
    {
        Debug.Log("Chapter 2 button clicked");

        PlayerPrefs.SetInt("ACTIVE_CHAPTER", 2);
        PlayerPrefs.SetInt("CURRENT_LEVEL", 0);

        // ❌ no scene load
        // ❌ no new popup

        chapterLevelPopup.OpenPopup();
    }
}
