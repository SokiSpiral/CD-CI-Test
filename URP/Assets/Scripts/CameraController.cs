using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] UIManager _uiManager;
    private Vector2 _lastTouchPosition;
    private Vector2 _startTouchPosition;
    private bool _isRotating = false;

    private Vector2 _lastTouchDistance;

    private const float ROTATE_SPEED = 2f;
    private const float ZOOM_SPEED = 1f;
    private const float MOVE_THRESHOLD = 10f; // 回転のしきい値

    void Update()
    {
        TouchInputCheck();
    }

    void TouchInputCheck()
    {
        if (Input.touchCount == 1) // 1本指: 回転
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                _startTouchPosition = touch.position;
                _lastTouchPosition = touch.position;
                _isRotating = false;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                if (_uiManager.IsPointerOverUI())
                    return;

                Vector2 touchDiff = touch.position - _startTouchPosition;

                if (!_isRotating && Mathf.Abs(touchDiff.x) > MOVE_THRESHOLD)
                {
                    _isRotating = true;
                }

                if (_isRotating)
                {
                    RotateCameraTouch(touch);
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                _isRotating = false;
            }
        }
        else if (Input.touchCount == 2) // 2本指: ズーム
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            if (touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began)
            {
                _lastTouchDistance = touch2.position - touch1.position;
            }
            else if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
            {
                if (_uiManager.IsPointerOverUI())
                    return;

                ZoomCamera(touch1, touch2);
            }
        }
    }

    void RotateCameraTouch(Touch touch)
    {
        Vector2 delta = touch.position - _lastTouchPosition;
        float rotationX = -delta.y * ROTATE_SPEED * Time.deltaTime;
        float rotationY = delta.x * ROTATE_SPEED * Time.deltaTime;

        var currentEularAngles = transform.eulerAngles;

        float yaw = currentEularAngles.y + rotationY;
        float pitch = currentEularAngles.x + rotationX;
        pitch = ClampAngle(pitch, -80, 80);

        transform.rotation = Quaternion.Euler(pitch, yaw, 0);

        _lastTouchPosition = touch.position;
    }

    float ClampAngle(float angle, float min, float max)
    {
        if (angle > 180.0f)
            angle -= 360.0f;

        return Mathf.Clamp(angle, min, max);
    }

    void ZoomCamera(Touch touch1, Touch touch2)
    {
        Vector2 currentTouchDistance = touch2.position - touch1.position;
        float deltaMagnitude = _lastTouchDistance.magnitude - currentTouchDistance.magnitude;

        transform.position += transform.forward * deltaMagnitude * ZOOM_SPEED * Time.deltaTime;
        _lastTouchDistance = currentTouchDistance;
    }
}
