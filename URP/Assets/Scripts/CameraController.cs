using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    [SerializeField] UIManager _uiManager;

    private float minZoom = 40f, maxZoom = 120f;
    private float zoomSpeed = 5.0f;
    Camera _cam;
    Vector2 _lastMovePosition;
    private Vector2 _lastTouchPosition;

    const float MOVE_SPEED = 1.0f;
    const float ORBIT_SPEED = 0.2f;

    void Start()
    {
        _cam = Camera.main;
    }

    void Update()
    {
        if (Application.isEditor)
        {
            MouseRotateCheck();
            MouseMoveCheck();
            MouseZoomCheck();
        }
        else
        {
            TouchMoveCheck();
        }
    }

    // --------------------------
    //  PC向けの操作 (マウス)
    // --------------------------

    void MouseMoveCheck()
    {
        var mousePosition = Input.mousePosition;
        if (Input.GetMouseButtonDown(1))
        {
            var hits = RayUtility.RayHitCheck(mousePosition);
            var groundHitData = RayUtility.GetRayHitData(hits, TagManager.GROUND_TAG);
            if (groundHitData.IsHit)
                return;

            _lastMovePosition = mousePosition;
        }
        else if (Input.GetMouseButton(1))
        {
            if (_uiManager.IsPointerOverUI())
                return;

            Vector2 touchDiff = (Vector2)mousePosition - _lastMovePosition;
            Vector3 move = new Vector3(-touchDiff.x * MOVE_SPEED * Time.deltaTime, 0, -touchDiff.y * MOVE_SPEED * Time.deltaTime);
            transform.Translate(move, Space.World);
            _lastMovePosition = mousePosition;
        }
    }

    void MouseRotateCheck()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _lastTouchPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            if (_uiManager.IsPointerOverUI())
                return;
            RotateCamera();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _lastTouchPosition = Vector2.zero;
        }
    }

    void RotateCamera()
    {
        Vector2 delta = (Vector2)Input.mousePosition - _lastTouchPosition;
        float rotationX = -delta.y * ORBIT_SPEED;
        float rotationY = delta.x * ORBIT_SPEED;

        // 回転制限を追加 (カメラの上下角度を制御)
        Quaternion currentRotation = transform.rotation;
        Quaternion yawRotation = Quaternion.Euler(0f, rotationY, 0f); // 左右回転
        Quaternion pitchRotation = Quaternion.Euler(rotationX, 0f, 0f); // 上下回転

        transform.rotation = yawRotation * currentRotation * pitchRotation;

        _lastTouchPosition = Input.mousePosition;
    }

    void MouseZoomCheck()
    {
        float scroll = Input.mouseScrollDelta.y;
        if (scroll != 0f)
        {
            if (_uiManager.IsPointerOverUI())
                return;

            ZoomCamera(scroll * zoomSpeed * -1);
        }
    }

    void ZoomCamera(float increment)
    {
        float newFOV = Mathf.Clamp(_cam.fieldOfView + increment, minZoom, maxZoom);
        _cam.fieldOfView = newFOV;
    }

    // --------------------------
    // スマホ向けの操作 (タッチ)
    // --------------------------

    void TouchMoveCheck()
    {
        if (Input.touchCount != 1)
            return;

        Touch touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Began)
        {
            _lastMovePosition = touch.position;
        }
        else if (touch.phase == TouchPhase.Moved)
        {
            if (_uiManager.IsPointerOverUI())
                return;

            Vector2 touchDiff = touch.position - _lastMovePosition;
            Vector3 move = new Vector3(-touchDiff.x * MOVE_SPEED * Time.deltaTime, 0, -touchDiff.y * MOVE_SPEED * Time.deltaTime);
            transform.Translate(move, Space.World);
            _lastMovePosition = touch.position;
        }
        else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
        {
            _lastMovePosition = Vector2.zero;
        }
    }
}
