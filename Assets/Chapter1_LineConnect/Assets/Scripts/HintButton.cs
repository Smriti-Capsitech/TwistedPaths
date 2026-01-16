

// using UnityEngine;
// using System.Collections;

// public class HintButton : MonoBehaviour
// {
//     [Header("References")]
//     public LevelManager levelManager;
//     public CircularDotGenerator outerDots;

//     [Header("Visual")]
//     public Color hintColor = Color.cyan;
//     public float hintDuration = 2f;

//     bool hintUsed = false;
//     bool isShowing = false;

//     // =========================
//     // 💡 SHOW HINT
//     // =========================
//     public void ShowHint()
//     {
//         if (hintUsed || isShowing) return;
//         if (levelManager == null || outerDots == null) return;

//         LevelData level = levelManager.GetCurrentLevel();
//         if (level.hintDots == null || level.hintDots.Length == 0)
//             return;

//         StartCoroutine(FlashDots(level.hintDots));
//         hintUsed = true;
//     }

//     IEnumerator FlashDots(int[] indices)
//     {
//         isShowing = true;

//         SpriteRenderer[] renderers = new SpriteRenderer[indices.Length];
//         Color[] original = new Color[indices.Length];

//         for (int i = 0; i < indices.Length; i++)
//         {
//             int idx = indices[i];
//             if (idx < 0 || idx >= outerDots.dots.Count)
//                 continue;

//             SpriteRenderer sr = outerDots.dots[idx].GetComponent<SpriteRenderer>();
//             if (sr == null) continue;

//             renderers[i] = sr;
//             original[i] = sr.color;
//             sr.color = hintColor;
//         }

//         yield return new WaitForSeconds(hintDuration);

//         for (int i = 0; i < renderers.Length; i++)
//         {
//             if (renderers[i] != null)
//                 renderers[i].color = original[i];
//         }

//         isShowing = false;
//     }

//     // =========================
//     // 🔁 RESET HINT STATE
//     // =========================
//     public void ResetHints()
//     {
//         hintUsed = false;
//         isShowing = false;
//     }

//     // =========================
//     // 🔁 REPLAY CURRENT LEVEL
//     // =========================
//     public void ReplayLevel()
//     {
//         if (levelManager == null)
//         {
//             Debug.LogError("❌ LevelManager missing in HintButton");
//             return;
//         }

//         Debug.Log("🔁 Replay current level");

//         ResetHints();
//         levelManager.ReplayCurrentLevel();
//     }
// }
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class HintButton : MonoBehaviour
{
    [Header("References")]
    public LevelManager levelManager;
    public CircularDotGenerator outerDots;
    public InnerGridGenerator innerGrid; // 🔥 ADD THIS

    [Header("Visual")]
    public Color hintColor = Color.cyan;
    public float hintDuration = 2f;

    bool hintUsed = false;
    bool isShowing = false;

    // =========================
    // 💡 SHOW HINT
    // =========================
    public void ShowHint()
    {
        if (hintUsed || isShowing) return;
        if (levelManager == null || outerDots == null) return;

        LevelData level = levelManager.GetCurrentLevel();
        if (level == null || level.hintDots == null || level.hintDots.Length == 0)
            return;

        StartCoroutine(FlashDots(level.hintDots));
        hintUsed = true;
    }

    // =========================
    // 🔦 FLASH OUTER + INNER DOTS
    // =========================
    IEnumerator FlashDots(int[] indices)
    {
        isShowing = true;

        SpriteRenderer[] renderers = new SpriteRenderer[indices.Length];
        Color[] original = new Color[indices.Length];

        for (int i = 0; i < indices.Length; i++)
        {
            int idx = indices[i];
            SpriteRenderer sr = null;

            // -------- OUTER DOTS (0–11)
            if (idx >= 0 && idx < outerDots.dots.Count)
            {
                sr = outerDots.dots[idx].GetComponent<SpriteRenderer>();
            }
            // -------- INNER DOTS (12–15)
            else if (idx >= 12 && innerGrid != null)
            {
                InnerSnapNode node = innerGrid.nodes.Find(n => n.index == idx);
                if (node != null)
                    sr = node.GetComponent<SpriteRenderer>();
            }

            if (sr == null) continue;

            renderers[i] = sr;
            original[i] = sr.color;
            sr.color = hintColor;
        }

        yield return new WaitForSeconds(hintDuration);

        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i] != null)
                renderers[i].color = original[i];
        }

        isShowing = false;
    }

    // =========================
    // 🔁 RESET HINT STATE
    // =========================
    public void ResetHints()
    {
        hintUsed = false;
        isShowing = false;
    }

    // =========================
    // 🔁 REPLAY CURRENT LEVEL
    // =========================
    public void ReplayLevel()
    {
        if (levelManager == null)
        {
            Debug.LogError("❌ LevelManager missing in HintButton");
            return;
        }

        Debug.Log("🔁 Replay current level");

        ResetHints();
        levelManager.ReplayCurrentLevel();
    }
    // 🔙 BACK TO CHAPTER SELECT (SAFE)
    public void OnBackToHome()
    {
        // ✅ Always restore time
        Time.timeScale = 1f;

        // ✅ Destroy ONLY Chapter 2 LevelManager
        if (LevelManager_1.Instance != null)
        {
            Destroy(LevelManager_1.Instance.gameObject);
        }

        // ✅ Load chapter select
        SceneManager.LoadScene("ChapterSelectScene");
    }
}


