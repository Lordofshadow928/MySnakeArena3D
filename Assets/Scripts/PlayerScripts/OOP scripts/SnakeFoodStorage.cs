using UnityEngine;

public class SnakeFoodStorage : MonoBehaviour
{
    public int StoredFood { get; private set; }

    public void AddFood(int amount)
    {
        StoredFood += amount;
    }

    public void RemoveFood(int amount)
    {
        StoredFood -= amount;
        StoredFood = Mathf.Max(0, StoredFood);
    }
}
