using System.Collections.Generic;
using UnityEngine;

public class BlockPlacementService
{
    private Dictionary<int, PreviewBlock> _previewBlocks = new Dictionary<int, PreviewBlock>();
    private int _currentBlockIndex;
    private int _prevBlockIndex;
    private Material _enableMaterial;
    private Material _disableMaterial;
    private BlockManager _blockManager;

    public BlockPlacementService(BlockManager blockManager, Material enableMaterial, Material disableMaterial)
    {
        _blockManager = blockManager;
        _enableMaterial = enableMaterial;
        _disableMaterial = disableMaterial;
    }

    public void Initialize(UIManager uiManager)
    {
        if (_blockManager == null)
        {
            Debug.LogError("BlockManager is not assigned!");
            return;
        }

        CreatePreviewBlock(0, _blockManager.GetCurrentBlock());
        _currentBlockIndex = 0;
        _prevBlockIndex = -1;

        uiManager.OnPlaceBlock += PlaceBlock;
        _blockManager.OnChangeSelectingBlock += ChangePreviewBlock;
    }

    public void PlaceBlock()
    {
        if (!_previewBlocks[_currentBlockIndex].gameObject.activeSelf || !_previewBlocks[_currentBlockIndex].IsEnable())
            return;

        _previewBlocks[_currentBlockIndex].Hide();
        var selectedBlockPrefab = _blockManager.GetCurrentBlock();
        if (selectedBlockPrefab == null) return;

        var block = Object.Instantiate(selectedBlockPrefab, _previewBlocks[_currentBlockIndex].transform.position, Quaternion.identity).AddComponent<Block>();
        block.Initialize();
        block.ColliderTransform.GetComponent<BoxCollider>().size *= 0.95f;
    }

    public void ChangePreviewBlock(int index)
    {
        _prevBlockIndex = _currentBlockIndex;
        _currentBlockIndex = index;

        bool hasPrevBlockActive = _previewBlocks[_prevBlockIndex].gameObject.activeSelf;
        _previewBlocks[_prevBlockIndex].Hide();

        if (!_previewBlocks.ContainsKey(index))
            CreatePreviewBlock(index, _blockManager.GetCurrentBlock());

        if (hasPrevBlockActive)
            _previewBlocks[index].Show(_previewBlocks[_prevBlockIndex].transform.position);
    }

    private void CreatePreviewBlock(int index, GameObject blockPrefab)
    {
        var previewBlockObj = Object.Instantiate(blockPrefab);
        var previewBlock = previewBlockObj.AddComponent<PreviewBlock>();
        previewBlock.Setup(_enableMaterial, _disableMaterial);
        previewBlock.Hide();

        _previewBlocks.Add(index, previewBlock);
    }

    public void UpdatePreviewPosition(Vector3 hitPoint)
    {
        _previewBlocks[_currentBlockIndex].Move(hitPoint);
    }

    public void HidePreviewBlock()
    {
        _previewBlocks[_currentBlockIndex].Hide();
    }
}
