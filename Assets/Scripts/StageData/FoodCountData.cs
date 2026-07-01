using UnityEngine;

[CreateAssetMenu(menuName = "Snake/Food Count Data")]
public class FoodCountData : ScriptableObject
{
    public int stageIndex;

    [Header("Unlock")]
    public int requiredFruit;
    public FruitType fruitType;
    [Header("Requirement UI")]
    public Sprite fruitSprite;
    
}
public enum FruitType
{
    None,
    Apple,
    Banana,
    Cherry,
    Grape,
    Orange,
    Watermelon,
    Pineapple,
    Strawberry,
    Mango,
    Broccoli,
    Carrot,
    Pear,
    DragonFruit,
    Pomegranate,
}