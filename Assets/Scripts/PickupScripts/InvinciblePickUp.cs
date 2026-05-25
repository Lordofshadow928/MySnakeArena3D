
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvinciblePickup : PickupBase
{
    public override void OnPickup(GameObject onpick)
    {
        
        SnakeInvincible invincible = onpick.GetComponent<SnakeInvincible>();
        if (invincible != null)
        {
            invincible.ActivateInvincible();
        }

        Destroy(gameObject);
    }
}

