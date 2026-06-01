using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private SnakeMovement movement;
    private void Awake()
    {
        movement = GetComponentInParent<SnakeMovement>();
    }

    private void Update()
    {
        movement.SteeringInput = Input.GetAxis("Horizontal");
    }
}
