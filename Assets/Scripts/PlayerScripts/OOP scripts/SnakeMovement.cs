using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float rotateSpeed = 240f;

    private float defaultMoveSpeed;
    private float defaultRotateSpeed;

    private Rigidbody rb;
    public float SteeringInput { get; set; }
    public float CurrentSpeed { get; private set; }
    public float MoveSpeed { get => moveSpeed; private set => moveSpeed = value; }

    private Vector3 lastPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        defaultMoveSpeed = MoveSpeed;
        defaultRotateSpeed = rotateSpeed;
    }

    private void FixedUpdate()
    {
        Move();
        CurrentSpeed = Vector3.Distance(transform.position, lastPosition) / Time.fixedDeltaTime;
        lastPosition = transform.position;
    }

    private void Move()
    {
        Quaternion deltaRotation = Quaternion.Euler(Vector3.up * SteeringInput * rotateSpeed * Time.fixedDeltaTime);
        rb.MoveRotation(rb.rotation * deltaRotation);
        Vector3 moveDirection = transform.forward * MoveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + moveDirection);
    }

    public void BoostSpeed(float multiplier)
    {
        MoveSpeed = defaultMoveSpeed * multiplier;
        rotateSpeed = defaultRotateSpeed * multiplier;
    }

    public void ResetSpeed()
    {
        MoveSpeed = defaultMoveSpeed;
        rotateSpeed = defaultRotateSpeed;
    }
}
