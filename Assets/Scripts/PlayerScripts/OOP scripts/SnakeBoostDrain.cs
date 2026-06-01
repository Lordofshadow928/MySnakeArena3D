using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBoostDrain : MonoBehaviour
{
    [SerializeField] private float drainInterval = 0.3f;

    private float timer;

    private SnakeBoost boost;
    private SnakeShrink shrink;

    private void Awake()
    {
        boost = GetComponent<SnakeBoost>();
        shrink = GetComponent<SnakeShrink>();
    }

    private void Update()
    {
        if (!boost.IsBoosting)
        {
            timer = 0f;
            return;
        }

        timer += Time.deltaTime;

        if (timer >= drainInterval)
        {
            timer = 0f;

            shrink.ConsumeBoostEnergy();
        }
    }
}
