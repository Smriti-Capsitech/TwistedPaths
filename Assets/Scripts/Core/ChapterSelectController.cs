
// using UnityEngine;

// public class ChapterSelectController : MonoBehaviour
// {
//     public ChapterLevelPopup chapterLevelPopup;

//     void Start()
//     {
//         if (PlayerPrefs.GetInt("OPEN_CHAPTER_POPUP", 0) == 1)
//         {
//             PlayerPrefs.DeleteKey("OPEN_CHAPTER_POPUP");
//             Invoke(nameof(OpenPopupSafe), 0.1f); // ⏱ small safe delay
//         }
//     }

//     void OpenPopupSafe()
//     {
//         chapterLevelPopup.gameObject.SetActive(true);
//         chapterLevelPopup.OpenPopup(); // 🔥 force rebuild every time
//     }

//     public void LoadChapter1()
//     {
//         PlayerPrefs.SetInt("ACTIVE_CHAPTER", 1);

//         if (!PlayerPrefs.HasKey("CH1_UNLOCKED_LEVEL"))
//             PlayerPrefs.SetInt("CH1_UNLOCKED_LEVEL", 0);

//         PlayerPrefs.Save();

//         chapterLevelPopup.gameObject.SetActive(true);
//         chapterLevelPopup.OpenPopup();
//     }

//     public void LoadChapter2()
//     {
//         PlayerPrefs.SetInt("ACTIVE_CHAPTER", 2);

//         if (!PlayerPrefs.HasKey("CH2_UNLOCKED_LEVEL"))
//             PlayerPrefs.SetInt("CH2_UNLOCKED_LEVEL", 0);

//         PlayerPrefs.Save();

//         chapterLevelPopup.gameObject.SetActive(true);
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
            Invoke(nameof(OpenPopupSafe), 0.1f);
        }
    }

    void OpenPopupSafe()
    {
        chapterLevelPopup.gameObject.SetActive(true);
        chapterLevelPopup.OpenPopup();
    }

    public void LoadChapter1()
    {
        PlayerPrefs.SetInt("ACTIVE_CHAPTER", 1);

        if (!PlayerPrefs.HasKey("CH1_UNLOCKED_LEVEL"))
            PlayerPrefs.SetInt("CH1_UNLOCKED_LEVEL", 0);

        PlayerPrefs.Save();

        chapterLevelPopup.gameObject.SetActive(true);
        chapterLevelPopup.OpenPopup();
    }

    public void LoadChapter2()
    {
        PlayerPrefs.SetInt("ACTIVE_CHAPTER", 2);

        if (!PlayerPrefs.HasKey("CH2_UNLOCKED_LEVEL"))
            PlayerPrefs.SetInt("CH2_UNLOCKED_LEVEL", 0);

        PlayerPrefs.Save();

        chapterLevelPopup.gameObject.SetActive(true);
        chapterLevelPopup.OpenPopup();
    }
}
