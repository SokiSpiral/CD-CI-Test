using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BlockPlacement : MonoBehaviour
{
    [SerializeField] GameObject _blockPrefab;
    [SerializeField] GridRenderer _gridRenderer;
    [SerializeField] UIManager _uiManager;
    [SerializeField] Material _enableMaterial;
    [SerializeField] Material _disableMaterial;
    PreviewBlock _previewBlock;
    bool _isPreviewFixed = false;

    private void Start()
    {
        var previewBlockObj = Instantiate(_blockPrefab);
        _previewBlock = previewBlockObj.AddComponent<PreviewBlock>();
        _previewBlock.Setup(_enableMaterial, _disableMaterial, _gridRenderer);

        _previewBlock.Hide();

        GetComponent<GridRenderer>().Setup(_previewBlock.ColliderTransform);
        _uiManager.OnPlaceBlock += PlaceBlock;
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
            _previewBlock.Hide();
            return;
        }

        _previewBlock.Move(groundHitData.HitPoint);
    }

    void PlaceBlock()
    {
        if (!_previewBlock.gameObject.activeSelf || !_previewBlock.IsEnable())
            return;

        _previewBlock.Hide();
        var block = Instantiate(_blockPrefab, _previewBlock.transform.position, Quaternion.identity).AddComponent<Block>();
        block.Initialize();
        block.ColliderTransform.GetComponent<BoxCollider>().size *= 0.95f;
    }
}
