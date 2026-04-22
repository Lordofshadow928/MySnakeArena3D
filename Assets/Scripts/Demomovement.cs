using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.Rendering.HableCurve;

public class Demomovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float steerSpeed = 180.0f;
    public float headSpeed = 240f;
    [Header("Pooling")]
    public ObjectPool bodyPool;
    [Header("Tail Settings")]
    public GameObject tailPrefab;
    // Internal components
    [SerializeField] private List<Vector3> positionHistory = new List<Vector3>();
    [SerializeField] private List<Transform> segments = new List<Transform>();
    [SerializeField] private List<Vector3> directions = new List<Vector3>();
    [SerializeField] private float segmentDistance = 0.5f;
    [SerializeField] private float segmentLenght = 1;
    [SerializeField] private float distanceBetweenPoints = 0.15f;
    private Transform tail;
    private int growPending = 0;
    private float deltaTime;
    [SerializeField] private int preHisotry = 15;
    [SerializeField] private Transform headPoint;

    private Transform root;
    private void Start()
    {
        SpawnSnake();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for (int i = 0; i < positionHistory.Count; i++)
        {
            Gizmos.DrawSphere(positionHistory[i], 0.05f);
        }
    }

    private void SpawnSnake()
    {
        CleanLists();
        // Head
        InitHead();

        // Spawn body
        InitBody();

        // Add tail LAST
        AddTail();
    }

    private void InitBody()
    {
        for (int i = 0; i < 3; i++)
        {
            GrowSnake();
        }
    }

    private void InitHead()
    {
        root = transform.parent;
        segments.Add(transform);
        directions.Add(transform.forward);
    }

    private void CleanLists()
    {
        segments.Clear();
        directions.Clear();
        positionHistory.Clear();
        deltaTime = Time.fixedDeltaTime;

        for (int i = 0; i < preHisotry; i++)
        {
            var movement = headPoint.forward * deltaTime * moveSpeed * i;
            positionHistory.Add(headPoint.position - movement);
        }
    }

    private void FixedUpdate()
    {

        positionHistory.Insert(0, headPoint.position);
        MoveHead();
        MoveSegments();
        if (growPending > 0)
        {
            GrowSnake();

            growPending--;
        }
    }
    private void MoveHead()
    {
        float horizontal = Input.GetAxis("Horizontal");
        root.Rotate(Vector3.up * horizontal * headSpeed * deltaTime);
        root.position += transform.forward * moveSpeed * deltaTime;
        Debug.Log($"Head Position: {transform.position}");
    }

    private void MoveSegments()
    {
        for (int i = 1; i < segments.Count; i++)
        {
            Transform segment = segments[i];
            //Transform current = segments[i];
            //Transform target = (i == 0) ? transform : segments[i - 1];

            int index = Mathf.RoundToInt(i * segmentLenght / distanceBetweenPoints);
            if (index < positionHistory.Count)
            {
                Transform current = segments[i];
                Transform target = (i == 0) ? transform : segments[i - 1];

                Vector3 targetPos = positionHistory[index];

                Vector3 moveDir = targetPos - segment.position;

                segment.position = Vector3.Lerp(
                    segment.position,
                    targetPos,
                    moveSpeed * deltaTime
                );

                current.LookAt(target);
            }
            Debug.Log($"Segment {i}: Position = {segment.position}, Target Index = {index}");
        }

        int maxHistory = segments.Count * Mathf.RoundToInt(segmentLenght / distanceBetweenPoints) + 10;
        if (positionHistory.Count > maxHistory)
        {
            positionHistory.RemoveAt(positionHistory.Count - 1);
        }
        //Debug.Log($"Max History = {maxHistory}, Current History Count = {positionHistory.Count}");
    }

    private GameObject GrowSnake()
    {
        GameObject body = bodyPool.GetObject();
        if (body == null) return null;

        //If tail exists last body is BEFORE tail
        Transform last = (tail != null)
            ? segments[segments.Count - 2]
            : segments[segments.Count - 1];

        body.transform.position = last.position - last.forward * segmentDistance;
        body.transform.rotation = last.rotation;
        body.SetActive(true);

        if (tail != null)
        {
            //insert BEFORE tail
            segments.Insert(segments.Count - 1, body.transform);
            directions.Insert(directions.Count - 1, last.forward);
        }
        else
        {
            segments.Add(body.transform);
            directions.Add(last.forward);
        }
        return body;
    }

    private void AddTail()
    {
        Transform last = segments[segments.Count - 1];

        tail = Instantiate(tailPrefab, last.position - last.forward * segmentDistance, last.rotation).transform;

        segments.Add(tail);
        directions.Add(last.forward);
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            growPending++;
            other.gameObject.SetActive(false);
        }

    }



}
