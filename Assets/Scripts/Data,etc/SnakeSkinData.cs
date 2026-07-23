using UnityEngine;

public enum SkinRarity
{
    Common,
    Rare,
    Epic,
    Event
}

[System.Serializable]
public class SnakeSkinData
{
    [Header("Info")]
    public string skinID;
    public string skinName;
    public SkinRarity rarity;

    [Header("Shop")]
    public int coinPrice;

    public Sprite icon;          
    public Sprite lockedIcon;   

    [Header("Snake Prefabs")]
    public GameObject headPrefab;
    public GameObject bodyPrefab;
    public GameObject tailPrefab;
}