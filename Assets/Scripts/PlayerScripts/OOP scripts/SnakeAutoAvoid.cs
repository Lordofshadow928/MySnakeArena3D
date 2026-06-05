using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeAutoAvoid : MonoBehaviour
{
    [Header("Obstacle Detection")]
    [SerializeField] private LayerMask Obstacle;

    [Header("Auto Avoid")]
    [SerializeField] private float autoAvoidStrength = 1.5f;
    [SerializeField] private float autoAvoidDecay = 8f;
    [SerializeField] private float collisionCooldown = 0.15f;

    private SnakeMovement movement;
    private SnakeStuckRecovery recovery;
    private float autoSteerInput;
    private bool canAutoAvoid = true;

    private void Awake()
    {
        movement = GetComponent<SnakeMovement>();
        recovery = GetComponent<SnakeStuckRecovery>();
    }

    private void Update()
    {
        if (recovery.IsRecovering)
        {
            movement.AvoidanceSteering = 0f;
            return;
        }

        autoSteerInput = Mathf.Lerp(autoSteerInput, 0f, autoAvoidDecay * Time.deltaTime);

        movement.AvoidanceSteering = autoSteerInput;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!canAutoAvoid)
            return;

        if ((Obstacle.value & (1 << collision.gameObject.layer)) == 0)
            return;

        ContactPoint contact = collision.contacts[0];

        AutoAvoid(contact.normal);
    }

    private void AutoAvoid(Vector3 collisionNormal)
    {
        canAutoAvoid = false;

        Vector3 obstacleDirection = -collisionNormal;

        float angle = Vector3.SignedAngle(transform.forward, obstacleDirection, Vector3.up);

        float steerDirection = -Mathf.Sign(angle);

        if (steerDirection == 0)
        {
            steerDirection = Random.value > 0.5f ? 1f : -1f;
        }

        autoSteerInput = steerDirection * autoAvoidStrength;

        Invoke(nameof(ResetCollision), collisionCooldown);
    }

    private void ResetCollision()
    {
        canAutoAvoid = true;
    }
}
