using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    [Header("References")]
    public ObjectPool pool;
    public Transform snakeHead;

    [Header("Spawner Settings")]
    public GameObject foodPrefab;
    public int maxActiveFood = 15;
    public Vector3 spawnArea = new Vector3(20, 1, 20);
    public float spawnInterval = 5f;

    [Header("Smart Spawn")]
    public float minDistanceFromSnake = 3f;
    public int maxSpawnAttempts = 10;

    void Start()
    {
        //Auto-assign prefab to pool if missing
        if (pool != null && pool.prefab == null && foodPrefab != null)
        {
            pool.prefab = foodPrefab;
        }

        //Safety check
        if (pool == null || pool.prefab == null)
        {
            Debug.LogError("FoodSpawner: Missing pool or prefab!");
            return;
        }

        InvokeRepeating(nameof(SpawnFood), 2f, spawnInterval);
    }

    void SpawnFood()
    {
        if (pool == null) return;

        if (pool.ActiveCount() >= maxActiveFood)
            return;

        GameObject food = pool.GetObject();
        if (food == null) return;

        Vector3 spawnPos;
        bool foundValid = false;

        for (int i = 0; i < maxSpawnAttempts; i++)
        {
            spawnPos = GetRandomPosition();

            if (IsFarFromSnake(spawnPos))
            {
                ActivateFood(food, spawnPos);
                foundValid = true;
                break;
            }
        }

        if (!foundValid)
        {
            ActivateFood(food, GetRandomPosition());
        }
    }

    void ActivateFood(GameObject food, Vector3 position)
    {
        food.transform.position = position;
        food.SetActive(true);
    }
    Vector3 GetRandomPosition()
    {
        return new Vector3(
            Random.Range(-spawnArea.x, spawnArea.x),
            0.5f,
            Random.Range(-spawnArea.z, spawnArea.z)
        );
    }

    bool IsFarFromSnake(Vector3 pos)
    {
        if (snakeHead == null) return true;

        return Vector3.Distance(pos, snakeHead.position) >= minDistanceFromSnake;
    }
}
