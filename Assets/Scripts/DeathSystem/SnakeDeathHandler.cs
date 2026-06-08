using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeDeathHandler : MonoBehaviour
{
    [SerializeField] private SnakeHealth health;
    [SerializeField] private SnakeBody body;
    [SerializeField] private GameObject foodPrefab;
    private void Start()
    {
        health.OnDeath.AddListener(HandleDeath);
    }

    private void HandleDeath(DeathData data)
    {
        Debug.Log($"Drop {data.foodCount} food");

        // Spawn food here
        var segments = body.Segments;

        for (int i = 0; i < data.foodCount; i++)
        {
            Transform segment = segments[Random.Range(0, segments.Count)];

            Vector3 offset = Random.insideUnitSphere * 0.3f;

            offset.y = 0;

            LeanPool.Spawn(foodPrefab, segment.position + offset, Quaternion.identity);
        }
        body.CleanupBody();
    }
}
