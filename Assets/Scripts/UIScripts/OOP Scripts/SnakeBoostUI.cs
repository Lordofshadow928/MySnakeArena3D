using UnityEngine;
using UnityEngine.UI;

public class SnakeBoostUI : MonoBehaviour
{
    [SerializeField] private SnakeBoost boost;
    [SerializeField] private Image boostImage;
    [SerializeField] private Sprite activeSprite;
    [SerializeField] private Sprite inactiveSprite;

    private void OnEnable()
    {
        boost.OnBoostChanged += UpdateUI;
    }

    private void OnDisable()
    {
        boost.OnBoostChanged -= UpdateUI;
    }

    private void UpdateUI(bool active)
    {
        boostImage.sprite = active ? activeSprite : inactiveSprite;
    }
}
