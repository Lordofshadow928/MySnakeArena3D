using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.HableCurve;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 10f;
    public float steerSpeed = 180.0f;
    public float headSpeed = 60f;
    [Header("Pooling")]
    public ObjectPool bodyPool;
    [Header("Tail Settings")]
    public GameObject tailPrefab;
    // Internal components
    [SerializeField] private List<Transform> segments = new List<Transform>();
    [SerializeField] private List<Vector3> directions = new List<Vector3>();
    [SerializeField] private float segmentDistance = 0.5f;
    [SerializeField] private float segmentLenght = 1;
    [SerializeField] private float distanceBetweenPoints = 0.2f;
    private Transform tail;
    private int growPending = 0;
    private float deltaTime;

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

                GrowSnake();
            }

            // Add tail LAST
            AddTail();
        }
    }



    private void FixedUpdate()
    {
        deltaTime = Time.fixedDeltaTime;
        MoveHead();
        MoveSegments();
        if (growPending > 0)
        {
            GrowSnake();
            
            growPending--;
        }
    }

    //private void MoveHead()
    //{
    //    float steer = Input.GetAxis("Horizontal");

    //    //Smooth rotation
    //    Quaternion targetRot = transform.rotation * Quaternion.Euler(0, steer * steerSpeed * Time.deltaTime, 0);
    //    transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 180f * Time.deltaTime);

    //    //Forward movement
    //    transform.position += transform.forward * moveSpeed * Time.deltaTime;
    //}
    private void MoveHead()
    {
        float horizontal = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up * horizontal * headSpeed * deltaTime);
        transform.position += transform.forward * moveSpeed * deltaTime;
    }


    //private void MoveSegments()
    //{
    //    for (int i = 1; i < segments.Count; i++)
    //    {
    //        Transform prev = segments[i - 1];
    //        Transform current = segments[i];

    //        Vector3 dir = (prev.position - current.position).normalized;

    //        ////Maintain distance
    //        current.position = prev.position - dir * segmentDistance;

    //        //Rotate forward
    //        if (dir.sqrMagnitude > 0.001f)
    //        {
    //            current.rotation = Quaternion.LookRotation(dir);
    //        }
    //    }
    //}


    //private void MoveSegments()
    //{
    //    for (int i = 1; i < segments.Count; i++)
    //    {
    //        Transform prev = segments[i - 1];
    //        Transform current = segments[i];

    //        Vector3 targetPos = prev.position - prev.forward * segmentDistance;

    //        // Move smoothly instead of snapping
    //        current.position = Vector3.MoveTowards(
    //            current.position,
    //            targetPos,
    //            moveSpeed * Time.deltaTime
    //        );

    //        // Rotate toward movement direction
    //        Vector3 dir = (prev.position - current.position);
    //        if (dir.sqrMagnitude > 0.001f)
    //        {
    //            current.rotation = Quaternion.LookRotation(dir);
    //        }
    //    }
    //}
    
    private void MoveSegments()
    {
        for (int i = 0; i < segments.Count; i++)
        {
            
            Transform current = segments[i];
            Transform target = (i == 0) ? transform : segments[i - 1];


            // Smooth rotate
            //current.rotation = Quaternion.RotateTowards(current.rotation, target.rotation, steerSpeed * Time.deltaTime);

            current.LookAt(target);

            // Move forward
            current.position += current.forward * moveSpeed * deltaTime;

            // Clamp distance
            Vector3 dir = current.position - target.position;
            float maxDistance = segmentLenght;
            Debug.Log($"Segment {i}: Distance to target = {dir.magnitude}");
            if (dir.magnitude > maxDistance)
            {
                current.position = target.position + dir.normalized * maxDistance;
            }
        }
    }

    private void GrowSnake()
    {
        GameObject body = bodyPool.GetObject(Vector3.zero, Quaternion.identity);
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

    //private void AddBody()
    //{
    //    GameObject body = bodyPool.GetObject();
    //    if (body == null) return;

    //    Transform last = segments[segments.Count - 1];

    //    // If tail exists insert before tail
    //    if (tail != null)
    //    {
    //        last = segments[segments.Count - 2];
    //    }

    //    body.transform.position = last.position - last.forward * segmentDistance;
    //    body.transform.rotation = last.rotation;
    //    body.SetActive(true);

    //    if (tail != null)
    //    {
    //        segments.Insert(segments.Count - 1, body.transform);
    //        directions.Insert(directions.Count - 1, last.forward);
    //    }
    //    else
    //    {
    //        segments.Add(body.transform);
    //        directions.Add(last.forward);
    //    }
    //}

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


