using System.Collections.Generic;
using UnityEngine;

public class GridData : MonoBehaviour
{
    public int Width { get; private set; }
    public int Height { get; private set; }

    Dictionary<Vector2Int, int> _blockDictionary = new Dictionary<Vector2Int, int>();

    public GridData(int width, int height)
    {
        Width = width;
        Height = height;
    }
}
