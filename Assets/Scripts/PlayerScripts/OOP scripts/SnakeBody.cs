using Lean.Pool;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBody : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject bodyPrefab;
    [SerializeField] private GameObject tailPrefab;

    [Header("References")]
    [SerializeField] private Transform headPoint;
    [SerializeField] private SnakeMovement movement;

    [Header("Body Settings")]
    [SerializeField] private float segmentDistance = 0.5f;
    [SerializeField] private float segmentLength = 1f;
    [SerializeField] private float distanceBetweenPoints = 0.15f;

    [Header("History")]
    [SerializeField] private int preHistory = 15;
    [SerializeField] private float stopThreshold = 0.01f;

    [SerializeField] private  List<Vector3> positionHistory = new();
    private readonly List<Transform> segments = new();

    private Transform tail;
    private bool isDead;
    public IReadOnlyList<Transform> Segments => segments;
    public Transform Head => segments[0];
    public Transform Tail => tail;
    public Transform TailPoint { get; private set; }

    private void Start()
    {
        InitializeSnake();
        
    }

    private void FixedUpdate()
    {
        UpdateHistory();
        MoveSegments();
        
    }

    private void OnDrawGizmos()
    {
        foreach(var position in positionHistory)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(position, 0.1f);
        }
    }
    private void InitializeSnake()
    {
        segments.Clear();
        positionHistory.Clear();

        segments.Add(transform);

        for (int i = 0; i < preHistory; i++)
        {
            Vector3 offset = headPoint.forward * Time.fixedDeltaTime * movement.MoveSpeed * i;

            positionHistory.Add(headPoint.position - offset);
        }

        for (int i = 0; i < 3; i++)
        {
            AddSegment();
        }
        CreateTail();
    }

    private void UpdateHistory()
    {
        if (movement.CurrentSpeed <= stopThreshold)
        {
            if (positionHistory.Count > 0)
            {
                positionHistory[0] = headPoint.position;
            }

            return;
        }
        positionHistory.Insert(0, headPoint.position);
    }

    private void MoveSegments()
    {
        for (int i = 1; i < segments.Count; i++)
        {
            Transform segment = segments[i];

            int index = Mathf.RoundToInt(i * segmentLength / distanceBetweenPoints);

            if (index >= positionHistory.Count)
                continue;

            Vector3 targetPos = positionHistory[index];

            segment.position = Vector3.Lerp(segment.position, targetPos, movement.CurrentSpeed * Time.fixedDeltaTime);

            if (Vector3.Distance(segment.position, targetPos) < 0.001f)
            {
                segment.position = targetPos;
            }

            Vector3 lookDir = segments[i - 1].position - segment.position;

            if (lookDir.sqrMagnitude > 0.0001f)
            {
                segment.rotation = Quaternion.LookRotation(lookDir);
            }
        }

        int maxHistory = segments.Count * Mathf.RoundToInt(segmentLength / distanceBetweenPoints) + 10;

        if (positionHistory.Count > maxHistory)
        {
            positionHistory.RemoveAt(positionHistory.Count - 1);
        }
    }

    public int GetTotalSegmentCount()
    {
        return segments.Count;
    }

    public int GetBodySegmentCount()
    {
        // excludes head + tail
        return Mathf.Max(0, segments.Count - 2);
    }

    public Transform GetLastBodySegment()
    {
        return segments.Count > 2 ? segments[segments.Count - 2] : null;
    }

    public void KillSnake()
    {
        if (isDead) return;

        isDead = true;
    }
    public void AddSegment()
    {
        GameObject body = LeanPool.Spawn(bodyPrefab);
        SnakePart part = body.GetComponentInParent<SnakePart>();

        if (part != null)
        {
            part.Owner = GetComponent<SnakeHealth>();
        }
        Transform last = tail != null ? segments[segments.Count - 2] : segments[segments.Count - 1];
        if (tail != null)
        {
            body.transform.position = tail.position;
            body.transform.rotation = tail.rotation;
        }
        else
        {
            body.transform.position = last.position - last.forward * segmentDistance;
            body.transform.rotation = last.rotation;
        }

        if (tail != null)
        {
            segments.Insert(segments.Count - 1, body.transform);
        }
        else
        {
            segments.Add(body.transform);
        }
        RefreshVFX();
    }

    public void RemoveSegment()
    {
        if (segments.Count <= 5)
            return;
        int removeIndex = segments.Count - 2;
        Transform segment = segments[removeIndex];
        segments.RemoveAt(removeIndex);
        LeanPool.Despawn(segment.gameObject);
    }

    private void CreateTail()
    {
        Transform last = segments[segments.Count - 1];
        tail = Instantiate(tailPrefab, last.position - last.forward * segmentDistance, last.rotation).transform;
        SnakePart part = tail.GetComponentInParent<SnakePart>();

        if (part != null)
        {
            part.Owner = GetComponent<SnakeHealth>();
        }
        TailPoint = tail.Find("TailPoint");
        segments.Add(tail);
        RefreshVFX();

    }

    private void RefreshVFX()
    {
        SnakeParticleVFX vfx = GetComponent<SnakeParticleVFX>();

        if (vfx != null)
            vfx.RefreshParticles();
    }
}


