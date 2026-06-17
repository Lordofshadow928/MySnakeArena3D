using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeStuckRecovery : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LayerMask Obstacle;
    [Header("Stuck Detection")]
    //[SerializeField] private float stuckSpeedThreshold = 0.2f;
    [SerializeField] private float stuckTime = 2f;
    [SerializeField] private float recoverySteering = 3f;

    private SnakeMovement movement;
    private SnakeWallContact friction;
    public bool IsRecovering => stuckTimer >= stuckTime;
    private float stuckTimer;
    private float recoveryDirection;

    private void Awake()
    {
        movement = GetComponent<SnakeMovement>();
        friction = GetComponent<SnakeWallContact>();
    }

    private void Update()
    {
        Debug.Log($"Speed: {movement.CurrentSpeed:F2} | Pressure: {friction.WallPressure:F2}");
        bool stuck = friction.WallPressure >= 0.95f;

        if (stuck)
        {
            stuckTimer += Time.deltaTime;
        }
        else
        {
            stuckTimer = 0f;
            movement.RecoverySteering = 0f;
        }

        if (stuckTimer >= stuckTime)
        {
            movement.RecoverySteering = recoveryDirection * recoverySteering;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((Obstacle.value & (1 << collision.gameObject.layer)) == 0)
            return;
        ContactPoint contact = collision.contacts[0];

        Vector3 obstacleDirection = -contact.normal;

        float angle = Vector3.SignedAngle(transform.forward, obstacleDirection, Vector3.up);

        recoveryDirection = -Mathf.Sign(angle);

        if (recoveryDirection == 0)
            recoveryDirection = Random.value > 0.5f ? 1f : -1f;
    }
}
