using UnityEngine;

public class GroundPlane : MonoBehaviour
{
    [SerializeField] private int width = 10;
    [SerializeField] private int height = 10;
    private const float SCALE_FACTOR = 0.1f; // 1 パラメータごとのスケール

    private GridData _gridData;

    public GridData GridData
    {
        get => _gridData;
        set
        {
            _gridData = value;
            if (_gridData != null)
            {
                width = _gridData.Width;
                height = _gridData.Height;
            }
        }
    }

    public int Width
    {
        get => width;
        set
        {
            width = value;
            UpdateScale();
        }
    }

    public int Height
    {
        get => height;
        set
        {
            height = value;
            UpdateScale();
        }
    }

    private void Start()
    {
        UpdateScale();
    }

    public void UpdateScale()
    {
        float scaleX = width * SCALE_FACTOR;
        float scaleZ = height * SCALE_FACTOR;

        transform.localScale = new Vector3(scaleX, 1f, scaleZ);
        GetComponent<GridRenderer>().DrawGrid();
    }
}