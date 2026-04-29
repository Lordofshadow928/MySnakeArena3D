using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoMagnet2 : MonoBehaviour
{
    [Header("Magnet Settings")]
    public float magnetRadius = 2f;
    public float magnetForce = 7f;
    public float eatDistance = 0.5f;
    public float magnetDuration = 3f;

    private bool isMagnetActive = false;

    public LayerMask foodLayer;
    [SerializeField] private Transform mouthPoint;

    private Animator animator;

    // Foods already captured by magnet
    private List<Transform> magnetFoods = new List<Transform>();

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OnEatFood()
    {
        ActivateMagnet();
    }

    public void ActivateMagnet()
    {
        StopAllCoroutines();
        StartCoroutine(MagnetCoroutine());
    }

    IEnumerator MagnetCoroutine()
    {
        isMagnetActive = true;

        yield return new WaitForSeconds(magnetDuration);

        isMagnetActive = false;
        magnetFoods.Clear();
        animator.SetBool("isEating", false);
    }

    void Update()
    {
        if (!isMagnetActive) return;

        Vector3 targetPos = mouthPoint != null ? mouthPoint.position : transform.position;

        // Detect new foods inside radius
        Collider[] foods = Physics.OverlapSphere(transform.position, magnetRadius, foodLayer);

        foreach (Collider col in foods)
        {
            if (!magnetFoods.Contains(col.transform))
            {
                magnetFoods.Add(col.transform);
            }
        }

        bool hasFood = magnetFoods.Count > 0;
        animator.SetBool("isEating", hasFood);

        // Pull all captured foods
        for (int i = magnetFoods.Count - 1; i >= 0; i--)
        {
            Transform food = magnetFoods[i];

            if (food == null)
            {
                magnetFoods.RemoveAt(i);
                continue;
            }

            Vector3 direction = targetPos - food.position;
            float distance = direction.magnitude;

            // Eat food
            if (distance <= eatDistance)
            {
                food.position = targetPos;
                food.gameObject.SetActive(false);
                magnetFoods.RemoveAt(i);
                continue;
            }

            // Continue pulling even outside radius
            float force = magnetForce / Mathf.Max(distance, 0.5f);
            food.position += direction.normalized * force * Time.deltaTime;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, magnetRadius);
    }
}

