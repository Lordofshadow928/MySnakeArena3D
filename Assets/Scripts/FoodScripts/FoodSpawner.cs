using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

public class FoodSpawner : MonoBehaviour
{
    [Header("Obstacle Check")]
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private float obstacleCheckRadius = 0.4f;
    private Transform snakeHead;
    private Transform[] spawnPoints;
    private GameObject foodPrefab;
    private int foodPerInterval;
    private int maxActiveFood;
    private float spawnInterval;
    private Vector3 spawnArea;
    private float minDistanceFromSnake;
    private int maxSpawnAttempts;
  
    bool IsPositionFree(Vector3 pos)
    {
        return !Physics.CheckSphere(pos, obstacleCheckRadius, obstacleLayer);
    }
    private int spawnedFood;

    [SerializeField]private List<GameObject> spawnedFoods = new List<GameObject>();
    public void Initialize(LevelData level, MapData map)
    {
        snakeHead = FindFirstObjectByType<SnakeMovement>().transform;

        foodPrefab = level.foodPrefab;

        foodPerInterval = level.foodPerInterval;

        maxActiveFood = level.maxActiveFood;

        spawnInterval = level.spawnInterval;

        spawnArea = level.spawnArea;

        minDistanceFromSnake = level.minDistanceFromSnake;

        maxSpawnAttempts = level.maxSpawnAttempts;

        spawnPoints = map.SpawnPoints;

        CancelInvoke();

        InvokeRepeating(nameof(SpawnFood), 2f, spawnInterval);
    }
    void Start()
    {
        if (foodPrefab != null)
            return;

        Initialize(LevelManager.Instance.CurrentLevelData, LevelManager.Instance.CurrentMapData);
    }

    void SpawnFood()
    {
        
        for (int j = 0; j < foodPerInterval; j++)
        {

            if (spawnedFood >= maxActiveFood)
                return;
            GameObject food = LeanPool.Spawn(foodPrefab);
            if (food == null) return;

            Vector3 spawnPos;
            bool foundValid = false;


            //maxSpawnAttempts is how many times we try to find a spawn point far from the snake before giving up
            for (int i = 0; i < maxSpawnAttempts; i++)
            {
                spawnPos = GetRandomPosition();

                if (IsFarFromSnake(spawnPos) && IsPositionFree(spawnPos))
                {
                    ActivateFood(food, spawnPos);
                    foundValid = true;
                    break;
                }
            }


            if (!foundValid)
            {
                Vector3 pos = GetRandomPosition();
                if(IsPositionFree(pos))
                {
                    ActivateFood(food, pos);
                }
                else
                {
                    LeanPool.Despawn(food);
                }
            }
        }
    }

    void ActivateFood(GameObject food, Vector3 position)
    {
        if (spawnedFood >= maxActiveFood)
            return;
        spawnedFood++;
        food.transform.position = position;
        spawnedFoods.Add(food);
    }

    public void OnFoodReturn(GameObject food)
    {
        if (food == null) return;

        // Remove from active list
        spawnedFoods.Remove(food);
        // Reduce active counter safely
        if (spawnedFood > 0)
            spawnedFood--;
        LeanPool.Despawn(food);
    }

    Vector3 GetRandomPosition()
    {
        Transform center = spawnPoints[Random.Range(0, spawnPoints.Length)];

        Vector3 pos = center.position + new Vector3(Random.Range(-spawnArea.x, spawnArea.x), 0.5f, Random.Range(-spawnArea.z, spawnArea.z));

        Debug.Log($"SpawnPoint: {center.name} | Center: {center.position} | SpawnPos: {pos}");

        return pos;
    }

    bool IsFarFromSnake(Vector3 pos)
    {
        if (snakeHead == null) return true;

        return Vector3.Distance(pos, snakeHead.position) >= minDistanceFromSnake;
    }

    public Transform GetClosestFood(Vector3 fromPosition)
    {
        GameObject closest = null;
        float minDist = float.MaxValue;

        foreach (var food in spawnedFoods)
        {
            if (food == null) continue;

            float dist = Vector3.Distance(fromPosition, food.transform.position);

            if (dist < minDist)
            {
                minDist = dist;
                closest = food;
            }
        }

        return closest != null ? closest.transform : null;
    }

    private void OnDrawGizmos()
    {
        if (spawnPoints == null) return;

        foreach (Transform point in spawnPoints)
        {
            if (point == null) continue;

            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(point.position, 0.5f);

            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(point.position, new Vector3(spawnArea.x * 2, 0.1f, spawnArea.z * 2));
        }
    }
}

