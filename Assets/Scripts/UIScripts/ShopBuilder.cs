using System.Collections.Generic;
using UnityEngine;

public class ShopBuilder : MonoBehaviour
{
    [System.Serializable]
    public class ShopPage
    {
        public SkinRarity rarity;
        public SkinButton[] buttons;
    }

    [Header("Database")]
    [SerializeField] private SnakeSkinDatabase database;

    [Header("Pages")]
    [SerializeField] private ShopPage[] pages;

    private void Awake()
    {
        BuildShop();
    }

    private void BuildShop()
    {
        if (database == null)
        {
            Debug.LogError("ShopBuilder: Database is missing.");
            return;
        }

        foreach (ShopPage page in pages)
        {
            foreach (SkinButton button in page.buttons)
            {
                if (button == null)
                    continue;

                SnakeSkinData skin = database.GetSkinByID(button.SkinID);
                if ( skin != null)
                {
                    button.gameObject.SetActive(true);
                    button.Initialize(skin);
                }
                else
                {
                    button.gameObject.SetActive(false);
                }
            }
        }
    }
}