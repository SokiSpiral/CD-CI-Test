using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class RayUtility
{
    public static RaycastHit[] RayHitCheck(Vector3 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);
        return Physics.RaycastAll(ray);
    }

    public static RayHitData GetRayHitData(RaycastHit[] hitArray, string tag)
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