using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeInvincible : MonoBehaviour
{
    [SerializeField] private float duration = 10f;
    [SerializeField] private float invincibleSpeedMultiplier = 1.5f;
    [SerializeField] private Collider headCollider;

    private OnlyMovement movement;
    private SnakeSpeedBoost boost;
    private bool isTriggered;
    private Coroutine routine;

    private void Awake()
    {
        movement = GetComponent<OnlyMovement>();
        boost = GetComponent<SnakeSpeedBoost>();
        isTriggered = headCollider.isTrigger;
    }

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
        // Disable normal boost
        boost.ForceDisableBoost(true);
        // Enable boost visuals
        boost.SetBoostVisual(true);
        // Constant speed
        movement.BoostSpeed(invincibleSpeedMultiplier);
        headCollider.isTrigger = true;
        Debug.Log("INVINCIBLE START");
        yield return new WaitForSeconds(duration);
        headCollider.isTrigger = false;
        // Restore movement
        movement.ResetSpeed();
        // Restore visuals
        boost.SetBoostVisual(false);
        // Enable normal boost again
        boost.ForceDisableBoost(false);
        Debug.Log("INVINCIBLE END");
    }
}
