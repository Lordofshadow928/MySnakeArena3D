using UnityEngine;

public class AISnakeBrain : MonoBehaviour
{
    [Header("Steering")]
    [SerializeField] private float steeringSensitivity = 90f;
    [SerializeField] private float steeringSmoothness = 4f;

    private SnakeMovement movement;
    private AISnakeObstacleSensor sensor;

    private void Awake()
    {
        movement = GetComponentInParent<SnakeMovement>();
        sensor = GetComponentInParent<AISnakeObstacleSensor>();
    }

    private void FixedUpdate()
    {
        if (FoodManager.Instance == null)
            return;

        Transform target = FoodManager.Instance.GetNearestFood(transform.position);

        if (target == null)
        {
            movement.SteeringInput = 0f;
            return;
        }

        Vector3 foodDirection = (target.position - transform.position).normalized;

        Vector3 finalDirection = foodDirection + sensor.AvoidanceDirection;

        finalDirection.y = 0f;

        if (finalDirection.sqrMagnitude < 0.01f)
            return;

        finalDirection.Normalize();

        float angle = Vector3.SignedAngle(transform.forward, finalDirection, Vector3.up);

        float targetSteering = Mathf.Clamp(angle / steeringSensitivity, -1f, 1f);

        movement.SteeringInput = Mathf.Lerp(movement.SteeringInput, targetSteering, steeringSmoothness * Time.fixedDeltaTime);
    }
}