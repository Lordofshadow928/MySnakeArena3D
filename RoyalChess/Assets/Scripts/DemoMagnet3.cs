using System.Collections.Generic;
using UnityEngine;

public class DemoMagnet3 : MonoBehaviour
{
    [Header("Magnet Settings")]
    [SerializeField] private float magnetRadius = 5f;
    [SerializeField] private float pullForce = 20f;
    [SerializeField] private float eatDistance = 0.4f;
    [SerializeField] private LayerMask foodLayer;
    [SerializeField] private Transform mouthPoint;

    private Animator animator;

    // Track captured foods so they keep flying to mouth
    private List<Rigidbody> magnetFoods = new List<Rigidbody>();

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        
        CaptureNewFoods();
        PullCapturedFoods();
    }

    // Step 1: detect foods and lock them
    private void CaptureNewFoods()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, magnetRadius, foodLayer);

        foreach (var col in cols)
        {
            Rigidbody rb = col.attachedRigidbody;
            if (rb != null && !magnetFoods.Contains(rb))
            {
                magnetFoods.Add(rb);
            }
        }
    }

    // Step 2: pull ALL captured foods to mouth (even outside radius)
    private void PullCapturedFoods()
    {
        if (magnetFoods.Count == 0)
        {
            animator.SetBool("isEating", false);
            return;
        }

        animator.SetBool("isEating", true);

        Vector3 target = mouthPoint != null ? mouthPoint.position : transform.position;

        for (int i = magnetFoods.Count - 1; i >= 0; i--)
        {
            Rigidbody rb = magnetFoods[i];

            if (rb == null)
            {
                magnetFoods.RemoveAt(i);
                continue;
            }

            Vector3 direction = target - rb.position;
            float distance = direction.magnitude;

            // Step 3: when close to eat
            if (distance <= eatDistance)
            {
                rb.position = target;

                // If using object pooling:
                rb.gameObject.SetActive(false);

                // If NOT using pooling, use:
                // Destroy(rb.gameObject);

                magnetFoods.RemoveAt(i);
                continue;
            }

            // Step 4: pull toward mouth
            float forceAmount = pullForce / Mathf.Max(distance, 0.5f);

            rb.AddForce(direction.normalized * forceAmount, ForceMode.Acceleration);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, magnetRadius);
    }
}