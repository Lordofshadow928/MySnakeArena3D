using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeEnergy : MonoBehaviour
{
    [SerializeField] private int maxEnergy = 400;

    public int CurrentEnergy { get; private set; }

    public event Action<int, int> OnEnergyChanged;

    public void AddEnergy(int amount)
    {
        CurrentEnergy += amount;
        CurrentEnergy = Mathf.Clamp(CurrentEnergy, 0, maxEnergy);

        OnEnergyChanged?.Invoke(CurrentEnergy, maxEnergy);
    }

    public void RemoveEnergy(int amount)
    {
        CurrentEnergy -= amount;
        CurrentEnergy = Mathf.Clamp(CurrentEnergy, 0, maxEnergy);

        OnEnergyChanged?.Invoke(CurrentEnergy, maxEnergy);
    }

}
