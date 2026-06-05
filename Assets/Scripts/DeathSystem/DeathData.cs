using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DeathData
{
    public int bodyCount;
    public Vector3 position;

    public DeathData(int bodyCount, Vector3 position)
    {
        this.bodyCount = bodyCount;
        this.position = position;
    }
}
