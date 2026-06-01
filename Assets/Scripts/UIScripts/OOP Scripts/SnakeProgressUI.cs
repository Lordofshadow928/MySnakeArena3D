using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


//public class SnakeProgressUI : MonoBehaviour
//{
//    [Header("%Progress TMP")]
//    [SerializeField] private TMP_Text progressText;

//    [Header("Progress UI")]
//    [SerializeField] private Image fillImage;
//    [SerializeField] private int maxProgress = 10;

//    private int currentProgress = 0;

//    public void AddProgress(int amount)
//    {
//        currentProgress += amount;
//        currentProgress = Mathf.Clamp(currentProgress, 0, maxProgress);

//        UpdateBar();
//    }

//    public void RemoveProgress(int amount)
//    {
//        currentProgress -= amount;
//        currentProgress = Mathf.Clamp(currentProgress, 0, maxProgress);

//        UpdateBar();
//    }

//    private void UpdateBar()
//    {
//        float percent = (float)currentProgress / maxProgress;
//        fillImage.fillAmount = percent;

//        int displayPercent = Mathf.RoundToInt(percent * 100);
//        progressText.text = $"{displayPercent}%";
//    }

//    public bool HasEnergy()
//    {
//        return currentProgress > 0;
//    }
//}


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
