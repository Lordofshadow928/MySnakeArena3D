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
        invincible = GetComponentInChildren<SnakeInvincible2>();
        boost = GetComponent<SnakeBoost>();
    }

    public void ActivateRainbowMode()
    {
        Debug.Log("RAINBOW PICKED");
        if (routine != null)
        {
            StopCoroutine(routine);
        }

        routine = StartCoroutine(RainbowRoutine());
    }

    private IEnumerator RainbowRoutine()
    {
        Debug.Log("COROUTINE START");
        if (invincible == null)
        {
            Debug.LogError("INVINCIBLE NULL");
            yield break;
        }

        if (boost == null)
        {
            Debug.LogError("BOOST NULL");
            yield break;
        }
        invincible.EnableInvincible();
        Debug.Log("INVINCIBLE ENABLED");

        boost.EnableBoost();
        boost.EnableFreeBoost();
        boost.EnableForcedBoost();
        Debug.Log("BOOST ACTIVATED");
        yield return new WaitForSeconds(duration);

        invincible.DisableInvincible();

        boost.DisableFreeBoost();
        boost.DisableForcedBoost();
        Debug.Log("RAINBOW END");
    }
}