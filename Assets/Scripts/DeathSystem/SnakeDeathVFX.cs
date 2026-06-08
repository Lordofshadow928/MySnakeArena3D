using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeDeathVFX : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SnakeHealth health;
    [SerializeField] private Animator animator;

    [Header("Effects")]
    [SerializeField] private ParticleSystem deathExplosion;

    [Header("Timing")]
    [SerializeField] private float destroyDelay = 0.5f;

    private void Awake()
    {
        if (health == null)
        {
            health = GetComponent<SnakeHealth>();
        }
    }

    private void OnEnable()
    {
        if (health != null)
        {
            health.OnDeath.AddListener(PlayDeathVFX);
        }
    }

    private void OnDisable()
    {
        if (health != null)
        {
            health.OnDeath.RemoveListener(PlayDeathVFX);
        }
    }

    private void PlayDeathVFX(DeathData data)
    {
        // Animation
        if (animator != null)
        {
            animator.SetTrigger("onDie");
        }

        // Explosion effect
        if (deathExplosion != null)
        {
            Instantiate(deathExplosion, data.position, Quaternion.identity);
        }

        StartCoroutine(DestroyAfterDelay());
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(destroyDelay);

        Destroy(gameObject);
    }
}
