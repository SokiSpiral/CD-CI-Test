using UnityEngine;

public class Block : MonoBehaviour
{
    public Transform ColliderTransform
    {
        get
        {
            if (_collider == null)
                return null;

            return _collider.transform;
        }
    }

    protected BoxCollider _collider;
    protected Renderer _renderer;

    public void Initialize()
    {
        _renderer = GetComponentInChildren<Renderer>();
        _collider = GetComponentInChildren<BoxCollider>();
        _collider.tag = TagManager.BLOCK_TAG;

        if(_collider == null)
        {
            Debug.Log("Invalid Block Found! : Not Assigned BoxCollider");
        }
    }
}
