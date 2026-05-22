using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

public class FoodPickUp : PickupBase
{
    public override void OnPickup(GameObject onpick)
    {
        GrowthShrinkLogic growth = onpick.GetComponent<GrowthShrinkLogic>();
        if (growth != null)
        {
            growth.AddFood();
        }
        LeanPool.Despawn(gameObject);
    }
}
