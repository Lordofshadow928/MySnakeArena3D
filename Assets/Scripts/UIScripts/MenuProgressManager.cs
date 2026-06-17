using UnityEngine;

public class MenuProgressManager : MonoBehaviour
{
    [SerializeField] private MenuIsland[] islands;

    public int MaxReachableIndex { get; private set; }

    private void Awake()
    {
        UpdateProgress();
    }

    public void UpdateProgress()
    {
        MaxReachableIndex = islands.Length - 1;

        for (int i = 0; i < islands.Length; i++)
        {
            if (islands[i].IsLocked)
            {
                MaxReachableIndex = i;
                break;
            }
        }
        // Disable everything after first locked island
        for (int i = MaxReachableIndex + 1; i < islands.Length; i++)
        {
            islands[i].gameObject.SetActive(false);
        }
    }
}