using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathResultObserver : MonoBehaviour
{
    [SerializeField] private SnakeHealth health;
    [SerializeField] private ResultHandler resultHandler;

    private void Start()
    {
        health.OnDeath.AddListener(OnDeath);
    }

    private void OnDestroy()
    {
        health.OnDeath.RemoveListener(OnDeath);
    }

    private void OnDeath(DeathData data)
    {
        resultHandler.HandleResult();
    }
}
