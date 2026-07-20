using UnityEngine;

public class PlayerSkinManager : MonoBehaviour
{
    public static PlayerSkinManager Instance { get; private set; }

    [SerializeField] private SnakeSkinDatabase database;

    private const string EquippedSkinKey = "EquippedSkin";
    private const string FirstLaunchKey = "SkinSystemInitialized";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            InitializeSave();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeSave()
    {
        if (PlayerPrefs.HasKey(FirstLaunchKey))
            return;

        // Unlock the default skin
        UnlockSkin("18");

        // Equip the default skin
        EquipSkin("18");

        PlayerPrefs.SetInt(FirstLaunchKey, 1);
        PlayerPrefs.Save();
    }

    public SnakeSkinData EquippedSkin
    {
        get
        {
            string skinID = PlayerPrefs.GetString(EquippedSkinKey, "18");
            return database.GetSkinByID(skinID);
        }
    }

    public void EquipSkin(string skinID)
    {
        if (!IsUnlocked(skinID))
            return;

        PlayerPrefs.SetString(EquippedSkinKey, skinID);
        PlayerPrefs.Save();
    }

    public bool IsUnlocked(string skinID)
    {
        return PlayerPrefs.GetInt("Skin_" + skinID, 0) == 1;
    }

    public void UnlockSkin(string skinID)
    {
        PlayerPrefs.SetInt("Skin_" + skinID, 1);
        PlayerPrefs.Save();
    }
}