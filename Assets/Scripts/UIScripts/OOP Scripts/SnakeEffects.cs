using UnityEngine;

public class SnakeEffects : MonoBehaviour
{
    [SerializeField] private SnakeParticleVFX snakeVFX;

    private SnakeGrowth growth;
    private SnakeShrink shrink;

    private void Awake()
    {
        growth = GetComponent<SnakeGrowth>();
        shrink = GetComponent<SnakeShrink>();
    }

    private void OnEnable()
    {
        growth.OnGrow += RefreshParticles;
        shrink.OnShrink += RefreshParticles;
    }

    private void OnDisable()
    {
        growth.OnGrow -= RefreshParticles;
        shrink.OnShrink -= RefreshParticles;
    }

    private void RefreshParticles()
    {
        snakeVFX.RefreshParticles();
    }
}
