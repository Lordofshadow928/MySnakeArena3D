using System;
using UnityEngine;

public class SnakeShrink : MonoBehaviour
{
    [SerializeField] private int foodsPerShrink = 4;

    private int shrinkCounter;
    private SnakeBody body;
    private SnakeEnergy energy;
    private SnakeBoost boost;
    public event Action OnShrink;

    private void Awake()
    {
        body = GetComponent<SnakeBody>();
        boost = GetComponent<SnakeBoost>();
        energy = GetComponent<SnakeEnergy>();
    }

    public void ConsumeBoostEnergy()
    {
        if(!boost.IsBoosting) return;
        if (boost.IgnoreEnergyCost) return;
        shrinkCounter++;
        energy.RemoveEnergy(1);
        if (shrinkCounter >= foodsPerShrink)
        {
            shrinkCounter = 0;
            body.RemoveSegment();
            OnShrink?.Invoke();
        }
    }
}