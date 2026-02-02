<<<<<<< Updated upstream
﻿
using UnityEngine;
=======
﻿using UnityEngine;
>>>>>>> Stashed changes
using UnityEngine.SceneManagement;

public class LevelCompleteUI_1 : MonoBehaviour
{
    public static LevelCompleteUI_1 Instance;
    public GameObject panel;

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(Instance.gameObject);

        Instance = this;

        if (panel == null)
            panel = gameObject;

        panel.SetActive(false);
    }

    // =========================
    // SHOW LEVEL COMPLETE
    // =========================
    public void Show()
    {
        panel.SetActive(true);
<<<<<<< Updated upstream

        // ❌ Banner must NEVER appear here
        if (AdManager.Instance != null)
            AdManager.Instance.HideBanner();

        // ⛔ Pause gameplay
        Time.timeScale = 0f;
    }

    // =========================
    // HIDE LEVEL COMPLETE
    // =========================
    public void Hide()
    {
        panel.SetActive(false);

        // ✅ Resume gameplay first
        Time.timeScale = 1f;

        // ✅ Count level completion AFTER resume
        if (AdManager.Instance != null)
            AdManager.Instance.OnLevelComplete();
=======
        Time.timeScale = 0f;
>>>>>>> Stashed changes
    }

    // =========================
    // NEXT LEVEL
    // =========================
    public void OnNext()
    {
        Hide();

<<<<<<< Updated upstream
        int chapter = 2; // 🔥 Chapter 2 ONLY
        int currentLevel = PlayerPrefs.GetInt("CURRENT_LEVEL", 0);

        // ⭐ Save stars
        PlayerPrefs.SetInt($"CH{chapter}_LEVEL_STARS_{currentLevel}", 3);

        // 🔓 Unlock next level
        int unlocked = PlayerPrefs.GetInt($"CH{chapter}_UNLOCKED_LEVEL", 0);
        if (currentLevel + 1 > unlocked)
            PlayerPrefs.SetInt($"CH{chapter}_UNLOCKED_LEVEL", currentLevel + 1);

        PlayerPrefs.Save();
=======
        int currentLevel =
            PlayerPrefs.GetInt("CURRENT_LEVEL", 0);

        PlayerPrefs.SetInt("CURRENT_LEVEL", currentLevel + 1);
>>>>>>> Stashed changes

        // 🛑 Last level → Chapter Select
        if (LevelManager_1.Instance.IsLastLevel())
        {
            SceneManager.LoadScene("ChapterSelectScene");
            return;
        }

<<<<<<< Updated upstream
        // ▶ Load next level
        PlayerPrefs.SetInt("CURRENT_LEVEL", currentLevel + 1);
=======
>>>>>>> Stashed changes
        LevelManager_1.Instance.NextLevel();
    }

    // =========================
    // RESTART LEVEL
    // =========================
    public void OnRestart()
    {
        Hide();
        LevelManager_1.Instance.RestartLevel();
    }

    // =========================
    // BACK TO HOME
    // =========================
    public void OnBackToHome()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("ChapterSelectScene");
    }
}
