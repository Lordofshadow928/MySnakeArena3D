using UnityEngine;

public class SnakeCrown : MonoBehaviour
{
    [SerializeField] private GameObject crownHolder;

    public SnakeFoodStorage FoodStorage { get; private set; }

    private void Awake()
    {
        FoodStorage = GetComponent<SnakeFoodStorage>();
    }

    public void SetCrown(bool active)
    {
        crownHolder.SetActive(active);
    }
}