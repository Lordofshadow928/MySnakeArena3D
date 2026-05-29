using System;
using UnityEngine;

public class SnakeShrink : MonoBehaviour
{
    [SerializeField] private int foodsPerShrink = 4;

    private int shrinkCounter;

    private SnakeBody body;

    public bool IsBoosted { get; private set; }

    public event Action OnShrink;

    private void Awake()
    {
        body = GetComponent<SnakeBody>();
    }

    public void StartBoost()
    {
        IsBoosted = true;
    }

    public void StopBoost()
    {
        IsBoosted = false;
    }

    public void ConsumeBoostEnergy()
    {
        shrinkCounter++;

        if (shrinkCounter >= foodsPerShrink)
        {
            shrinkCounter = 0;
            body.RemoveSegment();
            OnShrink?.Invoke();
        }
    }
}