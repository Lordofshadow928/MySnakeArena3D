using UnityEngine;

[CreateAssetMenu(menuName = "Snake/Food Count Data")]
public class FoodCountData : ScriptableObject
{
    public int stageIndex;

    [Header("Unlock")]
    public int requiredFruit;

    [Header("Requirement UI")]
    public Sprite requirementSprite;

    public string stageName;
}