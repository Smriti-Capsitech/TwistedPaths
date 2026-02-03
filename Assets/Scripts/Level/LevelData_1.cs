


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
    [Header("Identification")]
    public int ropeID;                 // ✅ ADD THIS

    [Header("Prefab")]
    public GameObject ropePrefab;      // ✅ ADD THIS

    [Header("Visual")]
    public Color ropeColor;
    public Material ropeMaterial;

    [Header("Nodes")]
    public Vector2 nodeAPosition;
    public Vector2 nodeBPosition;

    [Header("Movement")]
    public bool isMovable = true;
}


