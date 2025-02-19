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
        if (Application.isEditor && Input.GetMouseButtonDown(0))
        {
            _isPreviewFixed = !_isPreviewFixed; // クリックで固定・解除を切り替え
        }
        if (_isPreviewFixed)
            return;

        var hits = RayHitCheck(Input.mousePosition);
        var groundHitData = GetRayHitData(hits, TagManager.GROUND_TAG);
        if (!groundHitData.IsHit)
        {
            _previewBlock.Hide();
            return;
        }

        _previewBlock.Move(groundHitData.HitPoint);
    }

    RaycastHit[] RayHitCheck(Vector3 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);
        return Physics.RaycastAll(ray);
    }

    public RayHitData GetRayHitData(RaycastHit[] hitArray, string tag)
    {
        foreach (var hit in hitArray)
        {
            if (hit.collider.CompareTag(tag)) // 引数のタグと比較
            {
                return new RayHitData(hit.transform, hit.point);
            }
        }
        return new RayHitData();
    }

    void PlaceBlock()
    {
        if (!_previewBlock.IsEnable())
            return;

        _previewBlock.Hide();
        var block = Instantiate(_blockPrefab, _previewBlock.transform.position, Quaternion.identity).AddComponent<Block>();
        block.Initialize();
        block.ColliderTransform.GetComponent<BoxCollider>().size *= 0.95f;
    }
}

public readonly struct RayHitData
{
    public readonly bool IsHit => HitTransform != null;
    public readonly Vector3 HitPoint;
    public readonly Transform HitTransform;

    // コンストラクタ
    public RayHitData(Transform hitTransform, Vector3 hitPoint)
    {
        HitTransform = hitTransform;
        HitPoint = hitPoint;
    }
}
