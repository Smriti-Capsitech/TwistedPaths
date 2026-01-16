// using UnityEngine;
// using UnityEngine.SceneManagement;

// public class PlayButton : MonoBehaviour
// {
//     public void OnPlayButtonClicked()
//     {
//         SceneManager.LoadScene("ChapterSelectScene");
//     }
//     public void ExitGame()
//     {
//         Debug.Log("Exit button clicked");
//         Application.Quit();
//     }
// }
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    // ▶ PLAY → CHAPTER SELECT
    public void OnPlayButtonClicked()
    {
        SceneManager.LoadScene("ChapterSelectScene");
    }

    // 🔙 BACK → START SCENE
    public void OnBackButtonClicked()
    {
        Debug.Log("Back button clicked");
        SceneManager.LoadScene("StartScene"); // 🔁 change name if needed
    }
        public void ExitGame()
    {
        Debug.Log("Exit button clicked");
        Application.Quit();
    }

}
