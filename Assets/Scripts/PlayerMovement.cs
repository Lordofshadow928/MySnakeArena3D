using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float steerSpeed = 180f;

    [Header("Body Settings")]
    public float followSpeed = 10f;
    [SerializeField] private int gap = 20;

    [Header("Pooling")]
    public ObjectPool bodyPool;

    [Header("Tail")]
    public GameObject tailPrefab;

    private List<GameObject> bodyParts = new List<GameObject>();
    private List<Vector3> positionHistory = new List<Vector3>();

    private int growPending = 0;
    private float minDistance = 0.001f;

    private GameObject tail;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        positionHistory.Clear();
        bodyParts.Clear();
        PrefillHistory();
        SpawnInitialBody();
    }

    private void PrefillHistory()
    {
        for (int i = 0; i < 300; i++)
        {
            positionHistory.Add(transform.position - transform.forward * i * 0.1f);
        }
    }

    private void SpawnInitialBody()
    {
        for (int i = 0; i < 3; i++)
        {
            GrowSnake();
        }

        AddTail();
    }

    private void Update()
    {
        if (growPending > 0)
        {
            GrowSnake();
            growPending--;
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        MoveHead();
        RecordPosition();
        TrimHistory();
        MoveBody();
        MoveTail();
    }

    // ================= HEAD =================
    private void MoveHead()
    {
        float steer = Input.GetAxis("Horizontal");

        // Smooth & consistent rotation
        transform.Rotate(Vector3.up * steer * steerSpeed * Time.deltaTime);

        // Forward move
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }

    // ================= HISTORY =================
    private void RecordPosition()
    {
        if (positionHistory.Count == 0 ||
            Vector3.Distance(positionHistory[0], transform.position) > minDistance)
        {
            positionHistory.Insert(0, transform.position);
        }
    }

    private void TrimHistory()
    {
        int max = (bodyParts.Count + 5) * gap;

        if (positionHistory.Count > max)
        {
            positionHistory.RemoveRange(max, positionHistory.Count - max);
        }
    }

    // ================= BODY =================
    //private void MoveBody()
    //{
    //    for (int i = 0; i < bodyParts.Count; i++)
    //    {
    //        float index = i * gap;

    //        if (index >= positionHistory.Count - 1) continue;

    //        int indexA = Mathf.FloorToInt(index);
    //        int indexB = indexA + 1;
    //        float t = index - indexA;

    //        Vector3 pointA = positionHistory[indexA];
    //        Vector3 pointB = positionHistory[indexB];

    //        //HARD LOCK to path (NO LERP FOLLOW)
    //        Vector3 targetPos = Vector3.Lerp(pointA, pointB, t);

    //        Transform part = bodyParts[i].transform;

    //        part.position = targetPos;

    //        Vector3 dir = pointB - pointA;

    //        if (dir.sqrMagnitude > 0.0001f)
    //        {
    //            part.rotation = Quaternion.LookRotation(dir);
    //        }
    //    }
    //}
    private void MoveBody()
    {
        for (int i = 0; i < bodyParts.Count; i++)
        {
            float index = i * gap;

            int indexA = Mathf.FloorToInt(index);
            int indexB = Mathf.Min(indexA + 1, positionHistory.Count - 1);
            float t = index - indexA;

            if (indexA >= positionHistory.Count) continue;

            Vector3 posA = positionHistory[indexA];
            Vector3 posB = positionHistory[indexB];

            Vector3 targetPos = Vector3.Lerp(posA, posB, t);

            Transform part = bodyParts[i].transform;

            Vector3 dir = targetPos - part.position;

            /*float followSpeed = bodySpeed * (1f + i * 0.05f);*/ // tail looser

            part.position += dir * followSpeed * Time.deltaTime;

            if (dir.sqrMagnitude > 0.0001f)
            {
                part.rotation = Quaternion.Slerp(
                    part.rotation,
                    Quaternion.LookRotation(dir),
                    10f * Time.deltaTime
                );
            }
        }
    }

    // ================= TAIL =================
    private void AddTail()
    {
        tail = Instantiate(tailPrefab, GetTailPosition(), Quaternion.identity);
    }

    private void MoveTail()
    {
        if (tail == null) return;

        float index = bodyParts.Count * gap;

        if (index >= positionHistory.Count - 1) return;

        int indexA = Mathf.FloorToInt(index);
        int indexB = indexA + 1;
        float t = index - indexA;

        Vector3 pos = Vector3.Lerp(positionHistory[indexA], positionHistory[indexB], t);

        tail.transform.position = Vector3.Lerp(tail.transform.position, pos, followSpeed * Time.deltaTime);

        Vector3 dir = pos - tail.transform.position;

        if (dir.sqrMagnitude > 0.001f)
        {
            Quaternion rot = Quaternion.LookRotation(dir);
            tail.transform.rotation = Quaternion.Slerp(tail.transform.rotation, rot, 30f * Time.deltaTime);
        }
    }

    private Vector3 GetTailPosition()
    {
        int index = Mathf.Clamp(bodyParts.Count * gap, 0, positionHistory.Count - 1);
        return positionHistory[index];
    }

    // ================= GROW =================
    private void GrowSnake()
    {
        GameObject body = bodyPool.GetObject();
        if (body == null) return;

        int index = Mathf.Clamp(bodyParts.Count * gap, 0, positionHistory.Count - 1);

        body.transform.position = positionHistory[index];
        body.transform.rotation = Quaternion.identity;
        body.SetActive(true);

        bodyParts.Add(body);
    }

    // ================= COLLISION =================
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            growPending++;
            other.gameObject.SetActive(false);
        }
    }
}

