using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5.0f;
    public float steerSpeed = 180.0f;

    [Header("Body Settings")]
    public float bodySpeed = 5.0f;
    [SerializeField] private int gap = 5;

    [Header("Prefabs")]
    public GameObject bodyPrefab;
    public GameObject tailPrefab;

    // Internal components
    private List<GameObject> bodyParts = new List<GameObject>();
    [SerializeField]
    private List<Vector3> positionHistory = new List<Vector3>();

    private void Start()
    {
        Init();
    }


    [ContextMenu("Init")]
    private void Init()
    {
        PrefillPositionHistory();
        SpawnSnake();
        //Debug.LogError("Spawn");
    }

    private void SpawnSnake()
    {
        SpawnSnakesBody();
        // Add the tail
        AddTail();
    }

    private void SpawnSnakesBody()
    {
        // Add initial body parts
        for (int i = 0; i < 5; i++)
        {
            GrowSnake();
        }
    }

    private void PrefillPositionHistory()
    {
        // Pre-fill position history to avoid all body parts spawning on top of each other
        for (int i = 0; i < 100; i++)
        {
            positionHistory.Add(transform.position - transform.forward * 0.1f * i);
        }
    }

    private void FixedUpdate()
    {
        MoveSnake();
    }

    private void MoveSnake()
    {
        MoveHead();
        RecordHeadPosition();
        PrunePositionHistory();
        MoveBodyParts();
    }

    private void MoveHead()
    {
        // Move the head forward
        transform.position += transform.forward * moveSpeed * Time.deltaTime;

        // Turn based on input
        float steerDirection = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up * steerDirection * steerSpeed * Time.deltaTime);
    }

    private void RecordHeadPosition()
    {
        // Record head position
        positionHistory.Insert(0, transform.position);
    }

    private void PrunePositionHistory()
    {
        // Limit history to prevent memory overflow
        int maxHistory = (bodyParts.Count + 1) * gap;
        if (positionHistory.Count > maxHistory)
        {
            positionHistory.RemoveAt(positionHistory.Count - 1);
        }
    }

    private void MoveBodyParts()
    {
        // Move each body part
        for (int i = 0; i < bodyParts.Count; i++)
        {
            int index = Mathf.Min(i * gap, positionHistory.Count - 1);
            Vector3 targetPosition = positionHistory[index];
            GameObject part = bodyParts[i];

            Vector3 direction = targetPosition - part.transform.position;
            part.transform.position += direction * bodySpeed * Time.deltaTime;
            if (direction != Vector3.zero)
            {
                part.transform.rotation = Quaternion.LookRotation(direction);
            }
        }
    }

    private void GrowSnake()
    {
        Vector3 spawnPosition = bodyParts.Count == 0
            ? transform.position - transform.forward * 1.0f
            : bodyParts[bodyParts.Count - 1].transform.position - bodyParts[bodyParts.Count - 1].transform.forward * 1.0f;

        GameObject body = Instantiate(bodyPrefab, spawnPosition, Quaternion.identity);
        bodyParts.Add(body);
    }

    private void AddTail()
    {
        Vector3 spawnPosition = bodyParts.Count == 0
            ? transform.position - transform.forward * 1.0f
            : bodyParts[bodyParts.Count - 1].transform.position - bodyParts[bodyParts.Count - 1].transform.forward * 1.0f;

        GameObject tail = Instantiate(tailPrefab, spawnPosition, Quaternion.identity);
        bodyParts.Add(tail);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Food"))
        {
            GrowSnake();
            other.gameObject.SetActive(false);
        }
            
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (var pos in positionHistory)
        {
            Gizmos.DrawSphere(pos, 0.05f);
        }
    }
}
