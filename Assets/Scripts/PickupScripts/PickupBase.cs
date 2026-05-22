using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PickupBase : MonoBehaviour
{
    public abstract void OnPickup(GameObject onpick);
}
