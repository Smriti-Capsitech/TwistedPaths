using UnityEngine;
using TMPro;

public class MoveCounterUI : MonoBehaviour
{
    public static MoveCounterUI Instance;

    [Header("UI")]
    public TextMeshProUGUI moveText;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void UpdateMoves(int used, int max)
    {
        if (moveText == null) return;

        int left = Mathf.Max(0, max - used);
        moveText.text = $"Moves Left: {left}/{max}";

        if (left == 0)
            moveText.color = Color.red;
        else if (left == 1)
            moveText.color = new Color(1f, 0.6f, 0f); // orange
        else
            moveText.color = Color.white;
    }
}
