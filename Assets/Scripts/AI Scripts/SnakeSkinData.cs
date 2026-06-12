using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SnakeSkinData
{
    public string skinName;

    [Header("Prefabs")]
    public GameObject headPrefab;
    public GameObject bodyPrefab;
    public GameObject tailPrefab;
}
