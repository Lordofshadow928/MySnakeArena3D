using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowthShrinkLogic : MonoBehaviour
{
    [Header("Pooling")]
    [SerializeField] private GameObject bodyPrefab;
    [SerializeField] private FoodSpawner2 foodSpawner;

    [Header("Tail")]
    [SerializeField] private GameObject tailPrefab;
    [SerializeField] private Transform tailPoint;

    [Header("UI")]
    [SerializeField] private SnakeProgressUI progressUI;
    [SerializeField] private SnakeParticleVFX snakeVFX;

    [Header("Growth Settings")]
    [SerializeField] private int foodsPerGrowth = 4;
    [SerializeField] private int foodsPerShrink = 4;

    [Header("Segment Settings")]
    [SerializeField] private float segmentDistance = 0.5f;
    [SerializeField] private float segmentLength = 1f;
    [SerializeField] private float distanceBetweenPoints = 0.15f;

    [Header("History")]
    [SerializeField] private int preHistory = 15;
    [SerializeField] private Transform headPoint;
    [SerializeField] private float stopThreshold = 0.01f;

    private List<Vector3> positionHistory = new();
    private List<Transform> segments = new();
    private List<Vector3> directions = new();
    public List<Transform> GetSegments()
    {
        return segments;
    }
    private bool isBoosted;

    private Transform tail;

    private int foodCounter;
    private int shrinkCounter;
    private int growPending;
    private int currentFoodStored = 0;

    private OnlyMovement movement;

    private void Start()
    {
        movement = GetComponent<OnlyMovement>();
        SpawnSnake();
    }

    private void FixedUpdate()
    {
        UpdateHistory();
        MoveSegments();

        if (growPending > 0)
        {
            GrowSnake();
            growPending--;
        }
    }

    private void SpawnSnake()
    {
        segments.Clear();
        directions.Clear();
        positionHistory.Clear();

        segments.Add(transform);
        directions.Add(transform.forward);

        for (int i = 0; i < preHistory; i++)
        {
            Vector3 movementOffset = headPoint.forward * Time.fixedDeltaTime * movement.GetMoveSpeed() * i;

            positionHistory.Add(headPoint.position - movementOffset);
        }

        for (int i = 0; i < 3; i++)
        {
            GrowSnake();
        }

        AddTail();
    }

    private void UpdateHistory()
    {
        // Snake stopped
        if (movement.CurrentSpeed <= stopThreshold)
        {
            // Keep first history point synced to head
            if (positionHistory.Count > 0)
            {
                positionHistory[0] = headPoint.position;
            }

            return;
        }

        if (isBoosted)
        {
            var lastPos = positionHistory[0];
            positionHistory.Insert(0, (headPoint.position + lastPos) / 2);
        }
        positionHistory.Insert(0, headPoint.position);
    }

    private void MoveSegments()
    {
        for (int i = 1; i < segments.Count; i++)
        {
            Transform segment = segments[i];

            int index = Mathf.RoundToInt(i * segmentLength / distanceBetweenPoints);

            if (index < positionHistory.Count)
            {
                Vector3 targetPos = positionHistory[index];

                float followSpeed = movement.CurrentSpeed;

                segment.position = Vector3.Lerp(segment.position,targetPos,followSpeed * Time.fixedDeltaTime);

                segment.LookAt(segments[i - 1]);

                // Snap to target when close enough
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
        }

        int maxHistory = segments.Count * Mathf.RoundToInt(segmentLength / distanceBetweenPoints) + 10;

        if (positionHistory.Count > maxHistory)
        {
            positionHistory.RemoveAt(positionHistory.Count - 1);
        }
    }

    public void AddFood()
    {
        foodCounter++;
        currentFoodStored++;
        progressUI.AddProgress(1);

        if (foodCounter >= foodsPerGrowth)
        {
            foodCounter = 0;
            growPending++;
        }
    }

    public void SetBoost()
    {
        isBoosted = true;
    }

    public void DeBoost()
    {
        isBoosted = false;
    }

    public void ConsumeBoostEnergy()
    {
        progressUI.RemoveProgress(1);

        shrinkCounter++;

        if (shrinkCounter >= foodsPerShrink)
        {
            shrinkCounter = 0;
            ShrinkSnake();
        }
    }

    private GameObject GrowSnake()
    {
        GameObject body = LeanPool.Spawn(bodyPrefab, Vector3.zero, Quaternion.identity);
        Transform last = (tail != null)
            ? segments[segments.Count - 2]
            : segments[segments.Count - 1];

        if (tail != null)
        {
            body.transform.position = tail.position;
            body.transform.rotation = tail.rotation;
        }
        else
        {
            body.transform.position =
                last.position - last.forward * segmentDistance;

            body.transform.rotation = last.rotation;
        }

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
        if (snakeVFX != null)
        {
            snakeVFX.RefreshParticles();
        }

        return body;
    }

    public void ShrinkSnake()
    {
        if (segments.Count <= 5) return;
        int removeIndex = segments.Count - 2;

        Transform segmentToRemove = segments[removeIndex];

        segments.RemoveAt(removeIndex);
        directions.RemoveAt(removeIndex);

        LeanPool.Despawn(segmentToRemove.gameObject);
        if (snakeVFX != null)
        {
            snakeVFX.RefreshParticles();
        }
    }

    private void AddTail()
    {
        Transform last = segments[segments.Count - 1];

        tail = Instantiate(tailPrefab,last.position - last.forward * segmentDistance,last.rotation).transform;

        //Find tail point for poop system
        tailPoint = tail.Find("TailPoint");

        segments.Add(tail);
        directions.Add(last.forward);
    }

    public Transform GetTailPoint()
    {
        return tailPoint;
    }
    public int GetStoredFood()
    {
        return currentFoodStored;
    }

    public void RemoveStoredFood(int amount)
    {
        currentFoodStored -= amount;

        currentFoodStored = Mathf.Max(0, currentFoodStored);
    }
}
