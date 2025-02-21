using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    private List<GameObject> _blockList = new List<GameObject>();

    public void AddBlock(GameObject obj)
    {
        _blockList.Add(obj);
    }

    public void DestroyAllBlock()
    {
        foreach (GameObject obj in _blockList)
        {
            Destroy(obj);
        }
        _blockList.Clear();
    }
}
