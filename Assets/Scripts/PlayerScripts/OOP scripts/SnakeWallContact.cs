using UnityEngine;

public class SnakeWallContact : MonoBehaviour
{
    [Header("Obstacle Detection")]
    [SerializeField] private LayerMask Obstacle;

    [Header("Wall Friction")]
    [SerializeField] private float pressureBuildSpeed = 0.5f;
    [SerializeField] private float pressureReleaseSpeed = 2f;

    [Header("Speed Reduction")]
    [SerializeField] private float maxSlowdown = 0.9f;
    // 0.9 = can slow down to 10% speed

    private SnakeMovement movement;

    //private bool touchingWall;
    private float wallPressure;
    private float lastWallContactTime;
    public bool IsTouchingWall => Time.time - lastWallContactTime < 0.1f;
    public float WallPressure => wallPressure;

    private void Awake()
    {
        movement = GetComponent<SnakeMovement>();
    }

    private void Update()
    {
        bool touchingWall = IsTouchingWall;

        float targetPressure = touchingWall ? 1f : 0f;

        float speed = touchingWall ? pressureBuildSpeed : pressureReleaseSpeed;

        wallPressure = Mathf.MoveTowards(wallPressure, targetPressure, speed * Time.deltaTime);
        Debug.Log($"Touching:{IsTouchingWall} Pressure:{wallPressure:F2}"
);
        float speedMultiplier = Mathf.Lerp(1f, 1f - maxSlowdown, wallPressure);

        movement.SetSpeedMultiplier(speedMultiplier);

        // Reset each frame.
        // OnCollisionStay will set it back to true if still touching.
        
    }

    private void OnCollisionStay(Collision collision)
    {
        if ((Obstacle.value & (1 << collision.gameObject.layer)) == 0)
            return;

        lastWallContactTime = Time.time;
    }
}