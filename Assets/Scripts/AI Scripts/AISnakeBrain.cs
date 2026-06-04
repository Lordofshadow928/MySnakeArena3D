using UnityEngine;

public class AISnakeBrain : MonoBehaviour
{
    
    [SerializeField] private FoodSpawner2 spawner;
    [SerializeField] private float steeringSensitivity = 90f;
    [SerializeField] private float steeringSmoothness = 4f;

    private SnakeMovement movement;

    private void Awake()
    {
        movement = GetComponentInParent<SnakeMovement>();
    }

    private void FixedUpdate()
    {
        Transform target = spawner.GetClosestFood(transform.position);
        if (target == null)
        {
            movement.SteeringInput = 0f;
            return;
        }

        Vector3 directionToTarget = (target.position - transform.position).normalized;

        float angle = Vector3.SignedAngle(transform.forward, directionToTarget, Vector3.up);

        float targetSteering = Mathf.Clamp(angle / steeringSensitivity, -1f, 1f);

        movement.SteeringInput = Mathf.Lerp(movement.SteeringInput, targetSteering, steeringSmoothness * Time.fixedDeltaTime);
    }

    //public void SetTarget(Transform newTarget)
    //{
    //    target = newTarget;
    //}
}
