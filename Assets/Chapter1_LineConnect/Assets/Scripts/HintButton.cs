
// using UnityEngine;
// using System.Collections;
// using UnityEngine.SceneManagement;

// public class HintButton : MonoBehaviour
// {
//     [Header("References")]
//     public LevelManager levelManager;
//     public CircularLineController lineController;

//     [Header("UI")]
//     public GameObject circularBoard;
//     public GameObject settingsPanel;

//     [Header("Hint Visual")]
//     public Color hintColor = Color.cyan;
//     public float hintDuration = 2f;

//     bool hintUsed = false;
//     bool isShowing = false;
//     bool isPaused = false;

//     // =========================
//     // 💡 SHOW HINT
//     // =========================
//     public void ShowHint()
//     {
//         if (hintUsed || isShowing) return;

//         LevelData level = levelManager.GetCurrentLevel();
//         if (level == null || level.hintDots == null || level.hintDots.Length == 0)
//             return;

//         StartCoroutine(FlashDots(level.hintDots));
//         hintUsed = true;
//     }

//     IEnumerator FlashDots(int[] indices)
//     {
//         isShowing = true;
//         yield return new WaitForSeconds(hintDuration);
//         isShowing = false;
//     }

//     // =========================
//     // ⏸ PAUSE
//     // =========================
//     public void OnPause()
//     {
//         if (isPaused) return;
//         isPaused = true;

//         Time.timeScale = 0f;

//         // 🔥 Hide only gameplay visuals
//         if (circularBoard != null)
//             circularBoard.SetActive(false);

//         if (lineController != null)
//         {
//             LineRenderer lr = lineController.GetComponent<LineRenderer>();
//             if (lr != null) lr.enabled = false;
//         }

//         if (settingsPanel != null)
//             settingsPanel.SetActive(true);
//     }

//     // =========================
//     // ▶ RESUME
//     // =========================
//     public void OnResume()
//     {
//         Time.timeScale = 1f;
//         isPaused = false;

//         if (settingsPanel != null)
//             settingsPanel.SetActive(false);

//         if (circularBoard != null)
//             circularBoard.SetActive(true);

//         if (lineController != null)
//         {
//             LineRenderer lr = lineController.GetComponent<LineRenderer>();
//             if (lr != null) lr.enabled = true;
//         }
//     }

//     // =========================
//     // 🔁 RESTART
//     // =========================
//     public void OnRestart()
//     {
//         Time.timeScale = 1f;
//         isPaused = false;

//         if (settingsPanel != null)
//             settingsPanel.SetActive(false);

//         if (circularBoard != null)
//             circularBoard.SetActive(true);

//         if (lineController != null)
//         {
//             LineRenderer lr = lineController.GetComponent<LineRenderer>();
//             if (lr != null) lr.enabled = true;
//         }

//         levelManager.ReplayCurrentLevel();
//     }

//     // =========================
//     // 🏠 HOME
//     // =========================
//     public void OnHome()
//     {
//         Time.timeScale = 1f;
//         SceneManager.LoadScene("ChapterSelectScene");
//     }

//     // =========================
//     // 🔄 RESET
//     // =========================
//     public void ResetHints()
//     {
//         hintUsed = false;
//         isShowing = false;
//     }
// }
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class HintButton : MonoBehaviour
{
    [Header("References")]
    public LevelManager levelManager;
    public CircularLineController lineController;

    [Header("UI")]
    public GameObject circularBoard;
    public GameObject settingsPanel;

    [Header("Hint Visual")]
    public Color hintColor = Color.cyan;
    public float hintDuration = 2f;

    bool hintUsed = false;
    bool isShowing = false;
    bool isPaused = false;

    // =========================
    // 💡 SHOW HINT
    // =========================
    public void ShowHint()
    {
        if (hintUsed || isShowing) return;

        LevelData level = levelManager.GetCurrentLevel();
        if (level == null || level.hintDots == null || level.hintDots.Length == 0)
            return;

        StartCoroutine(FlashDots(level.hintDots));
        hintUsed = true;
    }

    IEnumerator FlashDots(int[] indices)
    {
        isShowing = true;
        yield return new WaitForSeconds(hintDuration);
        isShowing = false;
    }

    // =========================
    // ⏸ PAUSE
    // =========================
    public void OnPause()
    {
        if (isPaused) return;
        isPaused = true;

        Time.timeScale = 0f;

        // Hide gameplay board
        if (circularBoard != null)
            circularBoard.SetActive(false);

        // Hide rope visuals ONLY (not GameObjects)
        SetRopeVisuals(false);

        // Hide line
        if (lineController != null)
        {
            LineRenderer lr = lineController.GetComponent<LineRenderer>();
            if (lr != null) lr.enabled = false;
        }

        if (settingsPanel != null)
            settingsPanel.SetActive(true);
    }

    // =========================
    // ▶ RESUME
    // =========================
    public void OnResume()
    {
        Time.timeScale = 1f;
        isPaused = false;

        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        if (circularBoard != null)
            circularBoard.SetActive(true);

        // Restore rope visuals
        SetRopeVisuals(true);

        if (lineController != null)
        {
            LineRenderer lr = lineController.GetComponent<LineRenderer>();
            if (lr != null) lr.enabled = true;
        }
    }

    // =========================
    // 🔁 RESTART
    // =========================
    public void OnRestart()
    {
        Time.timeScale = 1f;
        isPaused = false;

        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        if (circularBoard != null)
            circularBoard.SetActive(true);

        SetRopeVisuals(true);

        if (lineController != null)
        {
            LineRenderer lr = lineController.GetComponent<LineRenderer>();
            if (lr != null) lr.enabled = true;
        }

        levelManager.ReplayCurrentLevel();
    }

    // =========================
    // 🏠 HOME
    // =========================
    public void OnHome()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("ChapterSelectScene");
    }

    // =========================
    // 🔄 RESET
    // =========================
    public void ResetHints()
    {
        hintUsed = false;
        isShowing = false;
    }

    // =========================
    // 🔥 ROPE VISUAL FIX (FINAL)
    // =========================
    void SetRopeVisuals(bool visible)
    {
        RopeNode[] ropeNodes =
            Object.FindObjectsByType<RopeNode>(FindObjectsSortMode.None);

        foreach (var node in ropeNodes)
        {
            if (node == null) continue;

            // Hide sprite
            SpriteRenderer sr = node.GetComponent<SpriteRenderer>();
            if (sr != null) sr.enabled = visible;

            // Disable interaction
            Collider2D col = node.GetComponent<Collider2D>();
            if (col != null) col.enabled = visible;
        }
    }
}
