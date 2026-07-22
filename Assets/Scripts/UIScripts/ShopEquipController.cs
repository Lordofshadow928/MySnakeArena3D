using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopEquipController : MonoBehaviour
{
    [SerializeField] private ShopSelectionManager selectionManager;

    [Header("UI")]
    [SerializeField] private Button equipButton;
    [SerializeField] private TMP_Text buttonText;
    [SerializeField] private SnakeUIPreview menuPreview;
    private void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        SkinButton selected = selectionManager.CurrentSelection;

        if (selected == null)
            return;

        SnakeSkinData skin = selected.SkinData;

        if (!PlayerSkinManager.Instance.IsUnlocked(skin.skinID))
        {
            equipButton.interactable = false;
            buttonText.text = "Locked";
            return;
        }

        if (PlayerSkinManager.Instance.IsEquipped(skin.skinID))
        {
            equipButton.interactable = false;
            buttonText.text = "EQUIPPED";
            return;
        }

        equipButton.interactable = true;
        buttonText.text = "EQUIP";
    }

    public void EquipSelected()
    {
        SkinButton selected = selectionManager.CurrentSelection;

        if (selected == null)
            return;

        PlayerSkinManager.Instance.EquipSkin(selected.SkinData.skinID);
        menuPreview.ShowSkin(PlayerSkinManager.Instance.EquippedSkin);
        Refresh();
    }
}