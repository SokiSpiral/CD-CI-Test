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
    [SerializeField] private Button _saveButton;
    [SerializeField] private Button _loadButton;
    [SerializeField] private SelectBlockUI _selectBlockUI;
    public event UnityAction OnPlaceBlock;
    public event UnityAction OnSaveGridData;
    public event UnityAction OnLoadGridData;

    private void Start()
    {
        _placeBlockButton.onClick.AddListener(InvokePlaceBlock);

        _saveButton.gameObject.SetActive(true);
        _saveButton.onClick.AddListener(InvokeSaveGridData);

        _loadButton.gameObject.SetActive(GridSaveDataManager.IsSaveFileExists());
        _loadButton.onClick.AddListener(InvokeLoadGridData);
    }

    void InvokePlaceBlock()
    {
        OnPlaceBlock?.Invoke();
    }

    void InvokeSaveGridData()
    {
        OnSaveGridData?.Invoke();
    }

    void InvokeLoadGridData() 
    { 
        OnLoadGridData?.Invoke();
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

    public void EnableLoadButton()
    {
        _loadButton.gameObject.SetActive(true);
    }
}