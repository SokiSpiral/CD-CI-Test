using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

[RequireComponent(typeof(Collider))]
public class BlockPlacement : MonoBehaviour
{
    [SerializeField] BlockManager _blockManager;
    [SerializeField] GridRenderer _gridRenderer;
    [SerializeField] UIManager _uiManager;
    [SerializeField] Material _enableMaterial;
    [SerializeField] Material _disableMaterial;

    bool _isPreviewFixed = false;
    Dictionary<int, PreviewBlock> _previewBlocks = new Dictionary<int, PreviewBlock>();
    int _currentBlockIndex;
    int _prevBlockIndex;

    private void Start()
    {
        if (_blockManager == null)
        {
            Debug.LogError("BlockManager is not assigned!");
            return;
        }

        CreatePreviewBlock(0, _blockManager.GetCurrentBlock());
        GetComponent<GridRenderer>().Setup(_previewBlocks[0].ColliderTransform);
        _currentBlockIndex = 0;
        _prevBlockIndex = -1;

        _uiManager.OnPlaceBlock += PlaceBlock;
        _blockManager.OnChangeSelectingBlock += ChangePreviewBlock;
    }

    private void Update()
    {
        if (_uiManager.IsPointerOverUI())
            return;

        if (Application.isEditor && Input.GetMouseButtonDown(0))
            _isPreviewFixed = !_isPreviewFixed;
        if (_isPreviewFixed)
            return;

        var hits = RayUtility.RayHitCheck(Input.mousePosition);
        var groundHitData = RayUtility.GetRayHitData(hits, TagManager.GROUND_TAG);
        if (!groundHitData.IsHit)
        {
            _previewBlocks[_currentBlockIndex].Hide();
            return;
        }

        _previewBlocks[_currentBlockIndex].Move(groundHitData.HitPoint);
    }

    void PlaceBlock()
    {
        if (!_previewBlocks[_currentBlockIndex].gameObject.activeSelf || !_previewBlocks[_currentBlockIndex].IsEnable())
            return;

        _previewBlocks[_currentBlockIndex].Hide();
        var selectedBlockPrefab = _blockManager.GetCurrentBlock();
        if (selectedBlockPrefab == null) return;

        var block = Instantiate(selectedBlockPrefab, _previewBlocks[_currentBlockIndex].transform.position, Quaternion.identity).AddComponent<Block>();
        block.Initialize();
        block.ColliderTransform.GetComponent<BoxCollider>().size *= 0.95f;
    }

    void ChangePreviewBlock(int index)
    {
        _prevBlockIndex = _currentBlockIndex;
        _currentBlockIndex = index;

        bool hasPrevBlockActive = _previewBlocks[_prevBlockIndex].gameObject.activeSelf;
        _previewBlocks[_prevBlockIndex].Hide();

        if (!_previewBlocks.ContainsKey(index))
            CreatePreviewBlock(index, _blockManager.GetCurrentBlock());

        if(hasPrevBlockActive)
            _previewBlocks[index].Show(_previewBlocks[_prevBlockIndex].transform.position);
    }

    void CreatePreviewBlock(int index, GameObject blockPrefab)
    {
        var previewBlockObj = Instantiate(blockPrefab);
        var previewBlock = previewBlockObj.AddComponent<PreviewBlock>();
        previewBlock.Setup(_enableMaterial, _disableMaterial, _gridRenderer);
        previewBlock.Hide();

        _previewBlocks.Add(index, previewBlock);
    }
}
