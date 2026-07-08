using UnityEngine;

public class MapData : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;

    public Transform[] SpawnPoints => spawnPoints;
}