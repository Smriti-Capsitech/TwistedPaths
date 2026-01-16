using UnityEngine;
using UnityEngine.EventSystems;

public class DotInput : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public static CircleDot selected;

    public void OnPointerDown(PointerEventData eventData)
    {
        selected = GetComponent<CircleDot>();
        Debug.Log("DOT CLICKED: " + selected.index);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        selected = null;
    }
}
