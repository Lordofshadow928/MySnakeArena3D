using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeCollisionDeath : MonoBehaviour
{
    [SerializeField] private SnakeHealth health;
    private SnakeInvincible2 invincible;

    private void Awake()
    {
        invincible = GetComponentInChildren<SnakeInvincible2>();
    }
    private void OnTriggerEnter(Collider other)
    {
        SnakePart part = other.GetComponentInParent<SnakePart>();

        if (part == null)
            return;
        // If invincible, kill other snake
        if (invincible != null && invincible.IsInvincible)
        {
            part.Owner.Die();
            return;
        }
        // Ignore our own snake
        if (part.Owner == health)
            return;

        health.Die();
    }
}
