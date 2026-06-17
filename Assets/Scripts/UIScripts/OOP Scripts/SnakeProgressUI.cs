using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SnakeProgressUI : MonoBehaviour
{
    [SerializeField] private SnakeEnergy energy;
    [SerializeField] private TMP_Text progressText;
    [SerializeField] private Image fillImage;

    private void OnEnable()
    {
        energy.OnEnergyChanged += UpdateUI;
    }

    private void OnDisable()
    {
        energy.OnEnergyChanged -= UpdateUI;
    }

    private void UpdateUI(int current, int max)
    {
        float percent = (float)current / max;

        fillImage.fillAmount = percent;

        progressText.text = $"{Mathf.RoundToInt(percent * 100)}%";
    }
}
