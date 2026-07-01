using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

public class FoodPickUp : PickupBase
{
    private void OnEnable()
    {
        FoodManager.Instance?.RegisterFood(transform);
    }

    private void OnDisable()
    {
        FoodManager.Instance?.UnregisterFood(transform);
    }

    public override void OnPickup(GameObject onpick)
    {
        SnakeGrowth growth = onpick.GetComponent<SnakeGrowth>();
        SnakeFoodStorage storage = onpick.GetComponent<SnakeFoodStorage>();

        FoodDemo food = GetComponent<FoodDemo>();

        if (growth != null && food != null)
        {
            growth.AddFood(food.FruitType);
        }
        LeanPool.Despawn(gameObject);
    }
}
