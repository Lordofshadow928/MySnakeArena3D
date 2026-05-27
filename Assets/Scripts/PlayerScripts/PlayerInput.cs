using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private OnlyMovement movement;

    private void Awake()
    {
        movement = GetComponent<OnlyMovement>();
    }

    private void Update()
    {
        movement.SteeringInput = Input.GetAxis("Horizontal");
    }
}
