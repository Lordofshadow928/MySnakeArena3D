using UnityEngine;

public class SnakeEffects : MonoBehaviour
{
    [SerializeField] private SnakeParticleVFX snakeVFX;

    private SnakeGrowth growth;
    private SnakeShrink boost;

    private void Awake()
    {
        growth = GetComponent<SnakeGrowth>();
        boost = GetComponent<SnakeShrink>();
    }

    private void OnEnable()
    {
        growth.OnGrow += RefreshParticles;
        boost.OnShrink += RefreshParticles;
    }

    private void OnDisable()
    {
        growth.OnGrow -= RefreshParticles;
        boost.OnShrink -= RefreshParticles;
    }

    private void RefreshParticles()
    {
        snakeVFX.RefreshParticles();
    }
}
