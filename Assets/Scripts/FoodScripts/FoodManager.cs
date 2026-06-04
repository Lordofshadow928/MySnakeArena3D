using System.Collections.Generic;
using UnityEngine;

public class FoodManager : MonoBehaviour
{
    public static FoodManager Instance { get; private set; }

    private readonly List<Transform> foods = new();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void RegisterFood(Transform food)
    {
        if (!foods.Contains(food))
            foods.Add(food);
    }

    public void UnregisterFood(Transform food)
    {
        foods.Remove(food);
    }

    public Transform GetNearestFood(Vector3 position)
    {
        Transform nearest = null;
        float shortestDistance = Mathf.Infinity;

        foreach (Transform food in foods)
        {
            if (food == null)
                continue;

            float distance = (food.position - position).sqrMagnitude;

            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearest = food;
            }
        }

        return nearest;
    }
}
