using System.Collections;
using UnityEngine;

public class RainBowPowerUp : MonoBehaviour
{
    [SerializeField] private float duration = 10f;

    private SnakeInvincible2 invincible;
    private SnakeBoost boost;

    private Coroutine routine;

    private void Awake()
    {
        invincible = GetComponent<SnakeInvincible2>();
        boost = GetComponent<SnakeBoost>();
    }

    public void ActivateRainbowMode()
    {
        if (routine != null)
        {
            StopCoroutine(routine);
        }

        routine = StartCoroutine(RainbowRoutine());
    }

    private IEnumerator RainbowRoutine()
    {
        invincible.EnableInvincible();

        boost.EnableFreeBoost();
        boost.ActivateBoost();

        yield return new WaitForSeconds(duration);

        invincible.DisableInvincible();

        boost.DisableFreeBoost();
        boost.DeactivateBoost();
    }
}