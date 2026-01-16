
// using UnityEngine;
// using UnityEngine.SceneManagement;
// using System.Collections;                     // ✅ REQUIRED (FIX 1)
// using System.Collections.Generic;             // ✅ REQUIRED

// public class LevelCompleteChecker : MonoBehaviour
// {
//     public CircularLineController player;
//     public TargetPatternRenderer target;
//     public LevelTimer timer;

//     bool completed = false;

//     // =========================
//     // MAIN CHECK (GRAPH BASED)
//     // =========================
//     public void CheckNow()
//     {
//         if (completed) return;

//         Debug.Log("🔍 CHECK NOW");

//         if (!player.PlayerModifiedRope())
//         {
//             Debug.Log("❌ Rope not modified");
//             return;
//         }

//         // ✅ FIX 2: Explicit Generic List
//         HashSet<Edge> playerEdges =
//             BuildEdgeSet(new System.Collections.Generic.List<int>(player.GetSnappedNodes()));

//         HashSet<Edge> targetEdges =
//             BuildEdgeSet(new System.Collections.Generic.List<int>(target.pattern));

//         Debug.Log("🧵 Player Edges: " + EdgeLog(playerEdges));
//         Debug.Log("🎯 Target Edges: " + EdgeLog(targetEdges));

//         if (!EdgeSetsEqual(playerEdges, targetEdges))
//         {
//             Debug.Log("❌ SHAPE MISMATCH");
//             return;
//         }

//         Debug.Log("🏆 LEVEL COMPLETE!");
//         completed = true;

//         StartCoroutine(LoadLevelCompleteAfterDelay());   
//     }

//     // =========================
//     // ⏳ DELAYED LOAD (FIX 1)
//     // =========================
//     IEnumerator LoadLevelCompleteAfterDelay()
//     {
//         if (timer != null)
//             timer.StopTimer();

//         yield return new WaitForSeconds(0.6f);   // ⏱ 2 seconds delay
//         SceneManager.LoadScene("LevelCompleteScene");
//     }

//     // =========================
//     // EDGE GRAPH BUILDER
//     // =========================
//     HashSet<Edge> BuildEdgeSet(System.Collections.Generic.List<int> nodes)
//     {
//         HashSet<Edge> edges = new HashSet<Edge>();

//         for (int i = 0; i < nodes.Count - 1; i++)
//         {
//             int a = nodes[i];
//             int b = nodes[i + 1];

//             if (a == b) continue;
//             edges.Add(new Edge(a, b));
//         }

//         return edges;
//     }

//     // =========================
//     // EDGE COMPARISON
//     // =========================
//     bool EdgeSetsEqual(HashSet<Edge> a, HashSet<Edge> b)
//     {
//         if (a.Count != b.Count) return false;

//         foreach (var e in a)
//             if (!b.Contains(e))
//                 return false;

//         return true;
//     }

//     // =========================
//     // REQUIRED (DO NOT REMOVE)
//     // =========================
//     public void HideUI()
//     {
//         completed = false;
//     }

//     // =========================
//     // DEBUG
//     // =========================
//     string EdgeLog(HashSet<Edge> edges)
//     {
//         List<string> list = new List<string>();
//         foreach (var e in edges)
//             list.Add($"{e.a}-{e.b}");
//         return string.Join(", ", list);
//     }

//     // =========================
//     // EDGE STRUCT (UNDIRECTED)
//     // =========================
//     struct Edge
//     {
//         public int a;
//         public int b;

//         public Edge(int x, int y)
//         {
//             a = Mathf.Min(x, y);
//             b = Mathf.Max(x, y);
//         }

//         public override bool Equals(object obj)
//         {
//             if (!(obj is Edge)) return false;
//             Edge e = (Edge)obj;
//             return a == e.a && b == e.b;
//         }

//         public override int GetHashCode()
//         {
//             return (a * 397) ^ b;
//         }
//     }
// }
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class LevelCompleteChecker : MonoBehaviour
{
    public CircularLineController player;
    public TargetPatternRenderer target;
    public LevelTimer timer;

    bool completed = false;

    // =========================
    // MAIN CHECK (GRAPH BASED)
    // =========================
    public void CheckNow()
    {
        if (completed) return;

        if (!player.PlayerModifiedRope())
            return;

        HashSet<Edge> playerEdges =
            BuildEdgeSet(new List<int>(player.GetSnappedNodes()));

        HashSet<Edge> targetEdges =
            BuildEdgeSet(new List<int>(target.pattern));

        if (!EdgeSetsEqual(playerEdges, targetEdges))
            return;

        completed = true;

        // ✅ SAVE PROGRESS HERE
        SaveLevelProgress();

        StartCoroutine(LoadLevelCompleteAfterDelay());
    }

    // =========================
    // 🔐 SAVE UNLOCK PROGRESS (FIX)
    // =========================
    void SaveLevelProgress()
    {
        int chapter = PlayerPrefs.GetInt("ACTIVE_CHAPTER", 1);
        int currentLevel = PlayerPrefs.GetInt("CURRENT_LEVEL", 0);

        string unlockKey = chapter == 1
            ? "UNLOCKED_LEVEL"
            : $"CH{chapter}_UNLOCKED_LEVEL";

        int unlocked = PlayerPrefs.GetInt(unlockKey, 0);

        if (currentLevel + 1 > unlocked)
        {
            PlayerPrefs.SetInt(unlockKey, currentLevel + 1);
            PlayerPrefs.Save();
        }
    }

    // =========================
    // ⏳ DELAYED LOAD
    // =========================
    IEnumerator LoadLevelCompleteAfterDelay()
    {
        if (timer != null)
            timer.StopTimer();

        yield return new WaitForSeconds(0.6f);
        SceneManager.LoadScene("LevelCompleteScene");
    }

    // =========================
    // EDGE GRAPH BUILDER
    // =========================
    HashSet<Edge> BuildEdgeSet(List<int> nodes)
    {
        HashSet<Edge> edges = new HashSet<Edge>();

        for (int i = 0; i < nodes.Count - 1; i++)
        {
            int a = nodes[i];
            int b = nodes[i + 1];
            if (a == b) continue;
            edges.Add(new Edge(a, b));
        }

        return edges;
    }

    bool EdgeSetsEqual(HashSet<Edge> a, HashSet<Edge> b)
    {
        if (a.Count != b.Count) return false;
        foreach (var e in a)
            if (!b.Contains(e))
                return false;
        return true;
    }

    public void HideUI()
    {
        completed = false;
    }

    string EdgeLog(HashSet<Edge> edges)
    {
        List<string> list = new List<string>();
        foreach (var e in edges)
            list.Add($"{e.a}-{e.b}");
        return string.Join(", ", list);
    }

    struct Edge
    {
        public int a;
        public int b;

        public Edge(int x, int y)
        {
            a = Mathf.Min(x, y);
            b = Mathf.Max(x, y);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Edge)) return false;
            Edge e = (Edge)obj;
            return a == e.a && b == e.b;
        }

        public override int GetHashCode()
        {
            return (a * 397) ^ b;
        }
    }
}
