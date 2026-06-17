using UnityEngine;

public class MenuIsland : MonoBehaviour
{
    public Transform PositionForSnake { get; private set; }
    public GameObject LockController { get; private set; }

    public bool IsLocked => LockController.activeSelf;

    private void Awake()
    {
        PositionForSnake = transform.Find("PositionForSnake");
        LockController = transform.Find("LockController").gameObject;
    }
}