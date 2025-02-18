using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BlockPlacement : MonoBehaviour
{
    [SerializeField] GameObject blockPrefab;
    [SerializeField] Material enableMaterial;
    [SerializeField] Material disableMaterial;
    PreviewBlock previewBlock;

    private void Start()
    {
        var previewBlockObj = Instantiate(blockPrefab);
        previewBlock = previewBlockObj.AddComponent<PreviewBlock>();
        previewBlock.Setup(enableMaterial, disableMaterial);

        previewBlock.Hide();

        GetComponent<GridRenderer>().Setup(previewBlock.ColliderTransform);
    }

    private void Update()
    {
        var hits = RayHitCheck(Input.mousePosition);
        var groundHitData = GetRayHitData(hits, TagManager.GROUND_TAG);
        if (!groundHitData.IsHit)
        {
            previewBlock.Hide();
            return;
        }

        previewBlock.Move(groundHitData.HitPoint);

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
        if (!previewBlock.IsEnable())
            return;

        previewBlock.Hide();
        Instantiate(blockPrefab, point, Quaternion.identity);
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
