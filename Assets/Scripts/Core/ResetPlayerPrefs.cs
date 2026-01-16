using UnityEngine;

public class ResetPlayerPrefs : MonoBehaviour
{
    void Start()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("✅ PlayerPrefs cleared");
    }
}
