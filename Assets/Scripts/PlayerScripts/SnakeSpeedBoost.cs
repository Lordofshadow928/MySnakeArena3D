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
    [SerializeField] private SnakeParticleVFX snakeVFX;

    private bool isBoosting = false;
    private float drainTimer = 0f;
    private void Awake()
    {
        movement = GetComponent<OnlyMovement>();
        growthShrinkLogic = GetComponent<GrowthShrinkLogic>();
    }
    public void ActivateBoost()
    {
        if (isBoosting) return;
        isBoosting = true;
        movement.BoostSpeed(boostMultiplier);
        growthShrinkLogic.SetBoost();
        if(snakeVFX != null)
        {
            snakeVFX.SetBoostVFX(true);
        }
    }

    public void DeActivateBoost()
    {
        if (!isBoosting) return;
        isBoosting = false;
        movement.ResetSpeed();
        growthShrinkLogic.DeBoost();
        if (snakeVFX != null)
        {
            snakeVFX.SetBoostVFX(false);
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


