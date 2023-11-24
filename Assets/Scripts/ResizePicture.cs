using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ResizePicture : MonoBehaviour
{
    [SerializeField] private float pinchSpeed = 1f;
    [SerializeField] private float minimumScale = 0.1f;
    [SerializeField] private float maximumScale = 1.0f;

    private float previousDistance = 0f;

    public void OnResizeObject()
    {
        if (!enabled)
        {
            return;
        }

        if (EventSystem.current.IsPointerOverGameObject(0))
        {
            return;
        }

        Touchscreen ts = Touchscreen.current;
        if (ts.touches[0].isInProgress && ts.touches[1].isInProgress)
        {
            Vector2 pos0 = ts.touches[0].position.ReadValue();
            Vector2 pos1 = ts.touches[1].position.ReadValue();
            TouchToResize(pos0, pos1);
        }
        else
        {
            previousDistance = 0f;
        }
    }

    void TouchToResize(Vector2 pos0, Vector2 pos1)
    {
        float distance = Vector2.Distance(pos0, pos1);

        if (previousDistance != 0)
        {
            float scale = transform.localScale.x;
            float scaleFactor = (distance / previousDistance) * pinchSpeed;
            scale *= scaleFactor;
            if (scale < minimumScale)
            {
                scale = minimumScale;
            }

            if (scale > maximumScale)
            {
                scale = maximumScale;
            }

            Vector3 localScale = transform.localScale;
            localScale.x = scale;
            localScale.y = scale;
            transform.localScale = localScale;
        }

        previousDistance = distance;
    }
}
