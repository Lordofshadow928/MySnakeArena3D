using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSelectionManager : MonoBehaviour
{
    [SerializeField] private SnakeUIPreview snakePreview;
    [SerializeField] private SkinButton[] skinButtons;

    private ShopEquipController shopEquipController;
    private SkinButton currentSelection;
    public SkinButton CurrentSelection => currentSelection;
    private void Awake()
    {
        shopEquipController = FindObjectOfType<ShopEquipController>();
    }
    public void Select(SkinButton selected)
    {
        Debug.Log($"selected = {selected}");
        Debug.Log($"snakePreview = {snakePreview}");
        Debug.Log($"shopEquipController = {shopEquipController}");
        Debug.Log($"SkinData = {selected.SkinData}");
        foreach (var button in skinButtons)
            button.SetSelected(false);

        selected.SetSelected(true);
        currentSelection = selected;
        snakePreview.ShowSkin(selected.SkinData);
        shopEquipController.Refresh();
    }
}
