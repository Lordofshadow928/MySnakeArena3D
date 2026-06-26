using System;
using UnityEngine;

public class SnakeFoodStorage : MonoBehaviour
{
    public int StoredFood { get; private set; }
    public event Action OnFoodChanged;
    public void AddFood(int amount)
    {
        StoredFood += amount;
        Debug.Log($"Current Run Food = {StoredFood}");
        OnFoodChanged?.Invoke();
    }

    public void RemoveFood(int amount)
    {
        StoredFood -= amount;
        StoredFood = Mathf.Max(0, StoredFood);
        OnFoodChanged?.Invoke();
    }
}
