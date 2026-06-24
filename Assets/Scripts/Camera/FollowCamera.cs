using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform snakeHead;
    [SerializeField] private SnakeMovement snakeMovement;

    [Header("Camera")]
    [SerializeField] private float height = 15f;
    [SerializeField] private float lookAheadDistance = 4f;

    [Header("Smooth")]
    [SerializeField] private float positionSmoothTime = 0.15f;
    [SerializeField] private float directionSmoothSpeed = 5f;

    private Vector3 currentDirection;
    private Vector3 velocity;

    private void Start()
    {
        currentDirection = snakeHead.forward;
    }

    private void LateUpdate()
    {
        if (snakeHead == null) return;

        // Smoothly update direction only while moving
        if (snakeMovement.CurrentSpeed > 0.1f)
        {
            currentDirection = Vector3.Slerp(currentDirection, snakeHead.forward, directionSmoothSpeed * Time.deltaTime);

            currentDirection.y = 0f;
            currentDirection.Normalize();
        }

        // Camera target slightly ahead of snake
        Vector3 targetPosition = snakeHead.position + currentDirection * lookAheadDistance + Vector3.up * height;

        // Smooth movement
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, positionSmoothTime);

        // Fixed angle
        transform.rotation = Quaternion.Euler(60f, 0f, 0f);
    }
}