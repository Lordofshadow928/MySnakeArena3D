using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeCollector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PickupBase pickup = other.GetComponent<PickupBase>();
        if (pickup != null)
        {
            pickup.OnPickup(gameObject);
        }
    }
}
