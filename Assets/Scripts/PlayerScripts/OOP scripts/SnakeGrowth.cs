using System;
using UnityEngine;

public class SnakeGrowth : MonoBehaviour
{
    [SerializeField] private int foodsPerGrowth = 4;
    private int foodCounter;
    private SnakeFoodStorage foodStorage;
    private SnakeBody body;
    private SnakeEnergy energy;
    private int pendingGrowth;
    public event Action OnGrow;

    private void Awake()
    {
        body = GetComponent<SnakeBody>();
        energy = GetComponent<SnakeEnergy>();
        foodStorage = GetComponent<SnakeFoodStorage>();
    }

    private void Update()
    {
        if (pendingGrowth > 0)
        {
            pendingGrowth--;
            body.AddSegment();
            OnGrow?.Invoke();
        }
    }
    public void AddFood()
    {
        foodCounter++;
        foodStorage?.AddFood(1);
        energy?.AddEnergy(1);
        FoodCountManager.Instance?.AddFruit(1);
        if (foodCounter >= foodsPerGrowth)
        {
            foodCounter = 0;
            pendingGrowth++;
        }
    }
}

