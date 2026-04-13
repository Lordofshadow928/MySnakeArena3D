using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public struct LocationHistory
{
    public Vector3 position;
    public Quaternion rotation;
    public LocationHistory(Vector3 pos, Quaternion rot)
    {
        position = pos;
        rotation = rot;
    }
}

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 100f;
    public float steerSpeed = 180.0f;

    [Header("Body Settings")]
    public float bodySpeed = 5.0f;
    [SerializeField] private int gap = 5;
    private GameObject tail;

    [Header("Pooling")]
    public ObjectPool bodyPool;
    [Header("Tail Settings")]
    public GameObject tailPrefab;
    // Internal components
    [SerializeField] private List<Transform> segments = new List<Transform>();

    [SerializeField] private float segmentDistance = 0.5f;
    private List<GameObject> bodyParts = new List<GameObject>();
    [SerializeField]
    //private List<LocationHistory> locationHistory = new List<LocationHistory>();
    private int growPending = 0;

    private void Start()
    {
        segments.Clear();

        // Add head first
        segments.Add(transform);

        // Add body parts
        foreach (var body in bodyParts)
        {
            segments.Add(body.transform);
        }

        // Add tail last
        if (tail != null)
        {
            segments.Add(tail.transform);
        }
    }


    [ContextMenu("Init")]
    //private void Init()
    //{
    //    //locationHistory.Clear();
    //    bodyParts.Clear();
    //    //PrefillPositionHistory();
    //    SpawnSnakesBody();
    //}

  

    private void SpawnSnakesBody()
    {
        // Spawn initial body parts
        for (int i = 0; i < 5; i++)
        {
            GrowSnake();
        }
        AddTail();
    }

    //private void PrefillPositionHistory()
    //{
    //    // Pre-fill position history to avoid all body parts spawning on top of each other
    //    for (int i = 0; i < 400; i++)
    //    {
    //        // Fix CS1503: create LocationHistory from Vector3 and Quaternion
    //        locationHistory.Add(new LocationHistory(
    //            transform.position - transform.forward * 0.1f * i,
    //            transform.rotation
    //        ));
    //    }
    //}

    //private void Update()
    //{
    //    // Handle growth if pending
    //    if (growPending > 0)
    //    {
    //        GrowSnake();
    //        growPending--;
    //    }
    //    MoveSnake();
    //}
    private void Update()
    {
        MoveHead();
        MoveSegments();
    }

    //private void FixedUpdate()
    //{
    //    MoveSnake();
    //}

    private void MoveSnake()
    {
        MoveHead();
        //RecordHeadPosition();
        //PrunePositionHistory();
        //MoveBodyParts();
        //MoveTail();
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

    //private float minDistance = 0.15f;
    //private void RecordHeadPosition()
    //{
    //    if (locationHistory.Count == 0 ||
    //        Vector3.Distance(locationHistory[0].position, transform.position) > minDistance)
    //    {
    //        //positionHistory.Insert(0, transform.position);
    //        locationHistory.Insert(0, new LocationHistory(transform.position, transform.rotation));
    //    }
    //}

    //private void PrunePositionHistory()
    //{
    //    // Limit history to prevent memory overflow
    //    int maxHistory = (bodyParts.Count + 1) * gap;
    //    if (locationHistory.Count > maxHistory)
    //    {
    //        locationHistory.RemoveRange(maxHistory,locationHistory.Count - maxHistory);
    //    }
    //}

    //private void MoveBodyParts()
    //{
    //    for (int i = 0; i < bodyParts.Count; i++)
    //    {
    //        float index = i * gap;

    //        int indexA = Mathf.FloorToInt(index);
    //        int indexB = Mathf.Min(indexA + 1, positionHistory.Count - 1);
    //        float t = index - indexA;

    //        if (indexA >= positionHistory.Count) continue;

    //        Vector3 pointA = positionHistory[indexA];
    //        Vector3 pointB = positionHistory[indexB];

    //        //Smooth interpolation
    //        Vector3 targetPos = Vector3.Lerp(pointA, pointB, t);

    //        Transform part = bodyParts[i].transform;

    //        Vector3 dir = targetPos - part.position;

    //        //Smooth follow
    //        float followSpeed = bodySpeed * (1f + i * 0.05f);
    //        part.position += dir * followSpeed * Time.deltaTime;

    //        if (dir.sqrMagnitude > 0.0001f)
    //        {
    //            Quaternion targetRot = Quaternion.LookRotation(dir);
    //            part.rotation = Quaternion.Slerp(part.rotation, targetRot, 10f * Time.deltaTime);
    //        }
    //    }
    //}

    //private void MoveBodyParts()
    //{
    //    for (int i = 0; i < bodyParts.Count; i++)
    //    {
    //        int index = i * gap;

    //        if (index >= locationHistory.Count) continue;

    //        Transform part = bodyParts[i].transform;

    //        //NO interpolation
    //        part.position = locationHistory[index].position;

    //        //Use stored rotation
    //        part.rotation = locationHistory[index].rotation;
    //    }
    //}
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

    //private void GrowSnake()
    //{
    //    GameObject body = bodyPool.GetObject();
    //    if (body == null) return;

    //    body.transform.position = GetPreTailPosition();
    //    body.transform.rotation = Quaternion.identity;
    //    body.SetActive(true);

    //    if (bodyParts.Count > 0)
    //    {
    //        bodyParts.Insert(bodyParts.Count - 1, body);
    //    }
    //    else
    //    {
    //        bodyParts.Add(body);
    //    }
    //}
    private void GrowSnake()
    {
        GameObject body = bodyPool.GetObject();
        if (body == null) return;

        Transform last = segments[segments.Count - 1];

        body.transform.position = last.position - last.forward * segmentDistance;
        body.transform.rotation = last.rotation;
        body.SetActive(true);

        segments.Add(body.transform);
    }
    //private Vector3 GetPreTailPosition()
    //{
    //    if (locationHistory == null || locationHistory.Count == 0)
    //        return transform.position;

    //    int index = Mathf.Clamp((bodyParts.Count - 1) * gap,0, locationHistory.Count - 1);
    //    return locationHistory[index].position;
    //}
    private void AddTail()
    {
        tail = Instantiate(tailPrefab, GetLastPosition(), Quaternion.identity);
    }

    //private void MoveTail()
    //{
    //    if (tail == null) return;

    //    float index = bodyParts.Count * gap;

    //    if (index >= locationHistory.Count - 1) return;

    //    int indexA = Mathf.FloorToInt(index);
    //    int indexB = indexA + 1;
    //    float t = index - indexA;

    //    Vector3 pos = Vector3.Lerp(locationHistory[indexA].position, locationHistory[indexB].position, t);

    //    tail.transform.position = pos;
    //    tail.transform.rotation = locationHistory[indexA].rotation;
    //}
    //private void MoveTail()
    //{
    //    if (tail == null) return;

    //    int index = bodyParts.Count * gap;

    //    if (index >= locationHistory.Count) return;

    //    //Directly use stored history (no interpolation)
    //    tail.transform.position = locationHistory[index].position;

    //    //Use stored rotation (already correct)
    //    tail.transform.rotation = locationHistory[index].rotation;
    //}

    //private Vector3 GetLastPosition()
    //{
    //    if (bodyParts.Count == 0)
    //        return transform.position - transform.forward;

    //    Transform last = bodyParts[bodyParts.Count - 1].transform;
    //    return last.position - last.forward;
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            growPending++;
            other.gameObject.SetActive(false);
        }

    }
  
}

