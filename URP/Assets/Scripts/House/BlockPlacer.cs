using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider))]
public class BlockPlacer : MonoBehaviour
{
    [SerializeField] GameObject blockPrefab;
    void Start()
    {
        Debug.Log("Start");
        //UniRx�̃C�x���g�g���K�[�ǉ�
        var eventTrigger = gameObject.AddComponent<ObservablePointerDownTrigger>();

        eventTrigger.OnPointerDownAsObservable().Subscribe(CreatingBlockProcess).AddTo(this);
    }

    void CreatingBlockProcess(PointerEventData pointerEventData)
    {
        Debug.Log("CreatingBlockProcess");
        HitCheck(pointerEventData.position, CreateBlock);
    }

    void HitCheck(Vector3 position, UnityAction<Vector3> action)
    {
        Debug.Log("HitCheck");
        var ray = Camera.main.ScreenPointToRay(position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
            action?.Invoke(hit.point);
    }

    void CreateBlock(Vector3 point)
    {
        Debug.Log("CreateBlock");
        Instantiate(blockPrefab, point, Quaternion.identity);
    }
}
