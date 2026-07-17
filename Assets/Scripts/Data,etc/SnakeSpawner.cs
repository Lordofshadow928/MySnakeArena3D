using System.Collections;
using UnityEngine;

public class SnakeSpawner : MonoBehaviour
{
    [SerializeField] private FoodSpawner foodSpawner;
    [SerializeField] private CameraFollow cameraFollow;
    [SerializeField] private DeathResultObserver deathObserver;
    [SerializeField] private ResultFoodUI resultFoodUI;
    [SerializeField] private SnakeProgressUI progressUI;
    private Transform player;
    private LevelData level;
    private MapData map;

    private void Start()
    {
        level = LevelManager.Instance.CurrentLevelData;
        map = LevelManager.Instance.CurrentMapData;

        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(level.firstSpawnDelay);

        SpawnPlayer();

        for (int i = 0; i < level.aiCount; i++)
        {
            SpawnAI();
            yield return new WaitForSeconds(level.aiSpawnInterval);
        }
    }

    void SpawnPlayer()
    {
        player =Instantiate(level.playerPrefab, map.PlayerSpawnPoint.position, map.PlayerSpawnPoint.rotation).transform;
        foodSpawner.Initialize(level, map, player);
        cameraFollow.Initialize(player);
        deathObserver.Initialize(player.GetComponent<SnakeHealth>());
        resultFoodUI.Initialize(player.GetComponent<SnakeFoodStorage>());
        progressUI.Initialize(player.GetComponent<SnakeEnergy>());
    }

    public void SpawnAI()
    {
        Transform spawn = map.GetRandomBotSpawn();

        GameObject snake = Instantiate(level.aiSnakePrefab, spawn.position, spawn.rotation);

        AISnakeRespawn respawn = snake.GetComponent<AISnakeRespawn>();

        if (respawn != null)
            respawn.Initialize(this);
    }

    public void RespawnAI()
    {
        StartCoroutine(RespawnRoutine());
    }

    IEnumerator RespawnRoutine()
    {
        yield return new WaitForSeconds(level.aiRespawnDelay);

        SpawnAI();
    }
}