


using UnityEngine;

public enum RopeMovementRule
{
    AllMovable,
    StaticFromData,
    OnlyTopOrder   // ✅ NEW
}

[CreateAssetMenu(menuName = "Rope Puzzle/Level Data")]
public class LevelData_1 : ScriptableObject
{
    [Header("Level Info")]
    public int levelNumber;

    [Header("Move Limit")]
    public int maxMoves = 5;

    [Header("Movement Rule")]
    public RopeMovementRule movementRule;

    [Header("Slots - Part A")]
    public Vector2[] partASlotPositions;

    [Header("Slots - Part B")]
    public Vector2[] partBSlotPositions;

    [Header("Ropes")]
    public RopeData[] ropes;
}


[System.Serializable]
public class RopeData
{
    public Color ropeColor;
    public Vector2 nodeAPosition;
    public Vector2 nodeBPosition;
    public Material ropeMaterial;

    [Header("Movement")]
    public bool isMovable = true;   
}

