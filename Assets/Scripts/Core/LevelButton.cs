
// using UnityEngine;
// using UnityEngine.UI;
// using TMPro;
// using UnityEngine.SceneManagement;

// public class LevelButton : MonoBehaviour
// {
//     public TextMeshProUGUI levelNumberText;
//     public GameObject lockIcon;

//     int levelIndex;

//     // 🔥 TRUST THE VALUE PASSED FROM POPUP
//     public void Setup(int levelNumber, bool unlocked)
//     {
//         levelIndex = levelNumber - 1;
//         levelNumberText.text = levelNumber.ToString();

//         if (!unlocked)
//         {
//             lockIcon.SetActive(true);
//             levelNumberText.gameObject.SetActive(false);
//             GetComponent<Button>().interactable = false;
//             return;
//         }

//         lockIcon.SetActive(false);
//         levelNumberText.gameObject.SetActive(true);
//         GetComponent<Button>().interactable = true;
//     }

//     public void OnClick()
//     {
//         PlayerPrefs.SetInt("CURRENT_LEVEL", levelIndex);
//         PlayerPrefs.Save();

//         int chapter = PlayerPrefs.GetInt("ACTIVE_CHAPTER", 1);

//         if (chapter == 1)
//             SceneManager.LoadScene("SampleScene");
//         else
//             SceneManager.LoadScene("Chapter_2");
//     }
// }
// using UnityEngine;
// using UnityEngine.UI;
// using TMPro;
// using UnityEngine.SceneManagement;

// public class LevelButton : MonoBehaviour
// {
//     public TextMeshProUGUI levelNumberText;
//     public GameObject lockIcon;

//     int levelIndex;

//     // 🔥 TRUST POPUP COMPLETELY
//     public void Setup(int levelNumber, bool unlocked)
//     {
//         levelIndex = levelNumber - 1;
//         levelNumberText.text = levelNumber.ToString();

//         lockIcon.SetActive(!unlocked);
//         levelNumberText.gameObject.SetActive(unlocked);
//         GetComponent<Button>().interactable = unlocked;
//     }

//     public void OnClick()
//     {
//         PlayerPrefs.SetInt("CURRENT_LEVEL", levelIndex);
//         PlayerPrefs.Save();

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

        lockIcon.SetActive(!unlocked);
        levelNumberText.gameObject.SetActive(unlocked);
        GetComponent<Button>().interactable = unlocked;
    }

    public void OnClick()
    {
        PlayerPrefs.SetInt("CURRENT_LEVEL", levelIndex);
        PlayerPrefs.Save();

        int chapter = PlayerPrefs.GetInt("ACTIVE_CHAPTER", 1);

        if (chapter == 1)
            SceneManager.LoadScene("SampleScene");
        else
            SceneManager.LoadScene("Chapter_2");
    }
}
