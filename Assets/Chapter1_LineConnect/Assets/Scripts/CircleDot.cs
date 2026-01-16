
// using UnityEngine;
// using UnityEngine.EventSystems;

// public class CircleDot : MonoBehaviour, IPointerDownHandler
// {
//     public int index;
//     public bool isOccupied;

//     public void OnPointerDown(PointerEventData eventData)
//     {
//         Debug.Log("DOT CLICKED: " + index);
//         CircularLineController.Instance.OnDotClicked(this);
//     }
using UnityEngine;

public class CircleDot : MonoBehaviour
{
    public int index;        // 🔥 REQUIRED
    public bool isOccupied;  // used by rope logic
}
