using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSelectionManager : MonoBehaviour
{
    [SerializeField] private SnakeUIPreview snakePreview;
    [SerializeField] private SkinButton[] skinButtons;

    private SkinButton currentSelection;
    public SkinButton CurrentSelection => currentSelection;
    public void Select(SkinButton selected)
    {
        foreach (var button in skinButtons)
            button.SetSelected(false);

        selected.SetSelected(true);
        currentSelection = selected;
        snakePreview.ShowSkin(selected.SkinData);
    }
}
