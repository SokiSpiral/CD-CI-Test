using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Button _placeBlockButton;
    public event UnityAction OnPlaceBlock;

    private void Start()
    {
        _placeBlockButton.onClick.AddListener(() => OnPlaceBlock?.Invoke());
    }
}