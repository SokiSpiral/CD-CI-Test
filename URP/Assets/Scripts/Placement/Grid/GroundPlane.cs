using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GroundPlane : MonoBehaviour
{
    [SerializeField] private int width = 10;
    [SerializeField] private int height = 10;
    public List<GridData> GridDataList { get; set; } = new List<GridData>();

    private const float SCALE_FACTOR = 0.1f; // 1 パラメータごとのスケール

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

    public void AddBlock(Vector3 position, int blockId)
    {
        var newBlock = new GridData(position, blockId);
        GridDataList.Add(newBlock);
    }
}

