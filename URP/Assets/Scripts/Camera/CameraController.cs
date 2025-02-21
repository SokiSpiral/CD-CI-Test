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
    private const float TOUCH_ROTATE_THRESHOLD = 10f; // 回転のしきい値
    private const float MOUSE_ZOOM_THRESHOLD = 50f; // 回転のしきい値

    void Update()
    {
        if (CommonUtility.IsPC)
            MouseInputCheck();
        else
            TouchInputCheck();
    }

    // --------------------------
    //  PC向けの操作 (マウス)
    // --------------------------

    void MouseInputCheck()
    {
        MouseZoomCheck();
        MouseRotateCheck();
    }


    void MouseZoomCheck()
    {
        if (_uiManager.IsPointerOverUI())
            return;

        float scroll = Input.mouseScrollDelta.y;
        if (scroll != 0f)
        {
            ZoomCamera(scroll * ZOOM_SPEED * MOUSE_ZOOM_THRESHOLD);
        }
    }

    void MouseRotateCheck()
    {
        if (_uiManager.IsPointerOverUI())
            return;

        if (Input.GetMouseButtonDown(0) && !IsMouseTouchGround())
        {
            _lastTouchPosition = Input.mousePosition;
            _isRotating = true;
        }
        else if (Input.GetMouseButton(0) && _isRotating)
        {
            Vector2 delta = (Vector2)Input.mousePosition - _lastTouchPosition;
            RotateCamera(delta);
            _lastTouchPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _lastTouchPosition = Vector2.zero;
            _isRotating = false;
        }
    }

    bool IsMouseTouchGround()
    {
        var hits = RayUtility.RayHitCheck(Input.mousePosition);
        var groundHitData = RayUtility.GetRayHitData(hits, TagManager.GROUND_TAG);
        return groundHitData.IsHit;
    }


    // --------------------------
    // スマホ向けの操作 (タッチ)
    // --------------------------

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

                if (!_isRotating && Mathf.Abs(touchDiff.x) > TOUCH_ROTATE_THRESHOLD)
                {
                    _isRotating = true;
                }

                if (_isRotating)
                {
                    Vector2 delta = touch.position - _lastTouchPosition;
                    RotateCamera(delta);
                    _lastTouchPosition = touch.position;
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

    void ZoomCamera(float scroll)
    {
        transform.position += transform.forward * scroll * ZOOM_SPEED * Time.deltaTime;
    }

    void RotateCamera(Vector2 delta)
    {
        float rotationX = -delta.y * ROTATE_SPEED * Time.deltaTime;
        float rotationY = delta.x * ROTATE_SPEED * Time.deltaTime;

        var currentEularAngles = transform.eulerAngles;

        float yaw = currentEularAngles.y + rotationY;
        float pitch = currentEularAngles.x + rotationX;
        pitch = ClampAngle(pitch, -80, 80);

        transform.rotation = Quaternion.Euler(pitch, yaw, 0);
    }
}
