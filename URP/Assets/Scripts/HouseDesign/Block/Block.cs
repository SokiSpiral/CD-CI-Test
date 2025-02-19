using UnityEngine;

public class Block : MonoBehaviour
{
    public Transform ColliderTransform => _collider.transform;

    protected Collider _collider;
    protected Renderer _renderer;

    public void Initialize()
    {
        _renderer = GetComponentInChildren<Renderer>();
        _collider = GetComponentInChildren<Collider>();
        _collider.tag = TagManager.BLOCK_TAG;
    }
}
