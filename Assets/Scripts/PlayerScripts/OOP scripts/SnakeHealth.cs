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

        int storedFood = foodStorage != null ? foodStorage.StoredFood : 0;
        int dropAmount = storedFood;
        DeathData data = new DeathData(
            dropAmount,
            transform.position
        );

        OnDeath?.Invoke(data);
    }

    public void ResetState()
    {
        isDead = false;
    }
}