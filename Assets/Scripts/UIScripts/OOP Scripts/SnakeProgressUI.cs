using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SnakeProgressUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SnakeEnergy energy;
    [SerializeField] private TMP_Text progressText;
    [SerializeField] private TMP_Text bestProgressText;
    [SerializeField] private Image fillImage;
    [SerializeField] private ResultHandler result;
    private bool hasWon;
    private int bestProgress;

    private void Start()
    {
        bestProgress = FoodCountManager.Instance.GetBestProgress();
        bestProgressText.text = $"Best {bestProgress}%";
    }
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

        int currentPercent = Mathf.RoundToInt(percent * 100);

        progressText.text = $"{currentPercent}%";

        if (FoodCountManager.Instance.SaveBestProgress(currentPercent))
        {
            bestProgress = currentPercent;
            bestProgressText.text = $"Best {bestProgress}%";
        }

        if (!hasWon && percent >= 1f)
        {
            hasWon = true;
            result.HandleResult();
        }
    }

}
