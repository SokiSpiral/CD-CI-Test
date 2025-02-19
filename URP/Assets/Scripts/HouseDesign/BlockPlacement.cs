using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BlockPlacement : MonoBehaviour
{
    [SerializeField] GameObject _blockPrefab;
    [SerializeField] GridRenderer _gridRenderer;
    [SerializeField] Material _enableMaterial;
    [SerializeField] Material _disableMaterial;
    PreviewBlock _previewBlock;

    private void Start()
    {
        var previewBlockObj = Instantiate(_blockPrefab);
        _previewBlock = previewBlockObj.AddComponent<PreviewBlock>();
        _previewBlock.Setup(_enableMaterial, _disableMaterial, _gridRenderer);

        _previewBlock.Hide();

        GetComponent<GridRenderer>().Setup(_previewBlock.ColliderTransform);
    }

    private void Update()
    {
        var hits = RayHitCheck(Input.mousePosition);
        var groundHitData = GetRayHitData(hits, TagManager.GROUND_TAG);
        if (!groundHitData.IsHit)
        {
            _previewBlock.Hide();
            return;
        }

        _previewBlock.Move(groundHitData.HitPoint);

        if (Input.GetMouseButtonDown(0))
            CreateBlock(groundHitData.HitPoint);
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

    void CreateBlock(Vector3 point)
    {
        if (!_previewBlock.IsEnable())
            return;

        _previewBlock.Hide();
        Vector3 snappedPosition = SnapToGrid(point);

        var block = Instantiate(_blockPrefab, snappedPosition, Quaternion.identity).AddComponent<Block>();
        block.Initialize();
        block.ColliderTransform.GetComponent<BoxCollider>().size *= 0.95f;
    }

    Vector3 SnapToGrid(Vector3 position)
    {
        float gridSize = _gridRenderer.GridSpacing;
        float x = Mathf.Round(position.x / gridSize) * gridSize + (gridSize / 2);
        float y = position.y;
        float z = Mathf.Round(position.z / gridSize) * gridSize + (gridSize / 2);
        return new Vector3(x, y, z);
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
