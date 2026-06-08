using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PickupBase : MonoBehaviour
{
    [Header("Collector Settings")]
    [SerializeField] private PickupTarget pickupTarget = PickupTarget.Everyone;

    public bool CanBeCollectedBy(GameObject onpick)
    {
        bool isPlayer = onpick.GetComponent<PlayerSnake>() != null;
        bool isAI = onpick.GetComponent<AISnake>() != null;

        switch (pickupTarget)
        {
            case PickupTarget.PlayerOnly:
                return isPlayer;

            case PickupTarget.AIOnly:
                return isAI;

            default:
                return true;
        }
    }
    public abstract void OnPickup(GameObject onpick);
}
