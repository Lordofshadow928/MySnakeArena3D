using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DeathEvent : UnityEvent<DeathData> { }

public class SnakeHealth : MonoBehaviour
{
    [Header("Events")]
    public DeathEvent OnDeath;

    private bool isDead;

    [Header("References")]
    [SerializeField] private SnakeFoodStorage foodStorage;

    public void Die()
    {
        if (isDead)
            return;
        isDead = true;
        SnakeMovement movement = GetComponent<SnakeMovement>();
        if (movement != null)
        {
            movement.enabled = false;
        }

        AISnakeBrain brain = GetComponent<AISnakeBrain>();
        if (brain != null)
        {
            brain.enabled = false;
        }

        SnakeCollisionDeath collisionDeath = GetComponent<SnakeCollisionDeath>();
        if (collisionDeath != null)
        {
            collisionDeath.enabled = false;
        }
        SnakeGrowth growth = GetComponent<SnakeGrowth>();

        if (growth != null)
        {
            growth.enabled = false;
        }
        int storedFood = foodStorage != null ? foodStorage.StoredFood : 0;
        int dropAmount = storedFood;
        DeathData data = new DeathData(dropAmount, transform.position);

        OnDeath?.Invoke(data);
    }

    public void ResetState()
    {
        isDead = false;
    }
}