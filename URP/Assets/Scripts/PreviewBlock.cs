using UnityEngine;

public class PreviewBlock : MonoBehaviour
{
    public Transform ColliderTransform => _collider.transform;

    Collider _collider;
    Renderer _renderer;

    Material _enableMaterial;
    Material _disableMaterial;

    bool _isEnable;

    public void Setup(Material enableMaterial, Material disableMaterial)
    {
        _renderer = GetComponentInChildren<Renderer>();
        _collider = GetComponentInChildren<Collider>();
        _collider.tag = TagManager.PREVIEW_TAG;

        _enableMaterial = enableMaterial;
        _disableMaterial = disableMaterial;
    }

    public void Move(Vector3 position)
    {
        transform.position = position;

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
            if (collider.CompareTag("Block"))
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
}
