using UnityEngine;

public class OnlyMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float rotateSpeed = 240f;

    [Header("Auto Avoid")]
    [SerializeField] private LayerMask Obstacle;
    [SerializeField] private float autoAvoidStrength = 1.5f;
    [SerializeField] private float autoAvoidDecay = 8f;
    [SerializeField] private float collisionCooldown = 0.15f;

    private float defaultMoveSpeed;
    private float defaultRotateSpeed;

    private bool isBoosted;

    private Rigidbody rb;

    public float SteeringInput { get; set; }

    // Smooth auto steering
    private float autoSteerInput;

    // Prevent collision spam
    private bool canAutoAvoid = true;

    // Real movement speed
    private Vector3 lastPosition;
    private float currentSpeed;

    public float CurrentSpeed => currentSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        defaultMoveSpeed = moveSpeed;
        defaultRotateSpeed = rotateSpeed;
    }

    private void FixedUpdate()
    {
        Move();

        // Calculate real speed
        currentSpeed =
            Vector3.Distance(transform.position, lastPosition)
            / Time.fixedDeltaTime;

        lastPosition = transform.position;

        // Smoothly remove auto steering over time
        autoSteerInput = Mathf.Lerp(
            autoSteerInput,
            0f,
            autoAvoidDecay * Time.fixedDeltaTime
        );
    }

    private void Move()
    {
        // Combine player steering + auto avoid steering
        float finalInput = SteeringInput + autoSteerInput;

        finalInput = Mathf.Clamp(finalInput, -1f, 1f);

        Quaternion deltaRotation =
            Quaternion.Euler(
                Vector3.up *
                finalInput *
                rotateSpeed *
                Time.fixedDeltaTime
            );

        rb.MoveRotation(rb.rotation * deltaRotation);

        Vector3 moveDirection =
            transform.forward *
            moveSpeed *
            Time.fixedDeltaTime;

        rb.MovePosition(rb.position + moveDirection);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!canAutoAvoid) return;

        // Ignore non-obstacles
        if ((Obstacle.value & (1 << collision.gameObject.layer)) == 0)
            return;
        ContactPoint contact = collision.contacts[0];
        AutoAvoid(contact.normal);
    }

    private void AutoAvoid(Vector3 collisionNormal)
    {
        canAutoAvoid = false;

        // Direction toward obstacle
        Vector3 obstacleDirection = -collisionNormal;

        // Calculate signed angle
        float angle =
            Vector3.SignedAngle(
                transform.forward,
                obstacleDirection,
                Vector3.up
            );

        // Turn AWAY from obstacle
        float steerDirection = -Mathf.Sign(angle);

        // Perfect front collision fallback
        if (steerDirection == 0)
        {
            steerDirection =
                Random.value > 0.5f ? 1f : -1f;
        }

        // Apply smooth steering force
        autoSteerInput =
            steerDirection * autoAvoidStrength;

        Invoke(nameof(ResetCollision), collisionCooldown);
    }

    private void ResetCollision()
    {
        canAutoAvoid = true;
    }

    public void BoostSpeed(float multiplier)
    {
        moveSpeed = defaultMoveSpeed * multiplier;
        rotateSpeed = defaultRotateSpeed * multiplier;

        isBoosted = true;
    }

    public void ResetSpeed()
    {
        moveSpeed = defaultMoveSpeed;
        rotateSpeed = defaultRotateSpeed;

        isBoosted = false;
    }

    public bool IsBoosted()
    {
        return isBoosted;
    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }
}