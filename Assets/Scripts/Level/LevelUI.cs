using UnityEngine;
using TMPro;

public class LevelUI : MonoBehaviour
{
    public static LevelUI Instance;

    public TextMeshProUGUI levelText;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void SetLevel(int levelNumber)
    {
        levelText.text = $"Level {levelNumber}";
    }
}
