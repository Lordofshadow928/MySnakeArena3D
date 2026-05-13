using UnityEngine;
using Lean.Pool;

public class FoodDemo : MonoBehaviour, IPoolable
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 12f;
    [SerializeField] private float maxSpeed = 15f;
    [SerializeField] private float eatDistance = 0.3f;

    private Rigidbody rb;
    private Transform target;
    private bool isMovingToTarget = false;
    private FoodSpawner2 foodSpawner;
    

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        foodSpawner  = FindObjectOfType<FoodSpawner2>();
    }

    void FixedUpdate()
    {
        if (!isMovingToTarget || target == null) return;
        MoveToTarget();
    }

    //Called by magnet
    public void MoveToTarget(Transform targetTransform)
    {
        target = targetTransform;
        isMovingToTarget = true;
    }

    private void MoveToTarget()
    {
        Vector3 direction = target.position - rb.position;
        float distance = direction.magnitude;

        //Eat when close enough
        if (distance <= eatDistance)
        {
            transform.position = target.position;
            foodSpawner.OnFoodReturn(gameObject); // Return to pool
            return;
        }

        //Normalize direction
        direction.Normalize();

        //Slow down when very close (smooth stop)
        float speed = moveSpeed;

        if (distance < 1.5f)
        {
            speed *= (distance / 1.5f); // smooth slow near mouth
        }

        //Set velocity directly
        rb.velocity = direction * speed;

        //Clamp just in case
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    public void OnSpawn()
    {
        ResetFood();
    }

    public void OnDespawn()
    {
        ResetFood();
    }

    private void ResetFood()
    {
        isMovingToTarget = false;
        target = null;

        MeshRenderer renderer = GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.white; // Reset color
        }
    }
}