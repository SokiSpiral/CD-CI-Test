using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BlockPlacementService
{
    private Dictionary<int, PreviewBlock> _previewBlocks = new Dictionary<int, PreviewBlock>();
    private int _currentBlockIndex;
    private int _prevBlockIndex;
    private Material _enableMaterial;
    private Material _disableMaterial;
    private BlockManager _blockManager;
    private BlockPrefabManager _blockPrefabManager;
    private GroundPlane _groundPlane;

    public BlockPlacementService(BlockManager blockManager, BlockPrefabManager blockPrefabManager, GroundPlane groundPlane, Material enableMaterial, Material disableMaterial)
    {
        _blockManager = blockManager;
        _blockPrefabManager = blockPrefabManager;
        _enableMaterial = enableMaterial;
        _disableMaterial = disableMaterial;
        _groundPlane = groundPlane;
    }

    public void Initialize(UIManager uiManager)
    {
        if (_blockPrefabManager == null)
        {
            Debug.LogError("BlockManager is not assigned!");
            return;
        }

        CreatePreviewBlock(0, _blockPrefabManager.GetCurrentBlock());
        _currentBlockIndex = 0;
        _prevBlockIndex = -1;

        uiManager.OnPlaceBlock += PlaceBlock;
        uiManager.OnSaveGridData += () => GridSaveDataManager.SaveGridData(_groundPlane, uiManager.EnableLoadButton);
        uiManager.OnLoadGridData += () => GridSaveDataManager.LoadGridData(_groundPlane, _blockManager.DestroyAllBlock, PlaceBlock);

        _blockPrefabManager.OnChangeSelectingBlock += ChangePreviewBlock;
    }

    private void PlaceBlock()
    {
        if (!_previewBlocks[_currentBlockIndex].gameObject.activeSelf || !_previewBlocks[_currentBlockIndex].IsEnable())
            return;

        _previewBlocks[_currentBlockIndex].Hide();
        var selectedBlockPrefab = _blockPrefabManager.GetCurrentBlock();
        if (selectedBlockPrefab == null) return;

        var block = CreateBlock(selectedBlockPrefab, _previewBlocks[_currentBlockIndex].transform.position);
        _groundPlane.AddBlock(block.transform.position, _currentBlockIndex);
    }

    public void PlaceBlock(Vector3 position, int blockId)
    {
        GameObject blockPrefab = _blockPrefabManager.GetBlockPrefab(blockId);
        if (blockPrefab == null)
        {
            Debug.LogWarning("Block prefab not found for ID: " + blockId);
            return;
        }

        CreateBlock(blockPrefab, position);
    }

    Block CreateBlock(GameObject blockPrefab, Vector3 position)
    {
        var block = Object.Instantiate(blockPrefab, position, Quaternion.identity).AddComponent<Block>();
        _blockManager.AddBlock(block.gameObject);
        block.Initialize();
        block.ColliderTransform.GetComponent<BoxCollider>().size *= 0.95f;
        return block;
    }

    private void ChangePreviewBlock(int index)
    {
        _prevBlockIndex = _currentBlockIndex;
        _currentBlockIndex = index;

        bool hasPrevBlockActive = _previewBlocks[_prevBlockIndex].gameObject.activeSelf;
        _previewBlocks[_prevBlockIndex].Hide();

        if (!_previewBlocks.ContainsKey(index))
            CreatePreviewBlock(index, _blockPrefabManager.GetCurrentBlock());

        if (hasPrevBlockActive)
        {
            _previewBlocks[index].Show(_previewBlocks[_prevBlockIndex].transform.position);
        }
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
        if (_previewBlocks.Count == 0)
            return;

        _previewBlocks[_currentBlockIndex].Hide();
    }
}
