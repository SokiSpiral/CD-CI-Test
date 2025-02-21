using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(GraphicRaycaster))]
public class UIManager : MonoBehaviour
{
    [SerializeField] private GraphicRaycaster _graphicRaycaster;
    [SerializeField] private Button _placeBlockButton;
    [SerializeField] private SelectBlockUI _selectBlockUI;
    public event UnityAction OnPlaceBlock;

    private void Start()
    {
        _placeBlockButton.onClick.AddListener(InvokePlaceBlock);
    }

    void InvokePlaceBlock()
    {
        OnPlaceBlock?.Invoke();
    }

    public bool IsPointerOverUI()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };
        List<RaycastResult> results = new List<RaycastResult>();
        _graphicRaycaster.Raycast(eventData, results);
        return results.Count > 0;
    }
}