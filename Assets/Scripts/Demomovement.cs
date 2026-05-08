using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demomovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotateSpeed = 240f;

    private bool isBoosted = false;
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

    public float GetMoveSpeed() => moveSpeed;
    public float GetHeadSpeed() => rotateSpeed;
    [Header("Pooling")]
    [SerializeField] private ObjectPool bodyPool;
    [Header("Tail Settings")]
    [SerializeField] private GameObject tailPrefab;
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

    private float defaultMoveSpeed;
    private float defaultRotateSpeed;
    private Transform root;
    private void Start()
    {
        Init();
        SpawnSnake();
    }

    private void Init()
    {
        defaultMoveSpeed = moveSpeed;
        defaultRotateSpeed = rotateSpeed;
        isBoosted = false;
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
        if (isBoosted)
        {
            var lastPos = positionHistory[0];
            positionHistory.Insert(0, (headPoint.position + lastPos) / 2);
        }
        positionHistory.Insert(0, headPoint.position);
        Moving();
    }

    private void Moving()
    {
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
        root.Rotate(Vector3.up * horizontal * rotateSpeed * deltaTime);
        root.position += transform.forward * moveSpeed * deltaTime;
        Debug.Log($"Head Position: {transform.position}");
    }


    private void MoveSegments()
    {
        for (int i = 1; i < segments.Count; i++)
        {
            Transform segment = segments[i];
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
        GameObject body = bodyPool.GetObject(Vector3.zero, Quaternion.identity);
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

    public void ShrinkSnake()
    {
        // Need at least head + 2 body + tail
        if (segments.Count <= 5) return;

        // Remove last body BEFORE tail
        int removeIndex = segments.Count - 2;

        Transform segmentToRemove = segments[removeIndex];

        segments.RemoveAt(removeIndex);
        directions.RemoveAt(removeIndex);

        // Return to pool instead of destroying
        segmentToRemove.gameObject.SetActive(false);
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
        if (LayerMask.LayerToName(other.gameObject.layer) == "Food")
        {
            growPending++;
            other.gameObject.SetActive(false);
            GetComponent<DemoMagnet4>();
        }
    }

}
