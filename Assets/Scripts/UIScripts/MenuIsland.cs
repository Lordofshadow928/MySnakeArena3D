using UnityEngine;

public class MenuIsland : MonoBehaviour
{
    [SerializeField] private int stageIndex;

    public Transform PositionForSnake { get; private set; }
    public GameObject LockController { get; private set; }

    public bool IsLocked => LockController.activeSelf;

    private void Awake()
    {
        PositionForSnake = transform.Find("PositionForSnake");
        LockController = transform.Find("LockController").gameObject;
    }

    private void Start()
    {
        RefreshLock();
    }

    public void RefreshLock()
    {
        bool unlocked = stageIndex <= FoodCountManager.Instance.HighestUnlockedStage;
        LockController.SetActive(!unlocked);
    }
}