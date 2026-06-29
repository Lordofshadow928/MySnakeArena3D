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
    private string progressKey;
    private int bestPercent;

    private void Start()
    {
        progressKey = $"LevelBest_{SceneManager.GetActiveScene().buildIndex}";

        bestPercent = PlayerPrefs.GetInt(progressKey, 0);
        bestProgressText.text = $"Best {bestPercent}%";
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

        progressText.text = $"{Mathf.RoundToInt(percent * 100)}%";
        int currentPercent = Mathf.RoundToInt(percent * 100);
        if (currentPercent > bestPercent)
        {
            bestPercent = currentPercent;
            PlayerPrefs.SetInt(progressKey, bestPercent);
            PlayerPrefs.Save();

            bestProgressText.text = $"Best {bestPercent}%";
        }

        if (!hasWon && percent >= 1f)
        {
            hasWon = true;
            result.HandleResult();
        }
    }
   
}
