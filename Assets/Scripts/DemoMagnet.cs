using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoMagnet : MonoBehaviour
{
    [Header("Magnet Settings")]
    public float magnetRadius = 2f;
    public float magnetForce = 7f;
    public float eatDistance = 0.5f;
    public float magnetDuration = 3f;
    bool isMagnetActive = false;

    public LayerMask foodLayer;
    [SerializeField] private Transform mouthPoint;

    private Animator animator;

    public void OnEatFood()
    {
        StartCoroutine(MagnetEffect());
    }

    IEnumerator MagnetEffect()
    {
        isMagnetActive = true;
        yield return new WaitForSeconds(magnetDuration);
        isMagnetActive = false;
    }
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isMagnetActive) return;
        Collider[] foods = Physics.OverlapSphere(transform.position, magnetRadius, foodLayer);

        bool hasFoodInRange = foods.Length > 0;

        //Control animation based on food presence
        animator.SetBool("isEating", hasFoodInRange);

        foreach (Collider col in foods)
        {
            Transform food = col.transform;

            Vector3 targetPos = mouthPoint != null ? mouthPoint.position : transform.position;

            Vector3 direction = targetPos - food.position;
            float distance = direction.magnitude;

            //If close enough then consume
            if (distance <= eatDistance)
            {
                food.position = targetPos;
                Destroy(food.gameObject);
                continue;
            }

            //Pull food
            float force = magnetForce / Mathf.Max(distance, 0.5f);
            food.position += direction.normalized * force * Time.deltaTime;
        }
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
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, magnetRadius);
    }
}