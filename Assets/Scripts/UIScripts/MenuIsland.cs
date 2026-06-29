using UnityEngine;

public class MenuIsland : MonoBehaviour
{
    [SerializeField] private int stageIndex;
    [SerializeField] private bool useAutomaticLock = false;

    public Transform PositionForSnake { get; private set; }
    public GameObject LockController { get; private set; }

    private GameObject fruitsPositions;
    private GameObject requirementUI;

    public bool IsLocked => LockController.activeSelf;

    private void Awake()
    {
        PositionForSnake = transform.Find("PositionForSnake");
        LockController = transform.Find("LockController").gameObject;

        fruitsPositions = transform.Find("FruitsPositions").gameObject;
        requirementUI = transform.Find("RequirementUI").gameObject;
    }

    private void Start()
    {
        requirementUI.SetActive(false);

        if (useAutomaticLock)
            RefreshLock();
    }

    public void RefreshLock()
    {
        bool unlocked = stageIndex <= FoodCountManager.Instance.HighestUnlockedStage;

        LockController.SetActive(!unlocked);

        fruitsPositions.SetActive(unlocked);

        // Always hide popup when refreshing
        requirementUI.SetActive(false);
    }

    public void ShowRequirement()
    {
        if (IsLocked)
            requirementUI.SetActive(true);
    }

    public void HideRequirement()
    {
        requirementUI.SetActive(false);
    }
}