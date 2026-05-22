using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvinciblePickup : PickupBase
{
    [SerializeField] private float duration = 10f;
    public override void OnPickup(GameObject onpick)
    {
        SnakeInvincible invincible = onpick.GetComponent<SnakeInvincible>();
        if (invincible != null)
        {
            invincible.ActivateInvincible(duration);
        }

        LeanPool.Despawn(gameObject);
    }
}

