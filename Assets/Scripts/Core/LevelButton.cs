
// using UnityEngine;
// using UnityEngine.UI;
// using TMPro;
// using UnityEngine.SceneManagement;

// public class LevelButton : MonoBehaviour
// {
//     public TextMeshProUGUI levelNumberText;
//     public GameObject lockIcon;

//     int levelIndex;

//     public void Setup(int levelNumber, bool unlocked)
//     {
//         levelIndex = levelNumber - 1;
//         levelNumberText.text = levelNumber.ToString();

//         int chapter = PlayerPrefs.GetInt("ACTIVE_CHAPTER", 1);

//         string unlockKey = chapter == 1
//             ? "UNLOCKED_LEVEL"
//             : $"CH{chapter}_UNLOCKED_LEVEL";

//         int unlockedLevel = PlayerPrefs.GetInt(unlockKey, 0);
//         bool isUnlocked = levelIndex <= unlockedLevel;

//         if (!isUnlocked)
//         {
//             lockIcon.SetActive(true);
//             GetComponent<Button>().interactable = false;
//             return;
//         }

//         lockIcon.SetActive(false);
//         GetComponent<Button>().interactable = true;
//     }

//     public void OnClick()
//     {
//         PlayerPrefs.SetInt("CURRENT_LEVEL", levelIndex);

//         int chapter = PlayerPrefs.GetInt("ACTIVE_CHAPTER", 1);

//         if (chapter == 1)
//             SceneManager.LoadScene("SampleScene");
//         else
//             SceneManager.LoadScene("Chapter_2");
//     }
// }
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    public TextMeshProUGUI levelNumberText;
    public GameObject lockIcon;

    int levelIndex;

    public void Setup(int levelNumber, bool unlocked)
    {
        levelIndex = levelNumber - 1;
        levelNumberText.text = levelNumber.ToString();

        int chapter = PlayerPrefs.GetInt("ACTIVE_CHAPTER", 1);

        string unlockKey = chapter == 1
            ? "UNLOCKED_LEVEL"
            : $"CH{chapter}_UNLOCKED_LEVEL";

        int unlockedLevel = PlayerPrefs.GetInt(unlockKey, 0);
        bool isUnlocked = levelIndex <= unlockedLevel;

        if (!isUnlocked)
        {
            lockIcon.SetActive(true);
            levelNumberText.gameObject.SetActive(false); // ✅ HIDE NUMBER
            GetComponent<Button>().interactable = false;
            return;
        }

        lockIcon.SetActive(false);
        levelNumberText.gameObject.SetActive(true); // ✅ SHOW NUMBER
        GetComponent<Button>().interactable = true;
    }

    public void OnClick()
    {
        PlayerPrefs.SetInt("CURRENT_LEVEL", levelIndex);

        int chapter = PlayerPrefs.GetInt("ACTIVE_CHAPTER", 1);

        if (chapter == 1)
            SceneManager.LoadScene("SampleScene");
        else
            SceneManager.LoadScene("Chapter_2");
    }
}
