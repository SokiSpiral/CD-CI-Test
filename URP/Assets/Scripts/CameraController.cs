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
    }

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

            var touchDiff =  new Vector2(mousePosition.x, mousePosition.y) - _lastMovePosition;
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
        Vector2 delta = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - _lastTouchPosition;
        float rotationX = -delta.y * ORBIT_SPEED; // 上下の動きでカメラのピッチを変える
        float rotationY = delta.x * ORBIT_SPEED;  // 左右の動きでカメラのヨーを変える

        transform.Rotate(Vector3.up, rotationY, Space.World);
        transform.Rotate(Vector3.right, rotationX, Space.Self);

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
}
