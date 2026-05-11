using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ObjectPool pool;
    [SerializeField] private Transform snakeHead;

    [Header("Spawner Settings")]
    [SerializeField] private GameObject foodPrefab;
    [SerializeField] private int foodPerInterval = 4;
    [SerializeField] private int maxActiveFood = 15;
    [SerializeField] private Vector3 spawnArea = new Vector3(20, 1, 20);
    [SerializeField] private float spawnInterval = 5f;

    [Header("Smart Spawn")]
    [SerializeField] private float minDistanceFromSnake = 3f;
    [SerializeField] private int maxSpawnAttempts = 3;
    private int spawnedFood;

    private List<GameObject> activeFoods = new List<GameObject>();
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
        //First food spawn after 2 seconds, then repeat every spawnInterval seconds
        InvokeRepeating(nameof(SpawnFood), 2f, spawnInterval);
    }

    void SpawnFood()
    {
        if (pool == null) return;
        for (int j = 0; j < foodPerInterval; j++)
        {

            if (spawnedFood >= maxActiveFood)
                return;
            //Debug.Log($"Currently active: {spawnedFood}");
            GameObject food = pool.GetObject(Vector3.zero, Quaternion.identity);
            if (food == null) return;

            Vector3 spawnPos;
            bool foundValid = false;


            //maxSpawnAttempts is how many times we try to find a spawn point far from the snake before giving up
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
    }

    void ActivateFood(GameObject food, Vector3 position)
    {
        if (spawnedFood >= maxActiveFood)
            return;
        spawnedFood++;
        //Debug.Log($"Total active: {spawnedFood}");
        food.transform.position = position;
        food.SetActive(true);
        activeFoods.Add(food);
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

    public List<GameObject> GetFoodInRange(Transform target, float range)
    {
        List<GameObject> foodsInRange = new List<GameObject>();
        foreach (var food in activeFoods)
        {
            if (food != null && Vector3.Distance(food.transform.position, target.position) <= range)
            {
                foodsInRange.Add(food);
            }
        }
        return foodsInRange;
    }
}
