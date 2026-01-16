// using UnityEngine;
// using System.Collections.Generic;

// public class TargetDotGenerator : MonoBehaviour
// {
//     public GameObject dotPrefab;
//     public SpriteRenderer board;

//     public int dotCount = 8;
//     public float radiusMultiplier = 0.06f;

//     [HideInInspector]
//     public List<Transform> dots = new();

//     void Start()
//     {
//         float boardRadius = board.bounds.size.x * 0.5f;
//         float radius = boardRadius * radiusMultiplier;

//         for (int i = 0; i < dotCount; i++)
//         {
//             float angle = i * Mathf.PI * 2f / dotCount;

//             Vector3 pos = new Vector3(
//                 Mathf.Cos(angle),
//                 Mathf.Sin(angle),
//                 0
//             ) * radius;

//             GameObject d = Instantiate(dotPrefab, transform);
//             d.transform.localPosition = pos;
//             dots.Add(d.transform);
//         }
//     }
// }
using UnityEngine;
using System.Collections.Generic;

public class TargetDotGenerator : MonoBehaviour
{
    public GameObject dotPrefab;
    public SpriteRenderer board;

    public int dotCount = 12;
    public float radiusMultiplier = 0.06f;

    [HideInInspector]
    public List<Transform> dots = new();

    void Start()
    {
        dots.Clear();

        float boardRadius = board.bounds.size.x * 0.5f;
        float radius = boardRadius * radiusMultiplier;

        // ======================
        // OUTER 8 DOTS
        // ======================
        for (int i = 0; i < dotCount; i++)
        {
            float angle = i * Mathf.PI * 2f / dotCount;

            Vector3 pos = new Vector3(
                Mathf.Cos(angle),
                Mathf.Sin(angle),
                0
            ) * radius;

            GameObject d = Instantiate(dotPrefab, transform);
            d.transform.localPosition = pos;
            dots.Add(d.transform);
        }

        // ======================
        // CENTER HUB (1)
        // ======================
        GameObject center = Instantiate(dotPrefab, transform);
        center.transform.localPosition = Vector3.zero;
        dots.Add(center.transform);
    }
}
