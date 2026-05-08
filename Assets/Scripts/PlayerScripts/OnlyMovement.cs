using UnityEngine;

public class OnlyMovement : MonoBehaviour
{

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotateSpeed = 240f;

    private float defaultMoveSpeed;
    private float defaultRotateSpeed;

    private bool isBoosted;
    private Transform root;

    private void Start()
    {
        root = transform.parent;

        defaultMoveSpeed = moveSpeed;
        defaultRotateSpeed = rotateSpeed;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");

        root.Rotate(Vector3.up * horizontal * rotateSpeed * Time.fixedDeltaTime);

        root.position += transform.forward * moveSpeed * Time.fixedDeltaTime;
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

