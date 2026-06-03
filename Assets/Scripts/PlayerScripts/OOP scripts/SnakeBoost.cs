using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBoost : MonoBehaviour
{
    [SerializeField] private float boostMultiplier = 1.5f;
    private SnakeMovement movement;
    public bool IsBoosting { get; private set; }
    public bool IgnoreEnergyCost { get; private set; }
    public bool ForceBoost { get; private set; }
    private bool boostDisabled;
    public event Action<bool> OnBoostChanged;

    private void Awake()
    {
        movement = GetComponent<SnakeMovement>();
    }

    public void ActivateBoost()
    {
        if (boostDisabled) return;
        if (IsBoosting) return;

        IsBoosting = true;
        movement.BoostSpeed(boostMultiplier);
        OnBoostChanged?.Invoke(true);
    }

    public void DeactivateBoost()
    {
        if (ForceBoost) return;
        if (!IsBoosting) return;
        IsBoosting = false;
        movement.ResetSpeed();
        OnBoostChanged?.Invoke(false);
    }

    public void EnableForcedBoost()
    {
        ForceBoost = true;

        ActivateBoost();
    }

    public void DisableForcedBoost()
    {
        ForceBoost = false;

        DeactivateBoost();
    }

    public void DisableBoost()
    {
        boostDisabled = true;
        DeactivateBoost();
    }

    public void EnableBoost()
    {
        boostDisabled = false;
    }

    public void EnableFreeBoost()
    {
        IgnoreEnergyCost = true;
    }

    public void DisableFreeBoost()
    {
        IgnoreEnergyCost = false;
    }
}
