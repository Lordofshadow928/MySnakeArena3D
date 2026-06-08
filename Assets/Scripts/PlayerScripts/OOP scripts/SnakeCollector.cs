using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeCollector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Collided with: " + other.name);
        PickupBase pickup = other.GetComponent<PickupBase>();
        if (pickup == null)
        {
            //Debug.Log("Picked up detected");
            return;
        }
        if(!pickup.CanBeCollectedBy(gameObject))
        {
            //Debug.Log("Pickup cannot be collected by this object");
            return;
        }
        pickup.OnPickup(gameObject);
    }
}

