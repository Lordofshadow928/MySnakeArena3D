using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeSpeedBoost : MonoBehaviour
{
    [Header("Boost Settings")]
    public float boostMultiplier = 2f;
    private PlayerMovement movement;
    [SerializeField] private float drainInterval = 0.3f;

    private float drainTimer = 0f;
    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
    }
    public void ActivateBoost()
    {
        movement.BoostSpeed(boostMultiplier);
    }

    public void DeActivateBoost()
    {
        movement.ResetSpeed();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ActivateBoost();
            drainTimer += Time.deltaTime;
            if (drainTimer >= drainInterval)
            {
                movement.ShrinkSnake();
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


