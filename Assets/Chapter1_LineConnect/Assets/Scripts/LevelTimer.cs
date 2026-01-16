
// using UnityEngine;
// using UnityEngine.SceneManagement;
// using UnityEngine.UI;

// public class LevelTimer : MonoBehaviour
// {
//     [Header("UI")]
//     public Image fillImage;   // drag Fill image here

//     float timeLeft;
//     float totalTime;
//     bool running = false;

//     // =========================
//     // ▶ START TIMER
//     // =========================
//     public void StartTimer(float seconds)
//     {
//         totalTime = seconds;
//         timeLeft = seconds;
//         running = true;

//         if (fillImage != null)
//             fillImage.fillAmount = 1f;
//     }

//     // =========================
//     // ⏸ STOP TIMER
//     // =========================
//     public void StopTimer()
//     {
//         running = false;
//     }

//     void Update()
//     {
//         if (!running) return;

//         timeLeft -= Time.deltaTime;

//         if (fillImage != null)
//             fillImage.fillAmount = timeLeft / totalTime;

//         if (timeLeft <= 0f)
//         {
//             running = false;
//             SceneManager.LoadScene("GameOverScene");
//         }
//     }
// }
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelTimer : MonoBehaviour
{
    [Header("UI")]
    public Image fillImage;                 // Timer fill image
    public TextMeshProUGUI levelText;       // 🔥 LEVEL TEXT (LEVEL 1, LEVEL 2…)

    float timeLeft;
    float totalTime;
    bool running = false;

    // =========================
    // ▶ START TIMER
    // =========================
    public void StartTimer(float seconds)
    {
        totalTime = seconds;
        timeLeft = seconds;
        running = true;

        if (fillImage != null)
            fillImage.fillAmount = 1f;

        UpdateLevelText(); // 🔥 UPDATE LEVEL UI
    }

    // =========================
    // ⏸ STOP TIMER
    // =========================
    public void StopTimer()
    {
        running = false;
    }

    void Update()
    {
        if (!running) return;

        timeLeft -= Time.deltaTime;

        if (fillImage != null)
            fillImage.fillAmount = timeLeft / totalTime;

        if (timeLeft <= 0f)
        {
            running = false;
            SceneManager.LoadScene("GameOverScene");
        }
    }

    // =========================
    // 🔥 LEVEL UI
    // =========================
    void UpdateLevelText()
    {
        if (levelText == null) return;

        int currentLevel = PlayerPrefs.GetInt("CURRENT_LEVEL", 0);
        levelText.text = $"Level {currentLevel + 1}";
    }
}
