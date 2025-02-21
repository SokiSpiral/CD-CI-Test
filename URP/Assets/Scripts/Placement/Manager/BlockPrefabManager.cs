using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BlockPrefabManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _prefabList;
    private int _selectIndex = 0;

    public int BlockCount => _prefabList.Count;
    public event UnityAction<int> OnChangeSelectingBlock;

    public GameObject GetCurrentBlock()
    {
        if (_prefabList == null || _prefabList.Count == 0)
        {
            Debug.LogError("BlockList is empty!");
            return null;
        }
        return _prefabList[_selectIndex];
    }

    public GameObject GetBlockPrefab(int index)
    {
        return _prefabList[index];
    }

    public void ChangeSelectingIndex(bool isLeft)
    {
        if (_prefabList == null || _prefabList.Count == 0) return;

        if (isLeft)
            _selectIndex--;
        else
            _selectIndex++;

        if (_selectIndex < 0)
            _selectIndex = _prefabList.Count - 1;
        else if (_selectIndex >= _prefabList.Count)
            _selectIndex = 0;

        OnChangeSelectingBlock?.Invoke(_selectIndex);
    }

    public string GetCurrentBlockName()
    {
        return GetCurrentBlock().name;
    }
}
