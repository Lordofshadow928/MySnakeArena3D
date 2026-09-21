using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopEquipController : MonoBehaviour
{
    [SerializeField] private ShopSelectionManager selectionManager;

    [Header("UI")]
    [SerializeField] private Button equipButton;
    [SerializeField] private Button unlockButton;

    [SerializeField] private Image coinIcon;

    [SerializeField] private TMP_Text equipText;
    [SerializeField] private TMP_Text unlockText;
    [SerializeField] private TMP_Text coinPriceText;

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

        bool isUnlocked = PlayerSkinManager.Instance.IsUnlocked(skin.skinID);

        bool isEquipped = PlayerSkinManager.Instance.IsEquipped(skin.skinID);

        // LOCKED
        if (!isUnlocked)
        {
            equipButton.gameObject.SetActive(false);

            unlockButton.gameObject.SetActive(true);
            unlockButton.interactable = true;

            unlockText.text = "UNLOCK";

            coinIcon.gameObject.SetActive(true);
            coinPriceText.gameObject.SetActive(true);

            coinPriceText.text = skin.coinPrice.ToString();

            return;
        }

        // EQUIPPED
        if (isEquipped)
        {
            equipButton.gameObject.SetActive(true);
            equipButton.interactable = false;

            equipText.text = "EQUIPPED";

            unlockButton.gameObject.SetActive(false);

            coinIcon.gameObject.SetActive(false);
            coinPriceText.gameObject.SetActive(false);

            return;
        }

        // UNLOCKED BUT NOT EQUIPPED
        equipButton.gameObject.SetActive(true);
        equipButton.interactable = true;

        equipText.text = "EQUIP";

        unlockButton.gameObject.SetActive(false);

        coinIcon.gameObject.SetActive(false);
        coinPriceText.gameObject.SetActive(false);
    }

    public void UnlockSelected()
    {
        SkinButton selected = selectionManager.CurrentSelection;

        if (selected == null)
        {
            return;
        }

        SnakeSkinData skin = selected.SkinData;

        // Already unlocked?
        if (PlayerSkinManager.Instance.IsUnlocked(skin.skinID))
        {
            Refresh();
            return;
        }

        // Check whether the player can afford it
        if (!CoinManager.Instance.CanAfford(skin.coinPrice))
        {
            return;
        }

        // Spend coins
        bool spent = CoinManager.Instance.SpendCoins(skin.coinPrice);

        if (!spent)
        {
            return;
        }

        // Unlock skin
        PlayerSkinManager.Instance.UnlockSkin(skin.skinID);
        // Update ActionPanel
        Refresh();
    }

    public void EquipSelected()
    {
        SkinButton selected = selectionManager.CurrentSelection;

        if (selected == null)
            return;

        PlayerSkinManager.Instance.EquipSkin(selected.SkinData.skinID);

        menuPreview.ShowSkin( PlayerSkinManager.Instance.EquippedSkin);
        Refresh();
    }
}