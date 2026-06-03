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
            snakeHeadCollider.enabled = true;
            Debug.Log("COLLIDER TRIGGER ON");
        }
        else
        {
            Debug.LogError("HEAD COLLIDER NULL");
        }
    }

    public void DisableInvincible()
    {
        Debug.Log("DISABLE INVINCIBLE");
        IsInvincible = false;
        if (snakeHeadCollider != null)
        {
            snakeHeadCollider.enabled = false;
            Debug.Log("COLLIDER TRIGGER OFF");
        }
        
    }
}

