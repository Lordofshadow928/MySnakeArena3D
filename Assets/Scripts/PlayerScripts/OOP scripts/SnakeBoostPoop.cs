using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBoostPoop : MonoBehaviour
{
    [SerializeField] private GameObject poopFoodPrefab;
    [SerializeField] private float poopInterval = 0.3f;

    private float timer;

    private SnakeBoost boost;
    private SnakeBody body;
    private SnakeFoodStorage storage;

    private void Awake()
    {
        boost = GetComponent<SnakeBoost>();
        body = GetComponent<SnakeBody>();
        storage = GetComponent<SnakeFoodStorage>();
    }

    private void Update()
    {
        if (!boost.IsBoosting)
        {
            timer = 0f;
            return;
        }

        timer += Time.deltaTime;

        if (timer >= poopInterval)
        {
            timer = 0f;
            SpawnPoop();
        }
    }

    private void SpawnPoop()
    {
        if (body.Segments.Count <= 5)
            return;

        if (storage.StoredFood <= 0)
            return;

        Transform tail = body.TailPoint;

        Vector3 pos = tail.position - tail.forward * 0.9f;

        LeanPool.Spawn(poopFoodPrefab, pos, tail.rotation);

        storage.RemoveFood(1);
    }
}
