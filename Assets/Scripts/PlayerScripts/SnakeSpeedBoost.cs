using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeSpeedBoost : MonoBehaviour
{
    [Header("Boost Settings")]
    public float boostMultiplier = 2f;
    private OnlyMovement movement;
    private GrowthShrinkLogic growthShrinkLogic;
    [SerializeField] private SnakeProgressUI progressUI;
    [SerializeField] private float drainInterval = 0.3f;

    [Header("Boost VFX")]
    [SerializeField] private ParticleSystem boostVFX;

    private float drainTimer = 0f;
    private void Awake()
    {
        movement = GetComponent<OnlyMovement>();
        growthShrinkLogic = GetComponent<GrowthShrinkLogic>();
    }
    public void ActivateBoost()
    {
        movement.BoostSpeed(boostMultiplier);
        growthShrinkLogic.SetBoost();
        if(boostVFX != null)
        {
            boostVFX.Play();
        }
    }

    public void DeActivateBoost()
    {
        movement.ResetSpeed();
        growthShrinkLogic.DeBoost();
        if (boostVFX != null)
        {
            boostVFX.Stop();
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space) && progressUI.HasEnergy())
        {
            ActivateBoost();
            drainTimer += Time.deltaTime;
            if (drainTimer >= drainInterval)
            {
                growthShrinkLogic.ConsumeBoostEnergy();
                progressUI.RemoveProgress(0);
                drainTimer = 0f;
            }
        }
        else
        {
            DeActivateBoost();
            drainTimer = 0f;
        }
    }
}


