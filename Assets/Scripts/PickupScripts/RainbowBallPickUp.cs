using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainbowBallPickUp : PickupBase
{
    public override void OnPickup(GameObject onpick)
    {
        RainBowPowerUp rainbow = onpick.GetComponentInParent<RainBowPowerUp>();

        if (rainbow != null)
        {
            rainbow.ActivateRainbowMode();
        }

        Destroy(gameObject);
    }
}
