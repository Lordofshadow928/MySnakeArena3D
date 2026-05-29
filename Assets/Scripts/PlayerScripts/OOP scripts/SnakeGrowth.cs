using System;
using UnityEngine;

public class SnakeGrowth : MonoBehaviour
{
    [SerializeField] private int foodsPerGrowth = 4;

    private int foodCounter;

    private SnakeBody body;

    public event Action OnGrow;

    private void Awake()
    {
        body = GetComponent<SnakeBody>();
    }

    public void AddFood()
    {
        foodCounter++;

        if (foodCounter >= foodsPerGrowth)
        {
            foodCounter = 0;
            body.AddSegment();
            OnGrow?.Invoke();
        }
    }
}

