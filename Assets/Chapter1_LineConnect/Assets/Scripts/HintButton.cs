
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

        CircularDotGenerator dotGen =
            Object.FindAnyObjectByType<CircularDotGenerator>();

        if (dotGen == null)
        {
            isShowing = false;
            yield break;
        }

        SpriteRenderer[] highlighted = new SpriteRenderer[indices.Length];

        // 🔥 Highlight dots
        for (int i = 0; i < indices.Length; i++)
        {
            int idx = indices[i];
            if (idx < 0 || idx >= dotGen.dots.Count) continue;

            CircleDot dot = dotGen.dots[idx];
            SpriteRenderer sr = dot.GetComponent<SpriteRenderer>();
            if (sr == null) continue;

            highlighted[i] = sr;
            sr.color = hintColor;
        }

        // ⛔ Pause-safe wait
        yield return new WaitForSecondsRealtime(hintDuration);

        // 🔄 Restore colors
        foreach (var sr in highlighted)
        {
            if (sr != null)
                sr.color = Color.white;
        }

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

        if (circularBoard != null)
            circularBoard.SetActive(false);

        SetRopeVisuals(false);

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
        ResetHints();

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
    // 🔥 ROPE VISUALS
    // =========================
    void SetRopeVisuals(bool visible)
    {
        RopeNode[] ropeNodes =
            Object.FindObjectsByType<RopeNode>(FindObjectsSortMode.None);

        foreach (var node in ropeNodes)
        {
            if (node == null) continue;

            SpriteRenderer sr = node.GetComponent<SpriteRenderer>();
            if (sr != null) sr.enabled = visible;

            Collider2D col = node.GetComponent<Collider2D>();
            if (col != null) col.enabled = visible;
        }
    }
}
