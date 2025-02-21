using UnityEngine;

public class BlockPlacementController : MonoBehaviour
{
    [SerializeField] GroundPlane _groundPlane;
    [SerializeField] BlockManager _blockManager;
    [SerializeField] BlockPrefabManager _blockPrefabManager;
    [SerializeField] UIManager _uiManager;
    [SerializeField] Material _enableMaterial;
    [SerializeField] Material _disableMaterial;

    bool _isFixPreviewPosition = false;
    private BlockPlacementService _service;

    private void Start()
    {
        _service = new BlockPlacementService(_blockManager, _blockPrefabManager, _groundPlane, _enableMaterial, _disableMaterial);
        _service.Initialize(_uiManager);
    }

    private void Update()
    {
        if (_uiManager.IsPointerOverUI())
            return;

        var hits = RayUtility.RayHitCheck(Input.mousePosition);
        var groundHitData = RayUtility.GetRayHitData(hits, TagManager.GROUND_TAG);

        if (CommonUtility.IsPC && Input.GetMouseButtonDown(0))
        {
            if (!groundHitData.IsHit)
                _isFixPreviewPosition = false;
            else
                _isFixPreviewPosition = !_isFixPreviewPosition;
        }

        if (_isFixPreviewPosition)
            return;

        if (!groundHitData.IsHit)
        {
            _service.HidePreviewBlock();
            return;
        }

        _service.UpdatePreviewPosition(groundHitData.HitPoint);
    }
}
