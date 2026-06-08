using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeDeathHandler : MonoBehaviour
{
    [SerializeField] private SnakeHealth health;
    [SerializeField] private GameObject foodPrefab;

    private void Start()
    {
        health.OnDeath.AddListener(HandleDeath);
    }

    private void HandleDeath(DeathData data)
    {
        Debug.Log($"Drop {data.foodCount} food");

        // Spawn food here
        for (int i = 0; i < data.foodCount; i++)
        {
            Vector3 offset = Random.insideUnitSphere;
            offset.y = 0;

            Instantiate(foodPrefab, data.position + offset, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
