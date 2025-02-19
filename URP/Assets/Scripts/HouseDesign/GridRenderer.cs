using UnityEngine;

public class GridRenderer : MonoBehaviour
{
    [SerializeField] private Color gridColor = Color.white;
    private BoxCollider blockCollider;
    
    public float GridSpacing { get; private set; }

    public void Setup(Transform boxTransform)
    {
        blockCollider = boxTransform.GetComponent<BoxCollider>();

        Vector3 blockSize = blockCollider.size;
        Vector3 blockScale = boxTransform.transform.lossyScale;
        GridSpacing = Mathf.Max(blockSize.x * blockScale.x, blockSize.z * blockScale.z); // XかZの大きい方をグリッドサイズに

        DrawGrid();
    }

    private void DrawGrid()
    {
        Bounds planeBounds = gameObject.GetComponent<Renderer>().bounds;

        float minX = planeBounds.min.x;
        float maxX = planeBounds.max.x;
        float minZ = planeBounds.min.z;
        float maxZ = planeBounds.max.z;

        GameObject gridParent = new GameObject("GridLines");

        // X軸方向のライン
        for (float x = minX; x <= maxX; x += GridSpacing)
        {
            CreateLine(new Vector3(x, planeBounds.max.y + 0.01f, minZ),
                       new Vector3(x, planeBounds.max.y + 0.01f, maxZ), gridParent);
        }

        // Z軸方向のライン
        for (float z = minZ; z <= maxZ; z += GridSpacing)
        {
            CreateLine(new Vector3(minX, planeBounds.max.y + 0.01f, z),
                       new Vector3(maxX, planeBounds.max.y + 0.01f, z), gridParent);
        }
    }

    private void CreateLine(Vector3 start, Vector3 end, GameObject parent)
    {
        GameObject lineObj = new GameObject("GridLine");
        lineObj.transform.parent = parent.transform;

        LineRenderer lineRenderer = lineObj.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.startColor = gridColor;
        lineRenderer.endColor = gridColor;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }
}
