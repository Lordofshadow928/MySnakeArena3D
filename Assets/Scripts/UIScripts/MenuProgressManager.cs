using UnityEngine;

public class MenuProgressManager : MonoBehaviour
{
    public static MenuProgressManager Instance { get; private set; }

    [SerializeField] private MenuIsland[] islands;

    public int MaxReachableIndex { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateProgress();
    }

    public void UpdateProgress()
    {
        // Refresh every island first
        foreach (MenuIsland island in islands)
        {
            island.RefreshLock();
            island.gameObject.SetActive(true);
        }

        // Find the first locked island
        MaxReachableIndex = islands.Length - 1;

        for (int i = 0; i < islands.Length; i++)
        {
            if (islands[i].IsLocked)
            {
                MaxReachableIndex = i;
                break;
            }
        }

        // Hide islands after the first locked one
        for (int i = MaxReachableIndex + 1; i < islands.Length; i++)
        {
            islands[i].gameObject.SetActive(false);
        }
    }
}