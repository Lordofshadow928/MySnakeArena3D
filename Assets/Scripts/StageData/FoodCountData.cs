using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "FoodCountData", menuName = "Snake/Stage Data")]
public class FoodCountData : ScriptableObject
{
    public int stageIndex;
    public int requiredFruit;
    
}
