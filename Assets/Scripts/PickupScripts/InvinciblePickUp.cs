using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvinciblePickup : PickupBase
{
    [SerializeField] private float duration = 5f;
    [SerializeField] private GameObject rainbowBallPrefab;

    public override void OnPickup(GameObject onpick)
    {
        Debug.Log($"Invincible for {duration} seconds!");

        // Future:
        // collector.GetComponent<PlayerHealth>()
        //     ?.SetInvincible(duration);

        LeanPool.Despawn(gameObject);
    }
}

