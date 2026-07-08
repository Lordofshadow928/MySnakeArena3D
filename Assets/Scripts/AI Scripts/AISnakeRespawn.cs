using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISnakeRespawn : MonoBehaviour
{
    private SnakeSpawner spawner;

    public void Initialize(SnakeSpawner snakeSpawner)
    {
        spawner = snakeSpawner;

        GetComponent<SnakeHealth>()
            .OnDeath
            .AddListener(OnDeath);
    }

    void OnDeath(DeathData data)
    {
        spawner.RespawnAI();

        Destroy(gameObject);
    }


}
