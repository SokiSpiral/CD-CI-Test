using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectBlockUI : MonoBehaviour
{
    [SerializeField] private Button _leftButton;
    [SerializeField] private Button _rightButton;
    [SerializeField] private Text _blockNameText;

    [SerializeField] private BlockManager _blockManager;

    private void Start()
    {
        _leftButton.onClick.AddListener(() => ChangeBlockSelection(true));
        _rightButton.onClick.AddListener(() => ChangeBlockSelection(false));

        UpdateBlockUI(); // 初期状態を更新
    }

    void ChangeBlockSelection(bool isLeft)
    {
        _blockManager.ChangeSelectingIndex(isLeft);
        UpdateBlockUI();
    }

    void UpdateBlockUI()
    {
        _blockNameText.text = _blockManager.GetCurrentBlockName();
    }
}
