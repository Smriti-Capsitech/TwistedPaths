

// using UnityEngine;
// using System.Collections;
// using System.Collections.Generic;

// public class GameManager : MonoBehaviour
// {
//     public static GameManager Instance;

//     [Header("Runtime Ropes")]
//     public RopeController_1[] ropes;

//     [Header("Moves")]
//     public int maxMoves;
//     public int currentMoves;

//     private bool levelEnded = false;
//     public bool IsLevelEnding => levelEnded;

//     [HideInInspector]
//     public LevelData_1 currentLevelData;

//     // ==================================================
//     // UNITY
//     // ==================================================
//     void Awake()
//     {
//         if (Instance != null)
//         {
//             Destroy(gameObject);
//             return;
//         }
//         Instance = this;
//     }

//     // ==================================================
//     // RESET
//     // ==================================================
//     public void ResetState()
//     {
//         levelEnded = false;
//         currentMoves = 0;
//     }

//     // ==================================================
//     // MOVE LIMIT
//     // ==================================================
//     public void SetMoveLimit(int moves)
//     {
//         maxMoves = moves;
//         currentMoves = 0;

//         if (MoveCounterUI.Instance != null)
//             MoveCounterUI.Instance.UpdateMoves(currentMoves, maxMoves);
//     }

//     // ==================================================
//     // REGISTER MOVE
//     // ==================================================
//     // public void RegisterMove()
//     // {
//     //     if (levelEnded) return;

//     //     currentMoves++;

//     //     if (MoveCounterUI.Instance != null)
//     //         MoveCounterUI.Instance.UpdateMoves(currentMoves, maxMoves);

//     //     if (currentMoves >= maxMoves)
//     //     {
//     //         if (IsLevelSolved())
//     //             WinLevel();
//     //         else
//     //             GameOver();
//     //     }
//     // }
//     public void RegisterMove()
// {
//     if (levelEnded) return;

//     currentMoves++;

//     if (MoveCounterUI.Instance != null)
//         MoveCounterUI.Instance.UpdateMoves(currentMoves, maxMoves);

//     // ✅ IMPORTANT: CHECK SOLVED FIRST
//     if (IsLevelSolved())
//     {
//         WinLevel();
//         return;
//     }

//     // ❌ ONLY THEN CHECK GAME OVER
//     if (currentMoves >= maxMoves)
//     {
//         GameOver();
//     }
// }


//     // ==================================================
//     // SOLVED CHECK
//     // ==================================================
//     bool IsLevelSolved()
//     {
//         foreach (var rope in ropes)
//         {
//             if (IsRopeIntersecting(rope))
//                 return false;
//         }
//         return true;
//     }

//     // ==================================================
//     // ROPE INTERSECTION (BOOLEAN)
//     // ==================================================
//     public bool IsRopeIntersecting(RopeController_1 rope)
//     {
//         foreach (var other in ropes)
//         {
//             if (other == rope) continue;

//             if (LinesIntersect(
//                 rope.nodeA.position,
//                 rope.nodeB.position,
//                 other.nodeA.position,
//                 other.nodeB.position))
//             {
//                 return true;
//             }
//         }
//         return false;
//     }

//     // ==================================================
//     // REGISTER ROPES (LEVEL LOAD)
//     // ==================================================
//     public void RegisterRopes(RopeController_1[] levelRopes)
//     {
//         ropes = levelRopes;

//         int order = 0;
//         foreach (var rope in ropes)
//             rope.SetOrder(++order);

//         UpdateTopOrderMovability();
//     }

//     // ==================================================
//     // EARLY WIN CHECK
//     // ==================================================
//     public void CheckLevelComplete()
//     {
//         if (!levelEnded && IsLevelSolved())
//             WinLevel();
//     }

//     // ==================================================
//     // WIN / LOSE
//     // ==================================================
//     void WinLevel()
//     {
//         if (levelEnded) return;
//         levelEnded = true;

//         StartCoroutine(ShowWinAfterDelay());
//     }

//     IEnumerator ShowWinAfterDelay()
//     {
//         yield return new WaitForSeconds(1f);

//         if (LevelCompleteUI_1.Instance != null)
//             LevelCompleteUI_1.Instance.Show();
//     }

//     public void GameOver()
//     {
//         if (levelEnded) return;
//         levelEnded = true;

//         StartCoroutine(ShowLoseAfterDelay());
//     }

//     IEnumerator ShowLoseAfterDelay()
//     {
//         yield return new WaitForSeconds(1f);

//         if (GameOverUI_1.Instance != null)
//             GameOverUI_1.Instance.Show();
//     }

//     // ==================================================
//     // LINE INTERSECTION (BOOLEAN)
//     // ==================================================
//     bool LinesIntersect(Vector2 A, Vector2 B, Vector2 C, Vector2 D)
//     {
//         float den = (A.x - B.x) * (C.y - D.y) -
//                     (A.y - B.y) * (C.x - D.x);

//         if (Mathf.Abs(den) < 0.0001f)
//             return false;

//         float t = ((A.x - C.x) * (C.y - D.y) -
//                    (A.y - C.y) * (C.x - D.x)) / den;

//         float u = -((A.x - B.x) * (A.y - C.y) -
//                     (A.y - B.y) * (A.x - C.x)) / den;

//         return t > 0f && t < 1f && u > 0f && u < 1f;
//     }

//     // ==================================================
//     // LINE INTERSECTION (WITH POINT) 🔥 REQUIRED BY ROPE
//     // ==================================================
//     public bool TryGetIntersection(
//         Vector2 A, Vector2 B,
//         Vector2 C, Vector2 D,
//         out Vector2 intersection)
//     {
//         intersection = Vector2.zero;

//         float den = (A.x - B.x) * (C.y - D.y) -
//                     (A.y - B.y) * (C.x - D.x);

//         if (Mathf.Abs(den) < 0.0001f)
//             return false;

//         float t = ((A.x - C.x) * (C.y - D.y) -
//                    (A.y - C.y) * (C.x - D.x)) / den;

//         float u = -((A.x - B.x) * (A.y - C.y) -
//                     (A.y - B.y) * (A.x - C.x)) / den;

//         if (t > 0f && t < 1f && u > 0f && u < 1f)
//         {
//             intersection = A + t * (B - A);
//             return true;
//         }

//         return false;
//     }

//     // ==================================================
//     // 🔥 TOP-ORDER MOVEMENT RULE
//     // ==================================================
//     public void UpdateTopOrderMovability()
//     {
//         if (currentLevelData == null) return;
//         if (currentLevelData.movementRule != RopeMovementRule.OnlyTopOrder)
//             return;

//         foreach (var rope in ropes)
//         {
//             if (!IsRopeIntersecting(rope))
//             {
//                 rope.SetMovable(true);
//                 continue;
//             }

//             int highestOrder = rope.GetOrder();

//             foreach (var other in ropes)
//             {
//                 if (other == rope) continue;

//                 if (LinesIntersect(
//                     rope.nodeA.position,
//                     rope.nodeB.position,
//                     other.nodeA.position,
//                     other.nodeB.position))
//                 {
//                     highestOrder = Mathf.Max(highestOrder, other.GetOrder());
//                 }
//             }

//             rope.SetMovable(rope.GetOrder() == highestOrder);
//         }
//     }

//     // ==================================================
//     // PROMOTE ROPE
//     // ==================================================
//     public void PromoteRope(RopeController_1 rope)
//     {
//         int highest = 0;
//         foreach (var r in ropes)
//             highest = Mathf.Max(highest, r.GetOrder());

//         rope.SetOrder(highest + 1);
//         UpdateTopOrderMovability();
//     }
// }



using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Runtime Ropes")]
    public RopeController_1[] ropes;

    [Header("Moves")]
    public int maxMoves = 10;
    public int currentMoves;

    [Header("Validation")]
    public float nodeBlockRadius = 0.25f;

    [Header("UI Panels")]
    public GameObject winPanel;
    public GameObject losePanel;

    private bool levelEnded = false;
    public bool IsLevelEnding => levelEnded;

    [HideInInspector] public LevelData_1 currentLevelData;

    // ==================================================
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Force clean UI state
        ForceHidePanel(winPanel);
        ForceHidePanel(losePanel);
    }

    // ==================================================
    // RESET (called by LevelManager)
    // ==================================================
    public void ResetState()
    {
        levelEnded = false;
        currentMoves = 0;

        ForceHidePanel(winPanel);
        ForceHidePanel(losePanel);
    }

    // ==================================================
    // MOVE LIMIT
    // ==================================================
    public void SetMoveLimit(int moves)
    {
        maxMoves = moves;
        currentMoves = 0;
    }

    // ==================================================
    // REGISTER MOVE (called from NodeDrag)
    // ==================================================
    public void RegisterMove()
    {
        if (levelEnded) return;

        currentMoves++;

        // ✅ WIN CHECK FIRST
        if (IsLevelSolved())
        {
            WinLevel();
            return;
        }

        // ❌ LOSE CHECK
        if (currentMoves >= maxMoves)
        {
            LoseLevel();
        }
    }

    // ==================================================
    // REGISTER ROPES
    // ==================================================
    public void RegisterRopes(RopeController_1[] levelRopes)
    {
        ropes = levelRopes;

        int order = 0;
        foreach (var rope in ropes)
            rope.SetOrder(++order);

        UpdateTopOrderMovability();
    }

    // ==================================================
    // 🔥 FINAL SOLVE CHECK (CURVE BASED)
    // ==================================================
    bool IsLevelSolved()
    {
        NodeDrag[] nodes =
            Object.FindObjectsByType<NodeDrag>(FindObjectsSortMode.None);

        // 1️⃣ Curve–Curve intersection
        for (int i = 0; i < ropes.Length; i++)
        {
            for (int j = i + 1; j < ropes.Length; j++)
            {
                if (CurvesIntersect(ropes[i], ropes[j]))
                    return false;
            }
        }

        // 2️⃣ Curve–Node intersection
        foreach (var rope in ropes)
        {
            Vector3[] ropePts = rope.GetRopePoints();

            foreach (var node in nodes)
            {
                // Same rope → allowed
                if (node.GetOwningRope() == rope)
                    continue;

                Vector2 nodePos = node.transform.position;

                for (int i = 0; i < ropePts.Length - 1; i++)
                {
                    float dist = DistancePointToSegment(
                        nodePos,
                        ropePts[i],
                        ropePts[i + 1]);

                    if (dist < nodeBlockRadius)
                        return false;
                }
            }
        }

        return true;
    }

    // ==================================================
    // ORDER RULES (USED BY NodeDrag)
    // ==================================================
    public void PromoteRope(RopeController_1 rope)
    {
        int highest = 0;
        foreach (var r in ropes)
            highest = Mathf.Max(highest, r.GetOrder());

        rope.SetOrder(highest + 1);
        UpdateTopOrderMovability();
    }

    public void UpdateTopOrderMovability()
    {
        if (currentLevelData == null) return;
        if (currentLevelData.movementRule != RopeMovementRule.OnlyTopOrder)
            return;

        foreach (var rope in ropes)
        {
            bool intersecting = false;
            int highestOrder = rope.GetOrder();

            foreach (var other in ropes)
            {
                if (other == rope) continue;

                if (CurvesIntersect(rope, other))
                {
                    intersecting = true;
                    highestOrder = Mathf.Max(highestOrder, other.GetOrder());
                }
            }

            rope.SetMovable(!intersecting || rope.GetOrder() == highestOrder);
        }
    }

    // ==================================================
    // WIN / LOSE
    // ==================================================
    void WinLevel()
    {
        if (levelEnded) return;
        levelEnded = true;

        Debug.Log("🔥 WinLevel() CALLED");
        ShowPanel(winPanel);
    }

    void LoseLevel()
    {
        if (levelEnded) return;
        levelEnded = true;

        ShowPanel(losePanel);
    }

    // ==================================================
    // 🔥 UI FORCE VISIBILITY (BULLETPROOF)
    // ==================================================
    void ShowPanel(GameObject panel)
    {
        if (panel == null)
        {
            Debug.LogError("❌ Panel reference is NULL");
            return;
        }

        // Enable full parent chain
        Transform t = panel.transform;
        while (t != null)
        {
            t.gameObject.SetActive(true);
            t = t.parent;
        }

        // Force canvas on top
        Canvas canvas = panel.GetComponentInParent<Canvas>(true);
        if (canvas != null)
        {
            canvas.enabled = true;
            canvas.sortingOrder = 999;
        }

        // Fix CanvasGroup hiding
        CanvasGroup cg = panel.GetComponent<CanvasGroup>();
        if (cg != null)
        {
            cg.alpha = 1f;
            cg.interactable = true;
            cg.blocksRaycasts = true;
        }

        // Fix invisible scale
        RectTransform rt = panel.GetComponent<RectTransform>();
        if (rt != null)
        {
            rt.localScale = Vector3.one;
            rt.anchoredPosition = Vector2.zero;
        }

        panel.SetActive(true);

        Debug.Log($"✅ Showing panel: {panel.name}");
    }

    void ForceHidePanel(GameObject panel)
    {
        if (panel == null) return;
        panel.SetActive(false);
    }

    // ==================================================
    // GEOMETRY HELPERS
    // ==================================================
    bool CurvesIntersect(RopeController_1 r1, RopeController_1 r2)
    {
        Vector3[] p1 = r1.GetRopePoints();
        Vector3[] p2 = r2.GetRopePoints();

        for (int i = 0; i < p1.Length - 1; i++)
        {
            for (int j = 0; j < p2.Length - 1; j++)
            {
                if (LinesIntersect(p1[i], p1[i + 1], p2[j], p2[j + 1]))
                    return true;
            }
        }
        return false;
    }

    bool LinesIntersect(Vector2 A, Vector2 B, Vector2 C, Vector2 D)
    {
        float den = (A.x - B.x) * (C.y - D.y) -
                    (A.y - B.y) * (C.x - D.x);

        if (Mathf.Abs(den) < 0.0001f) return false;

        float t = ((A.x - C.x) * (C.y - D.y) -
                   (A.y - C.y) * (C.x - D.x)) / den;

        float u = -((A.x - B.x) * (A.y - C.y) -
                    (A.y - B.y) * (A.x - C.x)) / den;

        return t > 0f && t < 1f && u > 0f && u < 1f;
    }

    float DistancePointToSegment(Vector2 p, Vector2 a, Vector2 b)
    {
        Vector2 ab = b - a;
        float t = Vector2.Dot(p - a, ab) / ab.sqrMagnitude;
        t = Mathf.Clamp01(t);
        Vector2 closest = a + t * ab;
        return Vector2.Distance(p, closest);
    }
}
