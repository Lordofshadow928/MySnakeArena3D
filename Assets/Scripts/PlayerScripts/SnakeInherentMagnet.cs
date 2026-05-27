using System;
using System.Collections.Generic;
using UnityEngine;

public class SnakeInherentMagnet : MonoBehaviour
{
    [Header("Magnet Settings")]
    [SerializeField] private float magnetRadius = 2f;
    [SerializeField] private LayerMask foodLayer;
    [SerializeField] private Transform mouthPoint;
    [SerializeField] private Animator animator;

    [SerializeField] private List<FoodDemo> magnetFoods = new List<FoodDemo>();
    private FoodSpawner2 spawner;

    void Start()
    {
        spawner = FindObjectOfType<FoodSpawner2>();
        if (spawner == null)
            Debug.LogError("SnakeInherentMagnet: No FoodSpawner2 found in scene!");

        if (mouthPoint == null)
            mouthPoint = transform; // fallback; better assign in inspector
    }

    // Use FixedUpdate so physics state is up-to-date when reading overlaps
    void FixedUpdate()
    {
        DetectFoods();
    }

    void Update()
    {
        UpdateEatingAnimation(); // animation can remain in Update
    }

    public void DetectFoods()
    {
        // Cleanup nulls
        for (int i = magnetFoods.Count - 1; i >= 0; i--)
        {
            if (magnetFoods[i] == null || !magnetFoods[i].gameObject.activeInHierarchy)
                magnetFoods.RemoveAt(i);
        }

        CollectFood_Collider();
    }

    private void CollectFood_Collider()
    {
        if (mouthPoint == null) return;

        // Ensure transforms are synchronized with physics if you must call from Update:
        // Physics.SyncTransforms();

        // Use mouthPoint.position and include triggers (food often uses trigger colliders)
        Collider[] cols = Physics.OverlapSphere(mouthPoint.position, magnetRadius, foodLayer, QueryTriggerInteraction.Collide);

        if (cols.Length == 0)
        {
            // Optional: debug to confirm why nothing is found
            // Debug.Log("Magnet detected 0 colliders. mouthPoint pos: " + mouthPoint.position);
        }

        foreach (var col in cols)
        {
            FoodDemo food = col.GetComponent<FoodDemo>();
            if (food != null && !magnetFoods.Contains(food))
            {
                magnetFoods.Add(food);

                var mr = food.GetComponent<MeshRenderer>();
                if (mr != null) mr.material.color = Color.green;

                food.MoveToTarget(mouthPoint);
            }
        }
    }

    private void UpdateEatingAnimation()
    {
        animator?.SetBool("isEating", magnetFoods.Count > 0);
    }

    public void RemoveMagnetFood(FoodDemo food)
    {
        if (food == null) return;
        magnetFoods.Remove(food);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        if (mouthPoint != null) Gizmos.DrawWireSphere(mouthPoint.position, magnetRadius);
        else Gizmos.DrawWireSphere(transform.position, magnetRadius);
    }
}
