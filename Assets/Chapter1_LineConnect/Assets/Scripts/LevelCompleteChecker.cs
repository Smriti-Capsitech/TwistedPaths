
//  using UnityEngine;
// using UnityEngine.SceneManagement;
// using System.Collections;
// using System.Collections.Generic;

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
// {
//     Debug.Log("🔎 CheckNow CALLED");

//     if (completed)
//     {
//         Debug.Log("⛔ Already completed");
//         return;
//     }

//     bool modified = player.PlayerModifiedRope();
//     Debug.Log("🧩 PlayerModifiedRope = " + modified);

//     HashSet<Edge> playerEdges =
//         BuildEdgeSet(new List<int>(player.GetSnappedNodes()));

//     HashSet<Edge> targetEdges =
//         BuildEdgeSet(new List<int>(target.pattern));

//     Debug.Log($"📐 PlayerEdges={playerEdges.Count}, TargetEdges={targetEdges.Count}");

//     if (!EdgeSetsEqual(playerEdges, targetEdges))
//     {
//         Debug.Log("❌ Edge mismatch");
//         return;
//     }

//     Debug.Log("✅ EDGE MATCH");

//     if (!modified)
//     {
//         Debug.Log("❌ BLOCKED by PlayerModifiedRope()");
//         return;
//     }

//     Debug.Log("🎉 LEVEL COMPLETE");
//     completed = true;

//     SaveLevelProgress();
//     StartCoroutine(LoadLevelCompleteAfterDelay());

//     if (AdManager.Instance != null)
//         AdManager.Instance.OnLevelComplete();
// }


//     // =========================
//     // 🔐 SAVE UNLOCK PROGRESS
//     // =========================
//     void SaveLevelProgress()
//     {
//         int chapter = PlayerPrefs.GetInt("ACTIVE_CHAPTER", 1);
//         int currentLevel = PlayerPrefs.GetInt("CURRENT_LEVEL", 0);

//         string unlockKey = chapter == 1
//             ? "UNLOCKED_LEVEL"
//             : $"CH{chapter}_UNLOCKED_LEVEL";

//         int unlocked = PlayerPrefs.GetInt(unlockKey, 0);

//         if (currentLevel + 1 > unlocked)
//         {
//             PlayerPrefs.SetInt(unlockKey, currentLevel + 1);
//             PlayerPrefs.Save();
//         }

//         Debug.Log($"🔓 Progress saved → Level {currentLevel + 1}");
//     }

//     // =========================
//     // ⏳ DELAYED LOAD
//     // =========================
//     IEnumerator LoadLevelCompleteAfterDelay()
//     {
//         Debug.Log("⏳ Loading LevelCompleteScene...");

//         if (timer != null)
//             timer.StopTimer();

//         if (AdManager.Instance != null)
//             AdManager.Instance.HideBanner();

//         yield return new WaitForSeconds(0.3f);

//         SceneManager.LoadScene("LevelCompleteScene");
//     }

//     // =========================
//     // EDGE GRAPH BUILDER
//     // =========================
//     HashSet<Edge> BuildEdgeSet(List<int> nodes)
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

//     bool EdgeSetsEqual(HashSet<Edge> a, HashSet<Edge> b)
//     {
//         if (a.Count != b.Count) return false;

//         foreach (var e in a)
//             if (!b.Contains(e))
//                 return false;

//         return true;
//     }

//     public void HideUI()
//     {
//         completed = false;
//     }

//     string EdgeLog(HashSet<Edge> edges)
//     {
//         List<string> list = new List<string>();
//         foreach (var e in edges)
//             list.Add($"{e.a}-{e.b}");
//         return string.Join(" | ", list);
//     }

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
        Debug.Log("🔎 CheckNow CALLED");

        if (completed)
        {
            Debug.Log("⛔ Already completed");
            return;
        }

        List<int> playerNodes = player.GetSnappedNodes();
        List<int> targetNodes = new List<int>(target.pattern);


        HashSet<Edge> playerEdges = BuildEdgeSet(playerNodes);
        HashSet<Edge> targetEdges = BuildEdgeSet(targetNodes);

        Debug.Log("🟦 PLAYER EDGES: " + EdgeLog(playerEdges));
        Debug.Log("🟥 TARGET EDGES: " + EdgeLog(targetEdges));

        if (!EdgeSetsEqual(playerEdges, targetEdges))
        {
            Debug.Log("❌ EDGE MISMATCH");
            return;
        }

        // ✅ DO NOT BLOCK IF ROPE WAS NOT MODIFIED
        Debug.Log("🎉 LEVEL COMPLETE");

        completed = true;

        SaveLevelProgress();
        StartCoroutine(LoadLevelCompleteAfterDelay());

        if (AdManager.Instance != null)
            AdManager.Instance.OnLevelComplete();
    }

    // =========================
    // 🔐 SAVE UNLOCK PROGRESS
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

        Debug.Log($"🔓 Progress saved → Level {currentLevel + 1}");
    }

    // =========================
    // ⏳ DELAYED LOAD
    // =========================
    IEnumerator LoadLevelCompleteAfterDelay()
    {
        Debug.Log("⏳ Loading LevelCompleteScene...");

        if (timer != null)
            timer.StopTimer();

        if (AdManager.Instance != null)
            AdManager.Instance.HideBanner();

        yield return new WaitForSeconds(0.3f);

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
        return string.Join(" | ", list);
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
