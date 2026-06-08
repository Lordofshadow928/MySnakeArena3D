using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeCollisionDeath : MonoBehaviour
{
    [SerializeField] private SnakeHealth health;

    private void OnTriggerEnter(Collider other)
    {
        SnakePart part = other.GetComponentInParent<SnakePart>();

        if (part == null)
            return;

        // Ignore our own snake
        if (part.Owner == health)
            return;

        health.Die();
    }
}
