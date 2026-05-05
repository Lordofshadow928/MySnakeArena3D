using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FoodMagnet : MonoBehaviour
{
    float magnetRadius = 2f;
    float magnetForce = 7f;
    float magnetDuration = 3f;
    bool isMagnetActive = false;
    [SerializeField] private Transform mouthPoint;

    private Animator animator;
    public LayerMask foodLayer;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

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

   
    void Update()
    {
        if (!isMagnetActive) return;

        Collider[] foods = Physics.OverlapSphere(mouthPoint.position, magnetRadius, foodLayer);

        foreach (Collider col in foods)
        {
            
            Transform food = col.transform;

            Vector3 direction = (transform.position - food.position);
            float distance = direction.magnitude;

            distance = Mathf.Max(distance, 0.5f);

            //Force stronger when closer
            float force = magnetForce / distance;

            // Smooth movement
            food.position = Vector3.Lerp(
                food.position,
                transform.position,
                force * Time.deltaTime
            );
        }
    }

    // Call this when snake eats food
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

    // Visualize magnet range in editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        if (mouthPoint != null)
        {
            Gizmos.DrawWireSphere(mouthPoint.position, magnetRadius);
        }
    }
}
