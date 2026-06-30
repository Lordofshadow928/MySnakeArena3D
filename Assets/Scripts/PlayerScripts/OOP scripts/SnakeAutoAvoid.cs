using UnityEngine;

[RequireComponent(typeof(SnakeMovement))]
public class SnakeAutoAvoid : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LayerMask obstacleMask;

    [Header("Sensor")]
    [SerializeField] private float sensorDistance = 1.5f;
    [SerializeField] private float sensorRadius = 0.35f;
    [SerializeField] private float sensorHeight = 0.2f;
    [SerializeField] private float sideOffset = 0.45f;
    [SerializeField] private float sensorAngle = 20f;

    [Header("Steering")]
    [SerializeField] private float maxAvoidSteering = 1f;
    [SerializeField] private float steeringSmooth = 10f;
    [SerializeField] private float steeringRelease = 6f;

    [Header("Weights")]
    [SerializeField] private float centerWeight = 1f;
    [SerializeField] private float sideWeight = 0.45f;

    private SnakeMovement movement;

    private float currentSteer;

    private struct SensorHit
    {
        public bool hit;
        public RaycastHit info;
    }

    private void Awake()
    {
        movement = GetComponent<SnakeMovement>();
    }

    private void FixedUpdate()
    {
        SensorHit left = CastSensor(-sideOffset, -sensorAngle);
        SensorHit center = CastSensor(0f, 0f);
        SensorHit right = CastSensor(sideOffset, sensorAngle);

        float desiredSteer = CalculateSteering(left, center, right);

        bool hasObstacle = left.hit || center.hit || right.hit;

        if (hasObstacle)
        {
            currentSteer = Mathf.MoveTowards( currentSteer, desiredSteer, steeringSmooth * Time.fixedDeltaTime);
        }
        else
        {
            currentSteer = Mathf.MoveTowards( currentSteer, 0f, steeringRelease * Time.fixedDeltaTime);
        }
        movement.AvoidanceSteering = currentSteer;
    }

    private SensorHit CastSensor(float xOffset, float angleOffset)
    {
        SensorHit result = new SensorHit();

        Vector3 origin = transform.position + transform.right * xOffset + Vector3.up * sensorHeight;

        Vector3 direction = Quaternion.Euler(0f, angleOffset, 0f) * transform.forward;

        result.hit = Physics.SphereCast( origin, sensorRadius, direction, out result.info, sensorDistance, obstacleMask, QueryTriggerInteraction.Ignore);

        return result;
    }

    private float CalculateSteering( SensorHit left, SensorHit center, SensorHit right)
    {
        float steer = 0f;
        if (left.hit)
        {
            float strength =
                1f - (left.info.distance / sensorDistance);

            steer += sideWeight * strength;
        }

        if (right.hit)
        {
            float strength =
                1f - (right.info.distance / sensorDistance);

            steer -= sideWeight * strength;
        }

        if (center.hit)
        {
            Vector3 tangentA = Vector3.Cross(Vector3.up, center.info.normal).normalized;

            Vector3 tangentB = -tangentA;

            Vector3 slideDirection = Vector3.Dot(transform.forward, tangentA) > Vector3.Dot(transform.forward, tangentB) ? tangentA : tangentB;

            float angle = Vector3.SignedAngle( transform.forward, slideDirection, Vector3.up);

            float slideSteer = Mathf.Clamp(angle / 60f, -1f, 1f);

            float strength = 1f - (center.info.distance / sensorDistance);

            steer += slideSteer * strength * centerWeight;
        }

        return Mathf.Clamp( steer, -maxAvoidSteering, maxAvoidSteering);
    }

    private void OnDrawGizmosSelected()
    {
        DrawSensor(-sideOffset, -sensorAngle, Color.green);
        DrawSensor(0f, 0f, Color.yellow);
        DrawSensor(sideOffset, sensorAngle, Color.red);
    }

    private void DrawSensor(float xOffset, float angle, Color color)
    {
        Gizmos.color = color;

        Vector3 origin = transform.position + transform.right * xOffset + Vector3.up * sensorHeight;

        Vector3 direction = Quaternion.Euler(0f, angle, 0f) * transform.forward;

        Vector3 end = origin + direction * sensorDistance;

        Gizmos.DrawWireSphere(origin, sensorRadius);
        Gizmos.DrawWireSphere(end, sensorRadius);
        Gizmos.DrawLine(origin, end);
    }
}