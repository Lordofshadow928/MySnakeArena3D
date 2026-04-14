using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.HableCurve;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 100f;
    public float steerSpeed = 180.0f;

    [Header("Body Settings")]
    public float bodySpeed = 5.0f;
    [SerializeField] private int gap = 5;

    [Header("Pooling")]
    public ObjectPool bodyPool;
    [Header("Tail Settings")]
    public GameObject tailPrefab;
    // Internal components
    [SerializeField] private List<Transform> segments = new List<Transform>();
    [SerializeField] private List<Vector3> directions = new List<Vector3>();
    [SerializeField] private float segmentDistance = 0.5f;
    private Transform tail;
    private List<GameObject> bodyParts = new List<GameObject>();
    private int growPending = 0;

    private void Start()
    {
        {
            segments.Clear();
            directions.Clear();

            // Head
            segments.Add(transform);
            directions.Add(transform.forward);

            // Spawn body
            for (int i = 0; i < 5; i++)
            {
                AddBody();
            }

            // Add tail LAST
            AddTail();
        }
    }



    private void Update()
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
        float steer = Input.GetAxis("Horizontal");

        //Smooth rotation
        Quaternion targetRot = transform.rotation * Quaternion.Euler(0, steer * steerSpeed * Time.deltaTime, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 180f * Time.deltaTime);

        //Forward movement
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }


    private void MoveSegments()
    {
        for (int i = 1; i < segments.Count; i++)
        {
            Transform prev = segments[i - 1];
            Transform current = segments[i];

            Vector3 dir = (prev.position - current.position).normalized;

            //Maintain distance
            current.position = prev.position - dir * segmentDistance;

            //Rotate forward
            if (dir.sqrMagnitude > 0.0001f)
            {
                current.rotation = Quaternion.LookRotation(dir);
            }
        }
    }

    private void GrowSnake()
    {
        GameObject body = bodyPool.GetObject();
        if (body == null) return;

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
    }

    private void AddBody()
    {
        GameObject body = bodyPool.GetObject();
        if (body == null) return;

        Transform last = segments[segments.Count - 1];

        // If tail exists insert before tail
        if (tail != null)
        {
            last = segments[segments.Count - 2];
        }

        body.transform.position = last.position - last.forward * segmentDistance;
        body.transform.rotation = last.rotation;
        body.SetActive(true);

        if (tail != null)
        {
            segments.Insert(segments.Count - 1, body.transform);
            directions.Insert(directions.Count - 1, last.forward);
        }
        else
        {
            segments.Add(body.transform);
            directions.Add(last.forward);
        }
    }

    private void AddTail()
    {
        Transform last = segments[segments.Count - 1];

        tail = Instantiate(tailPrefab,last.position - last.forward * segmentDistance,last.rotation).transform;

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


