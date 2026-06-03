using UnityEngine;
public class SnakeInvincible2 : MonoBehaviour
{
    [SerializeField] private Collider snakeHeadCollider;

    public bool IsInvincible { get; private set; }

    public void EnableInvincible()
    {
        Debug.Log("ENABLE INVINCIBLE");

        IsInvincible = true;

        if (snakeHeadCollider != null)
        {
            snakeHeadCollider.isTrigger = true;
            Debug.Log("HEAD COLLIDER = TRIGGER");
        }
    }

    public void DisableInvincible()
    {
        Debug.Log("DISABLE INVINCIBLE");

        IsInvincible = false;

        if (snakeHeadCollider != null)
        {
            snakeHeadCollider.isTrigger = false;
            Debug.Log("HEAD COLLIDER = SOLID");
        }
    }
}

