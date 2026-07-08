using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float smoothSpeed = 5f;
    private Transform target;
    private Vector3 offset;

    public void Initialize(Transform player)
    {
        target = player;
        offset = transform.position - target.position;
    }

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 targetPosition = target.position + offset;

        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }
}