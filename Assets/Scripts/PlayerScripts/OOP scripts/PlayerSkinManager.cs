using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkinManager : MonoBehaviour
{
    public static PlayerSkinManager Instance { get; private set; }

    [SerializeField] private SnakeSkinDatabase database;

    private const string EquippedSkinKey = "EquippedSkin";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public SnakeSkinData EquippedSkin
    {
        get
        {
            int index = PlayerPrefs.GetInt(EquippedSkinKey, 0);
            return database.GetSkin(index);
        }
    }

    public void EquipSkin(int index)
    {
        PlayerPrefs.SetInt(EquippedSkinKey, index);
        PlayerPrefs.Save();
    }
}
