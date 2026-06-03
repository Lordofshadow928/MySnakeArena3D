using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

public class FoodPickUp : PickupBase
{
    public override void OnPickup(GameObject onpick)
    {
        SnakeGrowth growth = onpick.GetComponent<SnakeGrowth>();
        if (growth != null)
        {
            growth.AddFood();
        }
        LeanPool.Despawn(gameObject);
    }
}
