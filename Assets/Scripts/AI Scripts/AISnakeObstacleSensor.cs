using UnityEngine;

public class AISnakeObstacleSensor : MonoBehaviour
{
    [Header("Layers")]
    [SerializeField] private LayerMask Obstacle;
    [SerializeField] private LayerMask Snake;

    [Header("Wall Detection")]
    [SerializeField] private float wallSensorDistance = 4f;
    [SerializeField] private float sideAngle = 45f;

    [Header("Snake Detection")]
    [SerializeField] private float snakeDetectionRadius = 1.5f;
    [SerializeField] private float reactionInterval = 0.3f;

    [Header("Weights")]
    [SerializeField] private float wallAvoidWeight = 2f;
    [SerializeField] private float snakeAvoidWeight = 0.75f;

    [Header("Vision")]
    [SerializeField] private float snakeViewDot = 0.5f;

    public Vector3 AvoidanceDirection { get; private set; }

    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= reactionInterval)
        {
            timer = 0f;
            CalculateAvoidance();
        }
    }

    private void CalculateAvoidance()
    {
        Vector3 wallAvoidance = GetWallAvoidance();
        Vector3 snakeAvoidance = GetSnakeAvoidance();

        Vector3 avoidance = wallAvoidance * wallAvoidWeight + snakeAvoidance * snakeAvoidWeight;

        avoidance.y = 0f;

        AvoidanceDirection = avoidance.sqrMagnitude > 0.01f ? avoidance.normalized : Vector3.zero;
    }

    private Vector3 GetWallAvoidance()
    {
        Vector3 avoidance = Vector3.zero;

        Vector3 forward = transform.forward;
        Vector3 left = Quaternion.Euler(0f, -sideAngle, 0f) * forward;
        Vector3 right = Quaternion.Euler(0f, sideAngle, 0f) * forward;

        avoidance += CheckRay(forward) * 2f;
        avoidance += CheckRay(left);
        avoidance += CheckRay(right);

        return avoidance;
    }

    private Vector3 CheckRay(Vector3 direction)
    {
        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, wallSensorDistance, Obstacle))
        {
            float strength = 1f - (hit.distance / wallSensorDistance);

            return -direction.normalized * strength;
        }

        return Vector3.zero;
    }

    private Vector3 GetSnakeAvoidance()
    {
        Vector3 avoidance = Vector3.zero;

        Collider[] hits = Physics.OverlapSphere(transform.position, snakeDetectionRadius, Snake);

        foreach (Collider hit in hits)
        {
            if (hit.transform.root == transform.root)
                continue;

            Vector3 toSnake = (hit.transform.position - transform.position).normalized;

            float dot = Vector3.Dot(transform.forward, toSnake);

            if (dot < snakeViewDot)
                continue;

            Vector3 away = transform.position - hit.transform.position;

            float distance = away.magnitude;

            if (distance < 0.01f)
                continue;

            avoidance += away.normalized / distance;
        }

        return avoidance;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Vector3 forward = transform.forward;
        Vector3 left = Quaternion.Euler(0f, -sideAngle, 0f) * forward;
        Vector3 right = Quaternion.Euler(0f, sideAngle, 0f) * forward;

        Gizmos.DrawRay(transform.position, forward * wallSensorDistance);
        Gizmos.DrawRay(transform.position, left * wallSensorDistance);
        Gizmos.DrawRay(transform.position, right * wallSensorDistance);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, snakeDetectionRadius);
    }
}