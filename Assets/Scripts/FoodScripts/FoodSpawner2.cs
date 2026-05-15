using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

public class FoodSpawner2 : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform snakeHead;

    [Header("Spawner Settings")]
    [SerializeField] private GameObject foodPrefab;
    [SerializeField] private int foodPerInterval = 4;
    [SerializeField] private int maxActiveFood = 15;
    [SerializeField] private Vector3 spawnArea = new Vector3(5, 1, 5);
    [SerializeField] private float spawnInterval = 5f;

    [Header("Smart Spawn")]
    [SerializeField] private float minDistanceFromSnake = 3f;
    [SerializeField] private int maxSpawnAttempts = 3;
    private int spawnedFood;

    private List<GameObject> activeFoods = new List<GameObject>();
    void Start()
    {
        //Safety check
        if (foodPrefab == null)
        {
            Debug.LogError("FoodSpawner: Missing food prefab!");
            return;
        }
            //First food spawn after 2 seconds, then repeat every spawnInterval seconds
            InvokeRepeating(nameof(SpawnFood), 2f, spawnInterval);
    }

    void SpawnFood()
    {
        
        for (int j = 0; j < foodPerInterval; j++)
        {

            if (spawnedFood >= maxActiveFood)
                return;
            //Debug.Log($"Currently active: {spawnedFood}");
            GameObject food = LeanPool.Spawn(foodPrefab);
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
        //food.SetActive(true);
        activeFoods.Add(food);
    }

    public void OnFoodReturn(GameObject food)
    {
        if (food == null) return;

        // Remove from active list
        activeFoods.Remove(food);

        // Reduce active counter safely
        if (spawnedFood > 0)
            spawnedFood--;
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

