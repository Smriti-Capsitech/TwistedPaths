using UnityEngine;

public enum RopeMovementRule
{
    AllMovable,
    StaticFromData,
    OnlyTopOrder
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
    [Header("Rope Identification")]
    public int ropeID;

    [Header("Rope Prefab (MUST ASSIGN)")]
    public GameObject ropePrefab;

    [Header("Node Positions")]
    public Vector2 nodeAPosition;
    public Vector2 nodeBPosition;

    [Header("Movement")]
    public bool isMovable = true;
}
