
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Levels")]
    public LevelData[] levels;

    [Header("References")]
    public TargetPatternRenderer targetRenderer;
    public CircularLineController player;
    public LevelCompleteChecker checker;
    public LevelTimer timer;

    int currentLevelIndex = 0;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // ✅ DO NOT FORCE LEVEL 1 EVERY TIME
        currentLevelIndex = PlayerPrefs.GetInt("CURRENT_LEVEL", 0);
        LoadLevel(currentLevelIndex);
    }

    public void LoadLevel(int index)
    {
        if (index >= levels.Length)
        {
            Debug.Log("🎉 ALL LEVELS COMPLETE");
            PlayerPrefs.DeleteKey("CURRENT_LEVEL");
            SceneManager.LoadScene("LevelCompleteScene");
            return;
        }

        currentLevelIndex = index;
        PlayerPrefs.SetInt("CURRENT_LEVEL", currentLevelIndex);

        LevelData level = levels[index];
        Debug.Log($"▶ Loading Level {index + 1}");

        if (checker != null)
            checker.HideUI();

        if (player != null)
            player.ResetCompletely();

        // reset hint if exists
        HintButton hint = FindAnyObjectByType<HintButton>();
        if (hint != null)
            hint.ResetHints();

        targetRenderer.pattern = level.targetPattern;
        targetRenderer.Redraw();

        StopAllCoroutines();
        StartCoroutine(InitRope(level.initialRope));

        if (timer != null)
            timer.StartTimer(level.timeLimit);
    }

    IEnumerator InitRope(int[] rope)
    {
        yield return null;
        yield return null;
        player.SendMessage("CreateInitialRope", rope, SendMessageOptions.DontRequireReceiver);
    }

    public void NextLevel()
    {
        LoadLevel(currentLevelIndex + 1);
    }

    public LevelData GetCurrentLevel()
    {
        return levels[currentLevelIndex];
    }
    // =========================
// 🔁 REPLAY CURRENT LEVEL
// =========================
public void ReplayCurrentLevel()
{
    LoadLevel(currentLevelIndex);
}

}
