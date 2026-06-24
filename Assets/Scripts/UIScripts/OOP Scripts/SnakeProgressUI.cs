using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SnakeProgressUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SnakeEnergy energy;
    [SerializeField] private TMP_Text progressText;
    [SerializeField] private Image fillImage;
    [SerializeField] private ResultHandler result;
    private bool hasWon;
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

        if (!hasWon && percent >= 1f)
        {
            hasWon = true;
            result.HandleResult();
        }
    }
   
}
