using UnityEngine;
using System.Collections.Generic;

public class InnerGridGenerator : MonoBehaviour
{
    public GameObject innerNodePrefab;
    public SpriteRenderer board;
    public float innerRadiusMultiplier = 0.1f;

    public List<InnerSnapNode> nodes = new();

    void Start()
    {
        GenerateInnerGrid();
    }

    void GenerateInnerGrid()
    {
        nodes.Clear();

        float boardRadius = board.bounds.size.x * 0.5f;
        float innerRadius = boardRadius * innerRadiusMultiplier;

        // =========================
        // 12 → CENTER HUB
        // =========================
        CreateNode(Vector3.zero, 12);

        // =========================
        // 13 → TOP INNER
        // =========================
        CreateNode(Vector3.up * innerRadius, 13);

        // =========================
        // 14 → RIGHT INNER
        // =========================
        CreateNode(Vector3.right * innerRadius, 14);

        // =========================
        // 15 → BOTTOM INNER
        // =========================
        CreateNode(Vector3.down * innerRadius, 15);

        // =========================
        // 16 → LEFT INNER (MISSING BEFORE)
        // =========================
        CreateNode(Vector3.left * innerRadius, 16);
    }

    void CreateNode(Vector3 localPos, int index)
    {
        GameObject obj = Instantiate(innerNodePrefab, transform);
        obj.transform.localPosition = localPos;

        InnerSnapNode node = obj.GetComponent<InnerSnapNode>();
        node.index = index;
        node.isOccupied = false;

        nodes.Add(node);
    }
}
