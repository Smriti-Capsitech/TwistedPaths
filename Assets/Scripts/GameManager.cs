


using UnityEngine;
using System.Collections;

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

        // 🔥 UPDATE MOVE UI
        if (MoveCounterUI.Instance != null)
            MoveCounterUI.Instance.UseMove();

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

        // 🔥 FIX: Delay initial movability check
        StartCoroutine(DelayedInitialMovability());
    }

    IEnumerator DelayedInitialMovability()
    {
        yield return null;   // wait one frame
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
    // ORDER RULES
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
    // BONUS MOVES
    // ==================================================
    public void AddBonusMoves(int amount)
    {
        if (levelEnded) return;

        currentMoves -= amount;
        currentMoves = Mathf.Clamp(currentMoves, 0, maxMoves);

        if (MoveCounterUI.Instance != null)
            MoveCounterUI.Instance.AddExtraMoves(amount);
    }

    // ==================================================
    // WIN / LOSE
    // ==================================================
    void WinLevel()
    {
        if (levelEnded) return;
        levelEnded = true;
        ShowPanel(winPanel);
    }

    void LoseLevel()
    {
        if (levelEnded) return;
        levelEnded = true;
        ShowPanel(losePanel);
    }

    // ==================================================
    // UI FORCE VISIBILITY
    // ==================================================
    void ShowPanel(GameObject panel)
    {
        if (panel == null) return;

        Transform t = panel.transform;
        while (t != null)
        {
            t.gameObject.SetActive(true);
            t = t.parent;
        }

        Canvas canvas = panel.GetComponentInParent<Canvas>(true);
        if (canvas != null)
        {
            canvas.enabled = true;
            canvas.sortingOrder = 999;
        }

        CanvasGroup cg = panel.GetComponent<CanvasGroup>();
        if (cg != null)
        {
            cg.alpha = 1f;
            cg.interactable = true;
            cg.blocksRaycasts = true;
        }

        RectTransform rt = panel.GetComponent<RectTransform>();
        if (rt != null)
        {
            rt.localScale = Vector3.one;
            rt.anchoredPosition = Vector2.zero;
        }

        panel.SetActive(true);
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


