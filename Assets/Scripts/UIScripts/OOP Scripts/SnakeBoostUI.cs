using UnityEngine;
using UnityEngine.UI;

public class SnakeBoostUI : MonoBehaviour
{
    [SerializeField] private SnakeBoost boost;
    [SerializeField] private Sprite activeSprite;
    [SerializeField] private Sprite inactiveSprite;
    private Image boostImage;
    private void Awake()
    {
        if (boostImage == null)
            boostImage = UIManager.Instance.LightningBoostImage;
    }
    private void Start()
    {
        if (boost == null)
            boost = FindFirstObjectByType<SnakeBoost>();

        if (boost != null)
            boost.OnBoostChanged += UpdateUI;

        if (boost != null)
            UpdateUI(boost.IsBoosting);
    }
    private void OnDestroy()
    {
        if (boost != null)
            boost.OnBoostChanged -= UpdateUI;
    }

    private void UpdateUI(bool active)
    {
        boostImage.sprite = active ? activeSprite : inactiveSprite;
    }
}
