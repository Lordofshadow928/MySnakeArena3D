using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathResultObserver : MonoBehaviour
{
    [SerializeField] private ResultHandler resultHandler;
    private SnakeHealth health;
    public void Initialize(SnakeHealth snakeHealth)
    {
        health = snakeHealth;
        if (health != null)
        {
            health.OnDeath.AddListener(OnDeath);
        }
    }

    private void OnDestroy()
    {
        if (health != null)
        {
            health.OnDeath.RemoveListener(OnDeath);
        }
    }

    private void OnDeath(DeathData data)
    {
        resultHandler.HandleResult();
    }
}
