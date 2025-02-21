using UnityEngine;

public class BlockPlacement : MonoBehaviour
{
    [SerializeField] GroundPlane _groundPlane;
    [SerializeField] BlockManager _blockManager;
    [SerializeField] UIManager _uiManager;
    [SerializeField] Material _enableMaterial;
    [SerializeField] Material _disableMaterial;

    bool _isPreviewFixed = false;
    private BlockPlacementService _placementService;

    private void Start()
    {
        _placementService = new BlockPlacementService(_blockManager, _enableMaterial, _disableMaterial);
        _placementService.Initialize(_uiManager);
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
            _placementService.HidePreviewBlock();
            return;
        }

        _placementService.UpdatePreviewPosition(groundHitData.HitPoint);
    }
}
