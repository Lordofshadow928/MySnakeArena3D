using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeSpeedBoost : MonoBehaviour
{

    [Header("Boost Settings")]
    public float boostMultiplier = 2f;
    public float boostDuration = 2f;

    private Demomovement movement;
    private bool isBoosting = false;

    private float normalMoveSpeed;
    private float normalHeadSpeed;

    private void Awake()
    {
        movement = GetComponent<Demomovement>();

        normalMoveSpeed = movement.GetMoveSpeed();
        normalHeadSpeed = movement.GetHeadSpeed();
    }

    public void ActivateBoost()
    {
        if (!isBoosting)
            StartCoroutine(BoostRoutine());
    }

    private IEnumerator BoostRoutine()
    {
        isBoosting = true;

        movement.SetSpeed(
            normalMoveSpeed * boostMultiplier,
            normalHeadSpeed * boostMultiplier
        );

        yield return new WaitForSeconds(boostDuration);

        movement.SetSpeed(normalMoveSpeed, normalHeadSpeed);

        isBoosting = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ActivateBoost();
        }
    }
}



