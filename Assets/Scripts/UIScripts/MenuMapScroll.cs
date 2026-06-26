using UnityEngine;

public class MenuMapScroll : MonoBehaviour
{
    [SerializeField] private MenuProgressManager progressManager;
    [SerializeField] private SnakeUITeleport snakeTeleportEffect;
    [SerializeField] private MenuIsland[] islands;

    private int currentLevel;
    private int previousLevel;

    [SerializeField] float scrollSpeed = 0.1f;
    [SerializeField] float minZ = -5f;
    [SerializeField] float maxZ = 210f;
    [SerializeField] float smoothSpeed = 4f;

    [SerializeField] float stepSize = 15f;
    [SerializeField] float snapOffset = -5f;

    private Vector3 lastMousePos;
    private float targetZ;

    private void Start()
    {
        targetZ = transform.position.z;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePos = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - lastMousePos;

            targetZ -= delta.y * scrollSpeed;
            targetZ = Mathf.Clamp(targetZ, minZ, maxZ);

            lastMousePos = Input.mousePosition;
        }

        // Snap when releasing mouse
        if (Input.GetMouseButtonUp(0))
        {
            float level = Mathf.Round((targetZ - snapOffset) / stepSize);
            currentLevel = Mathf.Clamp((int)level, 0, progressManager.MaxReachableIndex);

            targetZ = currentLevel * stepSize + snapOffset;
            targetZ = Mathf.Clamp(targetZ, minZ, maxZ);

            // Only teleport if level changed
            if (currentLevel != previousLevel)
            {
                if(snakeTeleportEffect.TeleportTo(islands[currentLevel]))
                {
                    previousLevel = currentLevel;
                }
            }
        }

        Vector3 pos = transform.position;
        pos.z = Mathf.Lerp(pos.z, targetZ, Time.deltaTime * smoothSpeed);
        transform.position = pos;
    }
}