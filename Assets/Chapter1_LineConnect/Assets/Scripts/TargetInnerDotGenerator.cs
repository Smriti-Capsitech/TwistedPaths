using UnityEngine;
using System.Collections.Generic;

public class TargetInnerDotGenerator : MonoBehaviour
{
    public GameObject dotPrefab;
    public SpriteRenderer board;

    public float innerRadiusMultiplier = 0.035f;

    [HideInInspector]
    public List<Transform> dots = new();

    void Start()
    {
        float boardRadius = board.bounds.size.x * 0.5f;
        float radius = boardRadius * innerRadiusMultiplier;

        for (int i = 0; i < 4; i++)
        {
            float angle = i * Mathf.PI * 2f / 4f;

            Vector3 pos = new Vector3(
                Mathf.Cos(angle),
                Mathf.Sin(angle),
                0
            ) * radius;

            GameObject d = Instantiate(dotPrefab, transform);
            d.transform.localPosition = pos;
            dots.Add(d.transform);
        }
    }
}
