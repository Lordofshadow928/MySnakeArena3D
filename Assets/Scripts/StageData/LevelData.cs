using UnityEngine;

[CreateAssetMenu(menuName = "Data/Level Data")]
public class LevelData : ScriptableObject
{
    [Header("Prefabs")]
    public GameObject mapPrefab;
    public GameObject foodPrefab;

    [Header("Food Spawn")]
    public int foodPerInterval = 4;
    public int maxActiveFood = 15;
    public float spawnInterval = 5f;
    public Vector3 spawnArea = new Vector3(5, 1, 5);
    public float minDistanceFromSnake = 3f;
    public int maxSpawnAttempts = 3;
}