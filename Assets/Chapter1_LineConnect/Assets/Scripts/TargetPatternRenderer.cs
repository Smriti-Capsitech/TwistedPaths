
// using UnityEngine;
// using System.Collections;
// using System.Collections.Generic;

// [RequireComponent(typeof(LineRenderer))]
// public class TargetPatternRenderer : MonoBehaviour
// {
//     [Header("Generators")]
//     public TargetDotGenerator outer;
//     public TargetInnerDotGenerator inner;

//     [Header("Pattern (OUTER 0–11, INNER 12–15)")]
//     public int[] pattern;

//     LineRenderer line;

//     void Awake()
//     {
//         line = GetComponent<LineRenderer>();
//         line.useWorldSpace = true;
//         line.positionCount = 0;
//     }

//     void Start()
//     {
//         Redraw(); // safe start
//     }

//     // 🔁 SAFE PUBLIC REDRAW
//     public void Redraw()
//     {
//         if (!gameObject.activeInHierarchy)
//             gameObject.SetActive(true);

//         StopAllCoroutines();
//         StartCoroutine(WaitAndDraw());
//     }

//     IEnumerator WaitAndDraw()
//     {
//         yield return new WaitUntil(() =>
//             outer != null &&
//             outer.dots != null &&
//             outer.dots.Count > 0
//         );

//         // inner dots may or may not exist (level dependent)
//         DrawPattern();
//     }

//     void DrawPattern()
//     {
//         if (pattern == null || pattern.Length < 2)
//         {
//             Debug.LogError("TargetPatternRenderer: Pattern too small");
//             return;
//         }

//         line.positionCount = pattern.Length;

//         for (int i = 0; i < pattern.Length; i++)
//         {
//             int id = pattern[i];

//             // ------------------
//             // OUTER DOT (0–11)
//             // ------------------
//             if (id >= 0 && id < outer.dots.Count)
//             {
//                 line.SetPosition(i, outer.dots[id].position);
//             }
//             // ------------------
//             // INNER DOT (12–15)
//             // ------------------
//             else if (id >= 12 && inner != null && inner.dots != null)
//             {
//                 int innerIndex = id - 12;

//                 if (innerIndex >= 0 && innerIndex < inner.dots.Count)
//                 {
//                     line.SetPosition(i, inner.dots[innerIndex].position);
//                 }
//                 else
//                 {
//                     Debug.LogError($"Invalid INNER index {id}");
//                     return;
//                 }
//             }
//             else
//             {
//                 Debug.LogError($"Invalid pattern index {id}");
//                 return;
//             }
//         }

//         Debug.Log($"Target pattern drawn. Count = {line.positionCount}");
//     }

//     public int PatternCount => line.positionCount;
//     public Vector3 GetPoint(int i) => line.GetPosition(i);
// }
// using UnityEngine;
// using System.Collections;
// using System.Collections.Generic;

// [RequireComponent(typeof(LineRenderer))]
// public class TargetPatternRenderer : MonoBehaviour
// {
//     [Header("Generators")]
//     public TargetDotGenerator outer;
//     public TargetInnerDotGenerator inner;

//     [Header("Pattern (OUTER 0–11, INNER 12–16)")]
//     public int[] pattern;

//     LineRenderer line;

//     void Awake()
//     {
//         line = GetComponent<LineRenderer>();
//         line.useWorldSpace = true;
//         line.positionCount = 0;
//     }

//     void Start()
//     {
//         Redraw(); // safe start
//     }

//     // 🔁 SAFE PUBLIC REDRAW
//     public void Redraw()
//     {
//         if (!gameObject.activeInHierarchy)
//             gameObject.SetActive(true);

//         StopAllCoroutines();
//         StartCoroutine(WaitAndDraw());
//     }

//     IEnumerator WaitAndDraw()
//     {
//         yield return new WaitUntil(() =>
//             outer != null &&
//             outer.dots != null &&
//             outer.dots.Count > 0
//         );

//         DrawPattern();
//     }

//     void DrawPattern()
//     {
//         if (pattern == null || pattern.Length < 2)
//         {
//             Debug.LogError("TargetPatternRenderer: Pattern too small");
//             return;
//         }

//         line.positionCount = pattern.Length;

//         for (int i = 0; i < pattern.Length; i++)
//         {
//             int id = pattern[i];

//             // ======================
//             // OUTER DOT (0–11)
//             // ======================
//             if (id >= 0 && id < outer.dots.Count)
//             {
//                 line.SetPosition(i, outer.dots[id].position);
//                 continue;
//             }

//             // ======================
//             // INNER DOT (12–16)
//             // ======================
//             if (id >= 12 && inner != null && inner.dots != null)
//             {
//                 Transform found = null;

//                 foreach (Transform t in inner.dots)
//                 {
//                     InnerSnapNode n = t.GetComponent<InnerSnapNode>();
//                     if (n != null && n.index == id)
//                     {
//                         found = t;
//                         break;
//                     }
//                 }

//                 if (found != null)
//                 {
//                     line.SetPosition(i, found.position);
//                     continue;
//                 }

//                 Debug.LogError($"Inner node with index {id} not found");
//                 return;
//             }

//             // ======================
//             // INVALID
//             // ======================
//             Debug.LogError($"Invalid pattern index {id}");
//             return;
//         }

//         Debug.Log($"Target pattern drawn. Count = {line.positionCount}");
//     }

//     public int PatternCount => line.positionCount;
//     public Vector3 GetPoint(int i) => line.GetPosition(i);
// }
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class TargetPatternRenderer : MonoBehaviour
{
    [Header("Generators")]
    public TargetDotGenerator outer;
    public TargetInnerDotGenerator inner;

    [Header("Pattern (OUTER 0–11, INNER 12–16)")]
    public int[] pattern;

    LineRenderer line;

    void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.useWorldSpace = true;
        line.positionCount = 0;
    }

    void Start()
    {
        Redraw();
    }

    // 🔁 SAFE PUBLIC REDRAW
    public void Redraw()
    {
        if (!gameObject.activeInHierarchy)
            gameObject.SetActive(true);

        StopAllCoroutines();
        StartCoroutine(WaitAndDraw());
    }

    IEnumerator WaitAndDraw()
    {
        yield return new WaitUntil(() =>
            outer != null &&
            outer.dots != null &&
            outer.dots.Count > 0
        );

        DrawPattern();
    }

    void DrawPattern()
    {
        if (pattern == null || pattern.Length < 2)
        {
            Debug.LogError("TargetPatternRenderer: Pattern too small");
            return;
        }

        line.positionCount = pattern.Length;

        for (int i = 0; i < pattern.Length; i++)
        {
            int id = pattern[i];

            // ======================
            // OUTER DOT (0–11)
            // ======================
            if (id >= 0 && id < outer.dots.Count)
            {
                line.SetPosition(i, outer.dots[id].position);
                continue;
            }

            // ======================
            // INNER DOTS (12–16)
            // VISUAL-ONLY MAPPING
            // ======================
            if (id >= 12 && inner != null && inner.dots != null && inner.dots.Count > 0)
            {
                Transform found = null;

                switch (id)
                {
                    case 12: // CENTER HUB
                        found = FindClosestToCenter(inner.dots);
                        break;

                    case 13: // TOP
                        found = FindHighest(inner.dots);
                        break;

                    case 14: // RIGHT
                        found = FindRightmost(inner.dots);
                        break;

                    case 15: // BOTTOM
                        found = FindLowest(inner.dots);
                        break;

                    case 16: // LEFT
                        found = FindLeftmost(inner.dots);
                        break;
                }

                if (found != null)
                {
                    line.SetPosition(i, found.position);
                    continue;
                }

                Debug.LogError($"Inner visual dot for index {id} not found");
                return;
            }

            // ======================
            // INVALID
            // ======================
            Debug.LogError($"Invalid pattern index {id}");
            return;
        }

        Debug.Log($"Target pattern drawn. Count = {line.positionCount}");
    }

    // ==================================================
    // VISUAL INNER DOT HELPERS (POSITION BASED)
    // ==================================================

    Transform FindHighest(List<Transform> dots)
    {
        Transform best = null;
        float maxY = float.MinValue;

        foreach (var t in dots)
        {
            if (t.position.y > maxY)
            {
                maxY = t.position.y;
                best = t;
            }
        }
        return best;
    }

    Transform FindLowest(List<Transform> dots)
    {
        Transform best = null;
        float minY = float.MaxValue;

        foreach (var t in dots)
        {
            if (t.position.y < minY)
            {
                minY = t.position.y;
                best = t;
            }
        }
        return best;
    }

    Transform FindRightmost(List<Transform> dots)
    {
        Transform best = null;
        float maxX = float.MinValue;

        foreach (var t in dots)
        {
            if (t.position.x > maxX)
            {
                maxX = t.position.x;
                best = t;
            }
        }
        return best;
    }

    Transform FindLeftmost(List<Transform> dots)
    {
        Transform best = null;
        float minX = float.MaxValue;

        foreach (var t in dots)
        {
            if (t.position.x < minX)
            {
                minX = t.position.x;
                best = t;
            }
        }
        return best;
    }

    Transform FindClosestToCenter(List<Transform> dots)
    {
        Transform best = null;
        float min = float.MaxValue;

        foreach (var t in dots)
        {
            float d = t.localPosition.sqrMagnitude;
            if (d < min)
            {
                min = d;
                best = t;
            }
        }
        return best;
    }

    public int PatternCount => line.positionCount;
    public Vector3 GetPoint(int i) => line.GetPosition(i);
}
