using UnityEngine;

public class AISnakeBrain : MonoBehaviour
{
    [Header("Steering")]
    [SerializeField] private float steeringSensitivity = 120f;
    [SerializeField] private float steeringSmoothness = 2f;
    [SerializeField] private float steeringDeadZone = 12f;

    [Header("Targeting")]
    [SerializeField] private float targetRefreshRate = 0.75f;

    private SnakeMovement movement;
    private AISnakeObstacleSensor sensor;

    private Transform currentTarget;
    private float targetTimer;

    private void Awake()
    {
        movement = GetComponentInParent<SnakeMovement>();
        sensor = GetComponentInParent<AISnakeObstacleSensor>();
    }

    private void FixedUpdate()
    {
        if (FoodManager.Instance == null)
            return;

        targetTimer += Time.fixedDeltaTime;

        if (currentTarget == null || targetTimer >= targetRefreshRate)
        {
            targetTimer = 0f;
            currentTarget = FoodManager.Instance.GetNearestFood(transform.position);
        }

        if (currentTarget == null)
        {
            movement.SteeringInput = 0f;
            return;
        }

        Vector3 foodDirection = (currentTarget.position - transform.position).normalized;

        Vector3 finalDirection = foodDirection;

        if (sensor.AvoidanceDirection.sqrMagnitude > 0.1f)
        {
            finalDirection += sensor.AvoidanceDirection;
        }

        finalDirection.y = 0f;

        if (finalDirection.sqrMagnitude < 0.01f)
            return;

        finalDirection.Normalize();

        float angle = Vector3.SignedAngle(transform.forward, finalDirection, Vector3.up);

        float targetSteering = 0f;

        if (Mathf.Abs(angle) > steeringDeadZone)
        {
            targetSteering = Mathf.Clamp(angle / steeringSensitivity, -1f, 1f);
        }

        movement.SteeringInput = Mathf.Lerp(movement.SteeringInput, targetSteering, steeringSmoothness * Time.fixedDeltaTime);
    }
}