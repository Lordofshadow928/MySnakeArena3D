using UnityEngine;

public class MenuIsland : MonoBehaviour
{
    [Header("Stage Data")]
    [SerializeField] private FoodCountData stageData;

    [Header("Settings")]
    [SerializeField] private bool useAutomaticLock = true;

    public FoodCountData StageData => stageData;

    public Transform PositionForSnake { get; private set; }
    public GameObject LockController { get; private set; }

    private GameObject fruitsPositions;

    public bool IsLocked => LockController.activeSelf;

    private void Awake()
    {
        PositionForSnake = transform.Find("PositionForSnake");
        LockController = transform.Find("LockController").gameObject;
        fruitsPositions = transform.Find("FruitsPositions").gameObject;
    }

    private void Start()
    {
        if (useAutomaticLock)
            RefreshLock();
    }

    public void RefreshLock()
    {
        bool unlocked = stageData.stageIndex <= FoodCountManager.Instance.HighestUnlockedStage;

        LockController.SetActive(!unlocked);

        if (fruitsPositions != null)
            fruitsPositions.SetActive(unlocked);
    }
}