

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Runtime Ropes")]
    public RopeController_1[] ropes;

    [Header("Moves")]
    public int maxMoves;
    public int currentMoves;

    private bool levelEnded = false;
    public bool IsLevelEnding => levelEnded;

    [HideInInspector]
    public LevelData_1 currentLevelData;

    // ==================================================
    // UNITY
    // ==================================================
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // ==================================================
    // RESET
    // ==================================================
    public void ResetState()
    {
        levelEnded = false;
        currentMoves = 0;
    }

    // ==================================================
    // MOVE LIMIT
    // ==================================================
    public void SetMoveLimit(int moves)
    {
        maxMoves = moves;
        currentMoves = 0;

        if (MoveCounterUI.Instance != null)
            MoveCounterUI.Instance.UpdateMoves(currentMoves, maxMoves);
    }

    // ==================================================
    // REGISTER MOVE
    // ==================================================
    // public void RegisterMove()
    // {
    //     if (levelEnded) return;

    //     currentMoves++;

    //     if (MoveCounterUI.Instance != null)
    //         MoveCounterUI.Instance.UpdateMoves(currentMoves, maxMoves);

    //     if (currentMoves >= maxMoves)
    //     {
    //         if (IsLevelSolved())
    //             WinLevel();
    //         else
    //             GameOver();
    //     }
    // }
    public void RegisterMove()
{
    if (levelEnded) return;

    currentMoves++;

    if (MoveCounterUI.Instance != null)
        MoveCounterUI.Instance.UpdateMoves(currentMoves, maxMoves);

    // ✅ IMPORTANT: CHECK SOLVED FIRST
    if (IsLevelSolved())
    {
        WinLevel();
        return;
    }

    // ❌ ONLY THEN CHECK GAME OVER
    if (currentMoves >= maxMoves)
    {
        GameOver();
    }
}


    // ==================================================
    // SOLVED CHECK
    // ==================================================
    bool IsLevelSolved()
    {
        foreach (var rope in ropes)
        {
            if (IsRopeIntersecting(rope))
                return false;
        }
        return true;
    }

    // ==================================================
    // ROPE INTERSECTION (BOOLEAN)
    // ==================================================
    public bool IsRopeIntersecting(RopeController_1 rope)
    {
        foreach (var other in ropes)
        {
            if (other == rope) continue;

            if (LinesIntersect(
                rope.nodeA.position,
                rope.nodeB.position,
                other.nodeA.position,
                other.nodeB.position))
            {
                return true;
            }
        }
        return false;
    }

    // ==================================================
    // REGISTER ROPES (LEVEL LOAD)
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
    // EARLY WIN CHECK
    // ==================================================
    public void CheckLevelComplete()
    {
        if (!levelEnded && IsLevelSolved())
            WinLevel();
    }

    // ==================================================
    // WIN / LOSE
    // ==================================================
    void WinLevel()
    {
        if (levelEnded) return;
        levelEnded = true;

        StartCoroutine(ShowWinAfterDelay());
    }

    IEnumerator ShowWinAfterDelay()
    {
        yield return new WaitForSeconds(1f);

        if (LevelCompleteUI_1.Instance != null)
            LevelCompleteUI_1.Instance.Show();
    }

    public void GameOver()
    {
        if (levelEnded) return;
        levelEnded = true;

        StartCoroutine(ShowLoseAfterDelay());
    }

    IEnumerator ShowLoseAfterDelay()
    {
        yield return new WaitForSeconds(1f);

        if (GameOverUI_1.Instance != null)
            GameOverUI_1.Instance.Show();
    }

    // ==================================================
    // LINE INTERSECTION (BOOLEAN)
    // ==================================================
    bool LinesIntersect(Vector2 A, Vector2 B, Vector2 C, Vector2 D)
    {
        float den = (A.x - B.x) * (C.y - D.y) -
                    (A.y - B.y) * (C.x - D.x);

        if (Mathf.Abs(den) < 0.0001f)
            return false;

        float t = ((A.x - C.x) * (C.y - D.y) -
                   (A.y - C.y) * (C.x - D.x)) / den;

        float u = -((A.x - B.x) * (A.y - C.y) -
                    (A.y - B.y) * (A.x - C.x)) / den;

        return t > 0f && t < 1f && u > 0f && u < 1f;
    }

    // ==================================================
    // LINE INTERSECTION (WITH POINT) 🔥 REQUIRED BY ROPE
    // ==================================================
    public bool TryGetIntersection(
        Vector2 A, Vector2 B,
        Vector2 C, Vector2 D,
        out Vector2 intersection)
    {
        intersection = Vector2.zero;

        float den = (A.x - B.x) * (C.y - D.y) -
                    (A.y - B.y) * (C.x - D.x);

        if (Mathf.Abs(den) < 0.0001f)
            return false;

        float t = ((A.x - C.x) * (C.y - D.y) -
                   (A.y - C.y) * (C.x - D.x)) / den;

        float u = -((A.x - B.x) * (A.y - C.y) -
                    (A.y - B.y) * (A.x - C.x)) / den;

        if (t > 0f && t < 1f && u > 0f && u < 1f)
        {
            intersection = A + t * (B - A);
            return true;
        }

        return false;
    }

    // ==================================================
    // 🔥 TOP-ORDER MOVEMENT RULE
    // ==================================================
    public void UpdateTopOrderMovability()
    {
        if (currentLevelData == null) return;
        if (currentLevelData.movementRule != RopeMovementRule.OnlyTopOrder)
            return;

        foreach (var rope in ropes)
        {
            if (!IsRopeIntersecting(rope))
            {
                rope.SetMovable(true);
                continue;
            }

            int highestOrder = rope.GetOrder();

            foreach (var other in ropes)
            {
                if (other == rope) continue;

                if (LinesIntersect(
                    rope.nodeA.position,
                    rope.nodeB.position,
                    other.nodeA.position,
                    other.nodeB.position))
                {
                    highestOrder = Mathf.Max(highestOrder, other.GetOrder());
                }
            }

            rope.SetMovable(rope.GetOrder() == highestOrder);
        }
    }

    // ==================================================
    // PROMOTE ROPE
    // ==================================================
    public void PromoteRope(RopeController_1 rope)
    {
        int highest = 0;
        foreach (var r in ropes)
            highest = Mathf.Max(highest, r.GetOrder());

        rope.SetOrder(highest + 1);
        UpdateTopOrderMovability();
    }
}
