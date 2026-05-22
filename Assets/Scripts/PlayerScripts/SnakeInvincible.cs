using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeInvincible : MonoBehaviour
{
    [SerializeField] private float duration = 5f;
    [SerializeField] private float invincibleSpeedMultiplier = 2f;

    private OnlyMovement movement;
    private SnakeSpeedBoost boost;

    private int normalLayer;
    private int invincibleLayer;

    private Coroutine routine;

    private void Awake()
    {
        movement = GetComponent<OnlyMovement>();
        boost = GetComponent<SnakeSpeedBoost>();

        normalLayer = LayerMask.NameToLayer("Snake");
        invincibleLayer = LayerMask.NameToLayer("InvincibleSnake");
    }

    public void ActivateInvincible(float time)
    {
        if (routine != null)
        {
            StopCoroutine(routine);
        }

        routine = StartCoroutine(InvincibleRoutine(time));
    }

    private IEnumerator InvincibleRoutine(float time)
    {
        // Disable normal boost
        boost.ForceDisableBoost(true);

        // Enable boost visuals
        boost.SetBoostVisual(true);

        // Constant speed
        movement.BoostSpeed(invincibleSpeedMultiplier);

        // Change layer
        gameObject.layer = invincibleLayer;

        Debug.Log("INVINCIBLE START");

        yield return new WaitForSeconds(time);

        // Restore layer
        gameObject.layer = normalLayer;

        // Restore movement
        movement.ResetSpeed();

        // Restore visuals
        boost.SetBoostVisual(false);

        // Enable normal boost again
        boost.ForceDisableBoost(false);

        Debug.Log("INVINCIBLE END");
    }
}
