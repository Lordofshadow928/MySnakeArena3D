using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnakeBoostVisual : MonoBehaviour
{
    [SerializeField] private Image boostImage;

    [SerializeField] private Sprite inactiveSprite;
    [SerializeField] private Sprite activeSprite;

    [SerializeField] private SnakeParticleVFX snakeVFX;

    private SnakeBoost boost;

    private void Awake()
    {
        boost = GetComponent<SnakeBoost>();
    }

    private void OnEnable()
    {
        boost.OnBoostChanged += UpdateVisual;
    }

    private void OnDisable()
    {
        boost.OnBoostChanged -= UpdateVisual;
    }

    private void UpdateVisual(bool active)
    {
        boostImage.sprite = active ? activeSprite : inactiveSprite;

        if (snakeVFX != null)
        {
            snakeVFX.SetBoostVFX(active);
        }
    }

}
