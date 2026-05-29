using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISnakeBrain : MonoBehaviour
{
    [SerializeField] private Transform target;

    private OnlyMovement movement;

    private void Awake()
    {
        movement = GetComponent<OnlyMovement>();
    }

    private void Update()
    {
        if (target == null)
            return;

        Vector3 directionToTarget =
            (target.position - transform.position).normalized;

        float angle =
            Vector3.SignedAngle(transform.forward, directionToTarget, Vector3.up);

        movement.SteeringInput =
            Mathf.Clamp(angle / 45f, -1f, 1f);
    }
}
