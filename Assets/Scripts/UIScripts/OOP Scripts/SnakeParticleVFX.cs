using System.Collections.Generic;
using UnityEngine;

public class SnakeParticleVFX : MonoBehaviour
{
    [Header("Head VFX")]
    [SerializeField] private ParticleSystem headBoostParticles;

    [Header("Body Segments")]
    [SerializeField] private SnakeBody body;

    private SnakeBoost boost;

    private List<ParticleSystem> segmentParticles = new();

    private bool initialized = false;

    private void Awake()
    {
        boost = GetComponent<SnakeBoost>();
    }

    private void OnEnable()
    {
        if (boost != null)
        {
            boost.OnBoostChanged += SetBoostVFX;
        }
    }

    private void OnDisable()
    {
        if (boost != null)
        {
            boost.OnBoostChanged -= SetBoostVFX;
        }
    }

    private void Start()
    {
        CacheSegmentParticles();
    }

    private void CacheSegmentParticles()
    {
        segmentParticles.Clear();

        IReadOnlyList<Transform> segments = body.Segments;

        foreach (Transform segment in segments)
        {
            ParticleSystem ps = segment.GetComponentInChildren<ParticleSystem>();

            if (ps != null)
            {
                segmentParticles.Add(ps);
            }
        }

        initialized = true;
    }

    public void SetBoostVFX(bool active)
    {
        if (!initialized)
        {
            CacheSegmentParticles();
        }

        // Head particles
        if (headBoostParticles != null)
        {
            if (active)
                headBoostParticles.Play();
            else
                headBoostParticles.Stop();
        }

        // Body particles
        foreach (ParticleSystem ps in segmentParticles)
        {
            if (ps == null) continue;

            if (active)
                ps.Play();
            else
                ps.Stop();
        }
    }

    public void RefreshParticles()
    {
        CacheSegmentParticles();
    }
}