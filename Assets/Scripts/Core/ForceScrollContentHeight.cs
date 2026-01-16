using UnityEngine;
using UnityEngine.UI;

public class ForceScrollContentHeight : MonoBehaviour
{
    public GridLayoutGroup grid;
    public int totalItems = 30;

    void OnEnable()
    {
        Canvas.ForceUpdateCanvases();
        Resize();
    }

    public void Resize()
    {
        RectTransform content = GetComponent<RectTransform>();

        // 👇 YOU SAID CONSTRAINT = 4 → we respect that
        int columns = grid.constraintCount;

        int rows = Mathf.CeilToInt((float)totalItems / columns);

        float height =
            rows * grid.cellSize.y +
            (rows - 1) * grid.spacing.y +
            grid.padding.top +
            grid.padding.bottom;

        content.sizeDelta = new Vector2(content.sizeDelta.x, height);
    }
}
