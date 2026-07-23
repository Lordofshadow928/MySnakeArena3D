using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinButton : MonoBehaviour
{
    [SerializeField] private string skinID;
    [SerializeField] private Image selectedBorder;

    public string SkinID => skinID;
    public SnakeSkinData SkinData { get; private set; }
    private ShopSelectionManager shopSelectionManager;
    private void Awake()
    {
        shopSelectionManager = FindObjectOfType<ShopSelectionManager>();
    }

    public void Initialize(SnakeSkinData skin)
    {
        SkinData = skin;
    }

    public void Click()
    {
        shopSelectionManager.Select(this);
    }

    public void SetSelected(bool selected)
    {
        selectedBorder.enabled = selected;
    }
}
