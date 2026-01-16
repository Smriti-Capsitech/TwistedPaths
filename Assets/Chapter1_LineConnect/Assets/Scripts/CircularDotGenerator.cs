// using UnityEngine;
// using System.Collections.Generic;

// public class CircularDotGenerator : MonoBehaviour
// {
//     [Header("References")]
//     public GameObject dotPrefab;
//     public SpriteRenderer circularBoard;   // assign board sprite here

//     [Header("Settings")]
//     public int dotCount = 12;
//     [Range(0.6f, 0.95f)]
//     public float radiusMultiplier = 0.85f; // how close to board edge

//     public List<Transform> dots = new List<Transform>();

//     void Start()
//     {
//         GenerateDots();
//     }

//     void GenerateDots()
//     {
//         // cleanup if re-running
//         foreach (Transform child in transform)
//             Destroy(child.gameObject);

//         dots.Clear();

//         // get board radius in WORLD units
//         float boardRadius = circularBoard.bounds.size.x * 0.5f;
//         float radius = boardRadius * radiusMultiplier;

//         for (int i = 0; i < dotCount; i++)
//         {
//             float angle = (360f / dotCount) * i * Mathf.Deg2Rad;

//             Vector3 localPos = new Vector3(
//                 Mathf.Cos(angle) * radius,
//                 Mathf.Sin(angle) * radius,
//                 -0.1f   // slightly in front of board
//             );

//             GameObject dot = Instantiate(dotPrefab, transform);
//             dot.transform.localPosition = localPos;
//             dot.name = "Dot_" + i;

//             dots.Add(dot.transform);
//         }
//     }
// }
using UnityEngine;
using System.Collections.Generic;

public class CircularDotGenerator : MonoBehaviour
{
    public GameObject dotPrefab;
    public SpriteRenderer board;
    public int dotCount = 12;
    public float radiusMultiplier = 0.85f;

    public List<CircleDot> dots = new();

    void Start()
    {
        float radius = board.bounds.size.x * 0.5f * radiusMultiplier;

        for (int i = 0; i < dotCount; i++)
        {
            
            float angle = i * Mathf.PI * 2 / dotCount;

            Vector3 pos = new Vector3(
                Mathf.Cos(angle),
                Mathf.Sin(angle),
                0
            ) * radius;

            GameObject obj = Instantiate(dotPrefab, transform);
            obj.transform.localPosition = pos;

            CircleDot dot = obj.GetComponent<CircleDot>();
            dot.index = i;
            dots.Add(dot);
        }
    }
}
