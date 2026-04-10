using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5.0f;
    public float steerSpeed = 360.0f;

    [Header("Body Settings")]
    public float bodySpeed = 8.0f;
    [SerializeField] private int gap = 8;

    [Header("Pooling")]
    public ObjectPool bodyPool;
    [Header("Tail Settings")]
    public GameObject tailPrefab;
    // Internal components
    private List<GameObject> bodyParts = new List<GameObject>();
    [SerializeField]
    private List<Vector3> positionHistory = new List<Vector3>();
    private int growPending = 0;

    private void Start()
    {
        Init();
    }


    [ContextMenu("Init")]
    private void Init()
    {
        positionHistory.Clear();
        bodyParts.Clear();
        PrefillPositionHistory();
        SpawnSnakesBody();
    }

  

    private void SpawnSnakesBody()
    {
        // Spawn initial body parts
        for (int i = 0; i < 5; i++)
        {
            GrowSnake();
        }
        AddTail();
    }

    private void PrefillPositionHistory()
    {
        // Pre-fill position history to avoid all body parts spawning on top of each other
        for (int i = 0; i < 200; i++)
        {
            positionHistory.Add(transform.position - transform.forward * 0.1f * i);
        }
    }

    private void Update()
    {
        // Handle growth if pending
        if (growPending > 0)
        {
            GrowSnake();
            growPending--;
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
        float steer = Input.GetAxis("Horizontal");

        //Smooth rotation
        Quaternion targetRot = transform.rotation * Quaternion.Euler(0, steer * steerSpeed * Time.deltaTime, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 180f * Time.deltaTime);

        //Forward movement
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }

    private float minDistance = 0.05f;
    private void RecordHeadPosition()
    {
        if (positionHistory.Count == 0 ||
            Vector3.Distance(positionHistory[0], transform.position) > minDistance)
        {
            positionHistory.Insert(0, transform.position);
        }
    }

    private void PrunePositionHistory()
    {
        // Limit history to prevent memory overflow
        int maxHistory = (bodyParts.Count + 1) * gap;
        if (positionHistory.Count > maxHistory)
        {
            positionHistory.RemoveRange(maxHistory,positionHistory.Count - maxHistory);
        }
    }

    private void MoveBodyParts()
    {
        for (int i = 0; i < bodyParts.Count; i++)
        {
            float index = i * gap;

            int indexA = Mathf.FloorToInt(index);
            int indexB = Mathf.Min(indexA + 1, positionHistory.Count - 1);
            float t = index - indexA;

            if (indexA >= positionHistory.Count) continue;

            Vector3 pointA = positionHistory[indexA];
            Vector3 pointB = positionHistory[indexB];

            //Smooth interpolation
            Vector3 targetPos = Vector3.Lerp(pointA, pointB, t);

            Transform part = bodyParts[i].transform;

            Vector3 dir = targetPos - part.position;

            //Smooth follow
            float followSpeed = bodySpeed * (1f + i * 0.05f);
            part.position += dir * followSpeed * Time.deltaTime;

            if (dir.sqrMagnitude > 0.0001f)
            {
                Quaternion targetRot = Quaternion.LookRotation(dir);
                part.rotation = Quaternion.Slerp(part.rotation, targetRot, 10f * Time.deltaTime);
            }
        }
    }

    private void GrowSnake()
    {
        GameObject body = bodyPool.GetObject();
        if (body == null) return;

        body.transform.position = GetPreTailPosition();
        body.transform.rotation = Quaternion.identity;
        body.SetActive(true);

        if (bodyParts.Count > 0)
        {
            bodyParts.Insert(bodyParts.Count - 1, body);
        }
        else
        {
            bodyParts.Add(body);
        }
    }
    private Vector3 GetPreTailPosition()
    {
        if (positionHistory == null || positionHistory.Count == 0)
            return transform.position;

        int index = Mathf.Clamp((bodyParts.Count - 1) * gap,0,positionHistory.Count - 1);
        return positionHistory[index];
    }
    private void AddTail()
    {
        tailPrefab = Instantiate(tailPrefab, GetLastPosition(), Quaternion.identity);
        bodyParts.Add(tailPrefab);
    }

    private Vector3 GetLastPosition()
    {
        if (bodyParts.Count == 0)
            return transform.position - transform.forward;

        Transform last = bodyParts[bodyParts.Count - 1].transform;
        return last.position - last.forward;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            growPending++;
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

