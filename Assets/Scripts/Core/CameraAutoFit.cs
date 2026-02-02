using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraAutoFit : MonoBehaviour
{
    public SpriteRenderer backgroundSprite;

    void Start()
    {
        FitCamera();
    }

    void FitCamera()
    {
        Camera cam = GetComponent<Camera>();

        float screenRatio = (float)Screen.width / Screen.height;
        float targetRatio = backgroundSprite.bounds.size.x /
                             backgroundSprite.bounds.size.y;

        if (screenRatio >= targetRatio)
        {
            cam.orthographicSize =
                backgroundSprite.bounds.size.y / 2f;
        }
        else
        {
            float diff = targetRatio / screenRatio;
            cam.orthographicSize =
                backgroundSprite.bounds.size.y / 2f * diff;
        }
    }
}
