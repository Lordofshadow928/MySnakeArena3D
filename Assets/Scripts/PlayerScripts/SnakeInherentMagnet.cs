using System;
using System.Collections.Generic;
using UnityEngine;

public class SnakeInherentMagnet : MonoBehaviour
{
    [Header("Magnet Settings")]
    [SerializeField] private float magnetRadius = 2f;
    [SerializeField] private LayerMask foodLayer;
    [SerializeField] private Transform mouthPoint;    
    private Animator animator;

    // Track foods already magnetized
    [SerializeField]private List<FoodDemo> magnetFoods = new List<FoodDemo>();
    private FoodSpawner2 spawner;


    void Start()
    {
        animator = GetComponent<Animator>();
            spawner = FindObjectOfType<FoodSpawner2>();
            if (spawner == null)
            {
                Debug.LogError("SnakeInherentMagnet: No FoodSpawner2 found in scene!");
        }
    }

    void Update()
    {
        DetectFoods();
        UpdateEatingAnimation();

    }

    //Detect foods and tell them to move
    public void DetectFoods()
    {
       

        //Cleanup nulls (destroyed/disabled foods)
        for (int i = magnetFoods.Count - 1; i >= 0; i--)
        {
            if (magnetFoods[i] == null || !magnetFoods[i].gameObject.activeInHierarchy)
            {
                magnetFoods.RemoveAt(i);
            }
        }

        //CollectFood_Collider();
        CollectFoodDistance();
    }

    private void CollectFoodDistance()
    {
        var foods = spawner.GetFoodInRange(transform, magnetRadius);
        foreach (var food in foods)
        {
            FoodDemo foodDemo = food.GetComponent<FoodDemo>();
            if (foodDemo != null && !magnetFoods.Contains(foodDemo))
            {
                magnetFoods.Add(foodDemo);
                foodDemo.GetComponent<MeshRenderer>().material.color = Color.green; // Optional: visually indicate magnetized food
                //Tell food to move to mouth
                foodDemo.MoveToTarget(mouthPoint);
            }
        }
    }

    //private void CollectFood_Collider()
    //{
    //    Collider[] cols = Physics.OverlapSphere(transform.position, magnetRadius, foodLayer);

    //    foreach (var col in cols)
    //    {
    //        FoodDemo food = col.GetComponent<FoodDemo>();

    //        if (food != null && !magnetFoods.Contains(food))
    //        {
    //            magnetFoods.Add(food);
    //            food.GetComponent<MeshRenderer>().material.color = Color.green; // Optional: visually indicate magnetized food
    //            //Tell food to move to mouth
    //            food.MoveToTarget(mouthPoint);
    //        }
    //    }
    //} this function was an earlier attempt to detect food using colliders, but it caused issues with fast-moving food and collider interactions, so I switched to distance-based detection instead.

    // Step 2: control animation
    private void UpdateEatingAnimation()
    {
        animator.SetBool("isEating", magnetFoods.Count > 0);
    }

private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, magnetRadius);
    }
}
