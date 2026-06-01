using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeInvincible2 : MonoBehaviour
{
    [SerializeField] private float duration = 10f;
    public bool IsInvincible { get; private set; }
    private Coroutine routine;

    public void ActivateInvincible()
    {
        if (routine != null)
        {
            StopCoroutine(routine);
        }
        routine = StartCoroutine(InvincibleRoutine());
    }

    private IEnumerator InvincibleRoutine()
    {
        IsInvincible = true;
        yield return new WaitForSeconds(duration);
        IsInvincible = false;
    }
}

