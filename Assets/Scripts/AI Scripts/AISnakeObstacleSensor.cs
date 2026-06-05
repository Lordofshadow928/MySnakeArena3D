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
    [SerializeField] float reactionInterval = 0.3f;

    [Header("Weights")]
    [SerializeField] private float wallAvoidWeight = 2f;
    [SerializeField] private float snakeAvoidWeight = 0.75f;

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

        //AvoidanceDirection =
        //    wallAvoidance * wallAvoidWeight +
        //    snakeAvoidance * snakeAvoidWeight;

        //AvoidanceDirection.y = 0f;
        Vector3 avoidance = wallAvoidance * wallAvoidWeight + snakeAvoidance * snakeAvoidWeight;

        avoidance.y = 0f;

        AvoidanceDirection = avoidance;
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
            return -direction.normalized;
        }

        return Vector3.zero;
    }

    private Vector3 GetSnakeAvoidance()
    {
        Vector3 avoidance = Vector3.zero;

        Collider[] hits = Physics.OverlapSphere(transform.position, snakeDetectionRadius, Snake);

        foreach (Collider hit in hits)
        {
            if (hit.transform.root == transform)
                continue;

            Vector3 away = transform.position - hit.transform.position;

            float distance = away.magnitude;

            if (distance < 0.01f)
                continue;

            avoidance += away.normalized / distance;
        }

        return avoidance.normalized;
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