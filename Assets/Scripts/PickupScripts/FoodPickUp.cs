using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

public class FoodPickUp : PickupBase
{
    public override void OnPickup(GameObject onpick)
    {
        SnakeGrowth growth = onpick.GetComponent<SnakeGrowth>();
        SnakeFoodStorage storage = onpick.GetComponent<SnakeFoodStorage>();

        if (growth != null)
        {
            growth.AddFood();
        }

        if (storage != null)
        {
            storage.AddFood(1);
        }
        LeanPool.Despawn(gameObject);
    }
}
