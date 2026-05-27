using UnityEngine;

public class OnlyMovement : MonoBehaviour
{

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float rotateSpeed = 240f;

    private float defaultMoveSpeed;
    private float defaultRotateSpeed;

    private bool isBoosted;
    private Rigidbody rb;
    private float horizontalInput;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        defaultMoveSpeed = moveSpeed;
        defaultRotateSpeed = rotateSpeed;
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Quaternion deltaRotation = Quaternion.Euler(Vector3.up * horizontalInput * rotateSpeed * Time.fixedDeltaTime);

        rb.MoveRotation(rb.rotation * deltaRotation);

        Vector3 moveDirection = transform.forward * moveSpeed * Time.fixedDeltaTime;

        rb.MovePosition(rb.position + moveDirection);
    }

    public void BoostSpeed(float multiplier)
    {
        moveSpeed = defaultMoveSpeed * multiplier;
        rotateSpeed = defaultRotateSpeed * multiplier;
        isBoosted = true;
    }

    public void ResetSpeed()
    {
        moveSpeed = defaultMoveSpeed;
        rotateSpeed = defaultRotateSpeed;
        isBoosted = false;
    }

    public bool IsBoosted()
    {
        return isBoosted;
    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }
}

