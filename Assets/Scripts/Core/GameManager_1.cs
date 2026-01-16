using UnityEngine;

public enum ChapterType
{
    Chapter1,
    Chapter2,
    Chapter1_LineConnect
}

public class GameManager_1 : MonoBehaviour
{
    public static GameManager_1 Instance;

    public ChapterType currentChapter;
    public int currentLevel = 1;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetChapter(ChapterType chapter)
    {
        currentChapter = chapter;
    }

    public void SetLevel(int level)
    {
        currentLevel = level;
    }
}
