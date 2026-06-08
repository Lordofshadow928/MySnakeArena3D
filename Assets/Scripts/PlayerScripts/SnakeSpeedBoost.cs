using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SnakeSpeedBoost : MonoBehaviour
{
    [Header("Boost Settings")]
    [SerializeField] private float boostMultiplier = 1.5f;
    private OnlyMovement movement;
    private GrowthShrinkLogic growthShrinkLogic;
    [SerializeField] private SnakeProgressUI progressUI;
    //[SerializeField] private float drainInterval = 0.3f;

    [Header("Boost UI")]
    [SerializeField] private Image boostImage;
    [SerializeField] private Sprite inactiveSprite;
    [SerializeField] private Sprite activeSprite;

    [Header("Boost VFX")]
    [SerializeField] private SnakeParticleVFX snakeVFX;

    [Header("Poop Food")]
    [SerializeField] private GameObject poopFoodPrefab;
    //[SerializeField] private float poopInterval = 0.3f;

    private bool isBoosting = false;
    private bool forceDisableBoost = false;
    //private float drainTimer = 0f;
    //private float poopTimer = 0f;
    private void Awake()
    {
        movement = GetComponent<OnlyMovement>();
        growthShrinkLogic = GetComponent<GrowthShrinkLogic>();
        boostImage.sprite = inactiveSprite;
    }

    public void ForceDisableBoost(bool value)
    {
        forceDisableBoost = value;

        if (value)
        {
            DeActivateBoost();
        }
    }

    public void SetBoostVisual(bool active)
    {
        boostImage.sprite = active ? activeSprite : inactiveSprite;

        if (snakeVFX != null)
        {
            snakeVFX.SetBoostVFX(active);
        }
    }
    public void ActivateBoost()
    {
        if (isBoosting) return;
        isBoosting = true;
        movement.BoostSpeed(boostMultiplier);
        growthShrinkLogic.SetBoost();
        SetBoostVisual(true);
    }

    public void DeActivateBoost()
    {
        if (!isBoosting) return;
        isBoosting = false;
        movement.ResetSpeed();
        growthShrinkLogic.DeBoost();
        SetBoostVisual(false);
    }

    private void PoopFood()
    {
        if (growthShrinkLogic.GetSegments().Count <= 5)
            return;

        if (growthShrinkLogic.GetStoredFood() <= 0)
            return;

        Transform tailPoint = growthShrinkLogic.GetTailPoint();

        Vector3 spawnPos = tailPoint.position - tailPoint.forward * 0.9f;
        Quaternion spawnRot = tailPoint.rotation;

        growthShrinkLogic.RemoveStoredFood(1);

        LeanPool.Spawn(poopFoodPrefab,spawnPos,spawnRot);
    }

    //private void Update()
    //{
    //    if (!forceDisableBoost && Input.GetKey(KeyCode.Space) && progressUI.HasEnergy())
    //    {
    //        ActivateBoost();
    //        //Energy drain logic
    //        drainTimer += Time.deltaTime;
    //        if (drainTimer >= drainInterval)
    //        {
    //            growthShrinkLogic.ConsumeBoostEnergy();
    //            progressUI.RemoveProgress(0);
    //            drainTimer = 0f;
    //        }

    //        //Poop food spawn logic
    //        poopTimer += Time.deltaTime;
    //        if (poopTimer >= poopInterval)
    //        {
    //            PoopFood();
    //            poopTimer = 0f;
    //        }
    //    }
    //    else
    //    {
    //        DeActivateBoost();
    //        drainTimer = 0f;
    //        poopTimer = 0f;
    //    }
    //}
}


