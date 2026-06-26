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

        if (growth != null)
        {
            growth.AddFood();
        }
        LeanPool.Despawn(gameObject);
    }
}
