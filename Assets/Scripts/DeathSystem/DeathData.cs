using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DeathData
{
    public int foodCount;
    public Vector3 position;

    public DeathData(int foodCount, Vector3 position)
    {
        this.foodCount = foodCount;
        this.position = position;
    }
}
