// using UnityEngine;

// [CreateAssetMenu(fileName = "LevelData", menuName = "LineConnect/Level Data")]
// public class LevelData : ScriptableObject
// {
//     [Header("Outer Pattern")]
//     public int[] targetPattern;

//     [Header("Initial Rope")]
//     public int[] initialRope;

//     [Header("Preview")]
//     public bool showPreview = true;
//     public float timeLimit;
//     public bool hasHint = false;
//     public int[] hintDots;  

    
// }
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "LineConnect/Level Data")]
public class LevelData : ScriptableObject
{
    [Header("Target Pattern (Outer 0–11, Inner 12–16)")]
    public int[] targetPattern;

    [Header("Initial Rope (Same Index Rules)")]
    public int[] initialRope;

    [Header("Preview")]
    public bool showPreview = true;

    [Header("Timer")]
    public float timeLimit;

    [Header("Hint Settings (Outer Dots Only)")]
    public bool hasHint = false;
    public int[] hintDots;
}
