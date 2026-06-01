using UnityEngine;

public class SnakeInvincible2 : MonoBehaviour
{
    public bool IsInvincible { get; private set; }

    public void EnableInvincible()
    {
        IsInvincible = true;
    }

    public void DisableInvincible()
    {
        IsInvincible = false;
    }
}

