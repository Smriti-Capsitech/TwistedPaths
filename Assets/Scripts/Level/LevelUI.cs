using UnityEngine;
using TMPro;

public class LevelUI : MonoBehaviour
{
    public static LevelUI Instance;

    public TextMeshProUGUI levelText;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void SetLevel(int levelNumber)
    {
        if (levelText != null)
            levelText.text = "Level " + levelNumber;
    }
}
