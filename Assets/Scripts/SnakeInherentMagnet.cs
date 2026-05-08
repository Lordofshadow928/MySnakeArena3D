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
    private List<FoodDemo> magnetFoods = new List<FoodDemo>();

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        DetectFoods();
        UpdateEatingAnimation();
    }

    //Detect foods and tell them to move
    private void DetectFoods()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, magnetRadius, foodLayer);

        foreach (var col in cols)
        {
            FoodDemo food = col.GetComponent<FoodDemo>();

            if (food != null && !magnetFoods.Contains(food))
            {
                magnetFoods.Add(food);

                //Tell food to move to mouth
                food.MoveToTarget(mouthPoint);
            }
        }

        //Cleanup nulls (destroyed/disabled foods)
        for (int i = magnetFoods.Count - 1; i >= 0; i--)
        {
            if (magnetFoods[i] == null || !magnetFoods[i].gameObject.activeInHierarchy)
            {
                magnetFoods.RemoveAt(i);
            }
        }
    }

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
