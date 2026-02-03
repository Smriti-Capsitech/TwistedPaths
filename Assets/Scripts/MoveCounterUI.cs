using UnityEngine;
using TMPro;

public class MoveCounterUI : MonoBehaviour
{
    public static MoveCounterUI Instance;

    [Header("UI")]
    public TextMeshProUGUI moveText;

    private int usedMoves;
    private int maxMoves;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (moveText == null)
        {
            Debug.LogError("MoveCounterUI: Move Text not assigned!");
            return;
        }
    }

    public void ResetMoves(int max)
    {
        maxMoves = max;
        usedMoves = 0;
        RefreshUI();
    }

    public void UseMove()
    {
        usedMoves++;
        usedMoves = Mathf.Clamp(usedMoves, 0, maxMoves);
        RefreshUI();
    }

    void RefreshUI()
    {
        int left = Mathf.Max(0, maxMoves - usedMoves);
        moveText.text = $"Moves: {left}/{maxMoves}";

        if (left == 0)
            moveText.color = Color.red;
        else if (left == 1)
            moveText.color = new Color(1f, 0.6f, 0f);
        else
            moveText.color = Color.white;
    }

    public bool NoMovesLeft()
    {
        return usedMoves >= maxMoves;
    }

    public void AddExtraMoves(int amount)
{
    usedMoves -= amount;
    usedMoves = Mathf.Clamp(usedMoves, 0, maxMoves);
    RefreshUI();
}

}
