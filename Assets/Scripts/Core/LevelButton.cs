
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

//         lockIcon.SetActive(!unlocked);
//         levelNumberText.gameObject.SetActive(unlocked);
//         GetComponent<Button>().interactable = unlocked;
//     }

//     public void OnClick()
//     {
//         PlayerPrefs.SetInt("CURRENT_LEVEL", levelIndex);
//         PlayerPrefs.Save();

//         int chapter = PlayerPrefs.GetInt("ACTIVE_CHAPTER", 1);

//         SceneManager.LoadScene(chapter == 1 ? "SampleScene" : "Chapter_2");
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

        int chapter = PlayerPrefs.GetInt("ACTIVE_CHAPTER", 1);
        PlayerPrefs.SetInt("ACTIVE_CHAPTER", chapter);

        PlayerPrefs.Save();

        if (chapter == 1)
            SceneManager.LoadScene("SampleScene");
        else
            SceneManager.LoadScene("Chapter_2");
    }
}
