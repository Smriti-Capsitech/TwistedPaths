
// using UnityEngine;

// public class ChapterSelectController : MonoBehaviour
// {
//     public ChapterLevelPopup chapterLevelPopup;

//     public void LoadChapter1()
//     {
//         Debug.Log("Chapter 1 button clicked");

//         PlayerPrefs.SetInt("ACTIVE_CHAPTER", 1);
//         PlayerPrefs.SetInt("CURRENT_LEVEL", 0);

//         GameManager_1.Instance.SetChapter(ChapterType.Chapter1_LineConnect);

//         chapterLevelPopup.OpenPopup();
//     }

//     public void LoadChapter2()
//     {
//         Debug.Log("Chapter 2 button clicked");

//         PlayerPrefs.SetInt("ACTIVE_CHAPTER", 2);
//         PlayerPrefs.SetInt("CURRENT_LEVEL", 0);

//         // ❌ no scene load
//         // ❌ no new popup

//         chapterLevelPopup.OpenPopup();
//     }
// }
using UnityEngine;

public class ChapterSelectController : MonoBehaviour
{
    public ChapterLevelPopup chapterLevelPopup;

    void Start()
    {
        if (PlayerPrefs.GetInt("OPEN_CHAPTER_POPUP", 0) == 1)
        {
            PlayerPrefs.DeleteKey("OPEN_CHAPTER_POPUP");
            Invoke(nameof(OpenPopupSafe), 0.05f);
        }
    }

    void OpenPopupSafe()
    {
        if (chapterLevelPopup != null)
            chapterLevelPopup.OpenPopup();
    }

    public void LoadChapter1()
    {
        PlayerPrefs.SetInt("ACTIVE_CHAPTER", 1);
        PlayerPrefs.Save();

        chapterLevelPopup.OpenPopup();
    }

    public void LoadChapter2()
    {
        PlayerPrefs.SetInt("ACTIVE_CHAPTER", 2);
        PlayerPrefs.Save();

        chapterLevelPopup.OpenPopup();
    }
}
