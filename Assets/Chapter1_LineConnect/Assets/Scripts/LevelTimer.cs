
// using UnityEngine;
// using UnityEngine.SceneManagement;
// using UnityEngine.UI;
// using TMPro;

// public class LevelTimer : MonoBehaviour
// {
//     [Header("UI")]
//     public Image fillImage;                 // Timer fill image
//     public TextMeshProUGUI levelText;       // 🔥 LEVEL TEXT (LEVEL 1, LEVEL 2…)

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

//         UpdateLevelText(); // 🔥 UPDATE LEVEL UI
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

//     // =========================
//     // 🔥 LEVEL UI
//     // =========================
//     void UpdateLevelText()
//     {
//         if (levelText == null) return;

//         int currentLevel = PlayerPrefs.GetInt("CURRENT_LEVEL", 0);
//         levelText.text = $"Level {currentLevel + 1}";
//     }
// }
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelTimer : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI levelText;   // Level 1, Level 2
    public TextMeshProUGUI timerText;   // 30, 29, 28

    float timeLeft;
    bool running = false;

    void Start()
    {
        UpdateLevelText();
    }

    // =========================
    // ▶ START TIMER
    // =========================
    public void StartTimer(float seconds)
    {
        timeLeft = seconds;
        running = true;
        UpdateTimerText();
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
        timeLeft = Mathf.Max(timeLeft, 0f);

        UpdateTimerText();

        if (timeLeft <= 0f)
        {
            running = false;
            SceneManager.LoadScene("GameOverScene");
        }
    }

    // =========================
    // 🔥 UI UPDATES
    // =========================
    void UpdateLevelText()
    {
        if (levelText == null) return;

        int currentLevel = PlayerPrefs.GetInt("CURRENT_LEVEL", 0);
        levelText.text = $"Level {currentLevel + 1}";
    }

    void UpdateTimerText()
    {
        if (timerText == null) return;
        timerText.text = $"TIME: {Mathf.CeilToInt(timeLeft)}";

        
    }
}
