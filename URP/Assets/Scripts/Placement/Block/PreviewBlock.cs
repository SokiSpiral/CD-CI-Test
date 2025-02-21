using UnityEngine;

public class PreviewBlock : Block
{
    Material _enableMaterial;
    Material _disableMaterial;

    bool _isEnable;

    public void Setup(Material enableMaterial, Material disableMaterial)
    {
        Initialize();
        _enableMaterial = enableMaterial;
        _disableMaterial = disableMaterial;

        _collider.tag = TagManager.PREVIEW_TAG;
    }

    public void Move(Vector3 position)
    {
        Vector3 snappedPosition = SnapToGrid(position);
        transform.position = snappedPosition; // グリッドの中央にスナップ

        var isHit = IsCollidingWithBlock();
        if (isHit)
            Disable();
        else
            Enable();

        Show();
    }

    bool IsCollidingWithBlock()
    {
        Collider[] hitColliders = Physics.OverlapBox(transform.position, transform.localScale / 2);
        foreach (var collider in hitColliders)
        {
            if (collider.CompareTag(TagManager.BLOCK_TAG))
            {
                return true;
            }
        }

        return false;
    }

    void Disable()
    {
        _isEnable = false;
        _renderer.material = _disableMaterial;
    }

    void Enable()
    {
        _isEnable = true;
        _renderer.material = _enableMaterial;
    }

    public bool IsEnable()
    {
        return _isEnable;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Show(Vector3 position)
    {
        Move(position);
        gameObject.SetActive(true);
    }

    Vector3 SnapToGrid(Vector3 position)
    {
        float gridSize = GridRenderer.GRID_SIZE;
        float x = Mathf.Round(position.x / gridSize) * gridSize;
        float y = position.y;
        float z = Mathf.Round(position.z / gridSize) * gridSize;
        return new Vector3(x, y, z);
    }
}
