using UnityEngine;

public class SnakeUI : MonoBehaviour
{
    [SerializeField] private SnakeProgressUI progressUI;

    public void AddProgress()
    {
        progressUI.AddProgress(1);
    }

    public void RemoveProgress()
    {
        progressUI.RemoveProgress(1);
    }
}
