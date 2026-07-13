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

    [Header("Snake Prefabs")]
    public GameObject playerPrefab;
    public GameObject aiSnakePrefab;

    [Header("Powerups")]
    public GameObject[] powerupPrefabs;
    [Header("AI Spawn")]
    public float firstSpawnDelay = 0.5f;
    public int aiCount = 7;
    public float aiSpawnInterval = 0.5f;
    public float aiRespawnDelay = 3f;

    [Header("Powerup Spawn")]
    public float powerupSpawnInterval = 15f;
    public int maxPowerups = 3;
}