using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine.Events;
using UnityEngine;

public static class GridSaveDataManager
{
    private static string GetFilePath()
    {
        return CommonUtility.IsPC ? Path.Combine(Application.dataPath, "gridData.json")
                                  : Path.Combine(Application.persistentDataPath, "gridData.json");
    }

    public static bool IsSaveFileExists()
    {
        string filePath = GetFilePath();
        return File.Exists(filePath);
    }

    public static void SaveGridData(GroundPlane groundPlane, UnityAction onComplete)
    {
        GridSaveData saveData = new GridSaveData
        {
            Width = groundPlane.Width,
            Height = groundPlane.Height,
            GridDataList = groundPlane.GridDataList
        };

        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(GetFilePath(), json);
        Debug.Log("GridData saved to: " + GetFilePath());

        onComplete?.Invoke();
    }

    public static void LoadGridData(GroundPlane groundPlane, UnityAction destroyBlockAction, UnityAction<Vector3, int> CreateBlockAction)
    {
        string filePath = GetFilePath();
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            GridSaveData saveData = JsonUtility.FromJson<GridSaveData>(json);

            groundPlane.Width = saveData.Width;
            groundPlane.Height = saveData.Height;
            groundPlane.GridDataList = saveData.GridDataList;

            destroyBlockAction?.Invoke();
            foreach (var grid in saveData.GridDataList)
            {
                CreateBlockAction.Invoke(grid.Position, grid.BlockId);
            }
            Debug.Log("GridData loaded from: " + filePath);
        }
        else
        {
            Debug.LogWarning("No saved GridData found at: " + filePath);
        }
    }
}

[Serializable]
public struct GridData
{
    public Vector3 Position;
    public int BlockId;

    public GridData(Vector3 position, int blockId)
    {
        Position = position;
        BlockId = blockId;
    }
}

[Serializable]
public class GridSaveData
{
    public int Width;
    public int Height;
    public List<GridData> GridDataList;
}