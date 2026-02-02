using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // ===========================
    [Header("Runtime Ropes")]
    public RopeController_1[] ropes;

    [Header("Moves")]
    public int maxMoves = 10;
    public int currentMoves;

    [Header("Validation")]
    public float nodeBlockRadius = 0.25f;

    [Header("UI Panels (Assign In Inspector)")]
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;

    private bool levelEnded = false;
    public bool IsLevelEnding => levelEnded;

    [HideInInspector] public LevelData_1 currentLevelData;

    // ===========================
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Safety check
        if (winPanel == null || losePanel == null)
            Debug.LogError("Assign WinPanel and LosePanel in GameManager Inspector!");

        ForceHidePanel(winPanel);
        ForceHidePanel(losePanel);
    }

    // ===========================
    public void ResetState()
    {
        levelEnded = false;
        currentMoves = 0;

        ForceHidePanel(winPanel);
        ForceHidePanel(losePanel);
    }

    // ===========================
    public void SetMoveLimit(int moves)
    {
        maxMoves = moves;
        currentMoves = 0;

        if (MoveCounterUI.Instance != null)
            MoveCounterUI.Instance.UpdateMoves(currentMoves, maxMoves);
    }

    // ===========================
    public void RegisterMove()
    {
        if (levelEnded) return;

        currentMoves++;

        if (MoveCounterUI.Instance != null)
            MoveCounterUI.Instance.UpdateMoves(currentMoves, maxMoves);

        if (IsLevelSolved())
        {
            WinLevel();
            return;
        }

        if (currentMoves >= maxMoves)
            LoseLevel();
    }

    // ===========================
    public void RegisterRopes(RopeController_1[] levelRopes)
    {
        ropes = levelRopes;

        int order = 0;
        foreach (var rope in ropes)
            rope.SetOrder(++order);

        UpdateTopOrderMovability();
    }

    // ===========================
// EXTRA MOVES BUTTON
// ===========================
    public void AddExtraMoves(int amount)
    {
        // reduce used moves
        currentMoves -= amount;

        // clamp so it never goes below 0
        currentMoves = Mathf.Clamp(currentMoves, 0, maxMoves);

        if (MoveCounterUI.Instance != null)
            MoveCounterUI.Instance.UpdateMoves(currentMoves, maxMoves);
    }


    // ===========================
    bool IsLevelSolved()
    {
        NodeDrag[] nodes =
            Object.FindObjectsByType<NodeDrag>(FindObjectsSortMode.None);

        for (int i = 0; i < ropes.Length; i++)
            for (int j = i + 1; j < ropes.Length; j++)
                if (CurvesIntersect(ropes[i], ropes[j]))
                    return false;

        foreach (var rope in ropes)
        {
            Vector3[] pts = rope.GetRopePoints();

            foreach (var node in nodes)
            {
                if (node.GetOwningRope() == rope) continue;

                Vector2 nPos = node.transform.position;

                for (int i = 0; i < pts.Length - 1; i++)
                    if (DistancePointToSegment(nPos, pts[i], pts[i + 1]) < nodeBlockRadius)
                        return false;
            }
        }

        return true;
    }

    // ===========================
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

    // ===========================
    void ShowPanel(GameObject panel)
    {
        if (panel == null)
        {
            Debug.LogError("Panel reference is NULL (assign in Inspector)");
            return;
        }

        panel.SetActive(true);
        Debug.Log("Showing panel: " + panel.name);
    }

    void ForceHidePanel(GameObject panel)
    {
        if (panel != null)
            panel.SetActive(false);
    }

    // ===========================
    // ORDER SYSTEM
    // ===========================
    public void PromoteRope(RopeController_1 rope)
    {
        if (ropes == null || ropes.Length == 0)
            return;

        int highest = 0;
        foreach (var r in ropes)
            highest = Mathf.Max(highest, r.GetOrder());

        rope.SetOrder(highest + 1);
        UpdateTopOrderMovability();
    }

    public void UpdateTopOrderMovability()
    {
        if (currentLevelData == null)
            return;

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

    // ===========================
    // GEOMETRY
    // ===========================
    bool CurvesIntersect(RopeController_1 r1, RopeController_1 r2)
    {
        Vector3[] p1 = r1.GetRopePoints();
        Vector3[] p2 = r2.GetRopePoints();

        for (int i = 0; i < p1.Length - 1; i++)
            for (int j = 0; j < p2.Length - 1; j++)
                if (LinesIntersect(p1[i], p1[i + 1], p2[j], p2[j + 1]))
                    return true;

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
        return Vector2.Distance(a + t * ab, p);
    }
}
