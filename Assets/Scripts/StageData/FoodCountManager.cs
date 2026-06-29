using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FoodCountManager : MonoBehaviour
{
    [SerializeField] private FoodCountData[] stages;
    public static FoodCountManager Instance;

    public int TotalFruitEaten => PlayerPrefs.GetInt("TotalFruit", 0);
    public int HighestUnlockedStage => PlayerPrefs.GetInt("HighestStage", 1);

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            if (!PlayerPrefs.HasKey("HighestStage"))
                PlayerPrefs.SetInt("HighestStage", 1);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public int GetBestProgress()
    {
        string key = $"LevelBest_{SceneManager.GetActiveScene().buildIndex}";
        return PlayerPrefs.GetInt(key, 0);
    }
    public bool SaveBestProgress(int percent)
    {
        string key = $"LevelBest_{SceneManager.GetActiveScene().buildIndex}";
        int best = PlayerPrefs.GetInt(key, 0);

        if (percent > best)
        {
            PlayerPrefs.SetInt(key, percent);
            PlayerPrefs.Save();
            return true;
        }

        return false;
    }

    public void AddFruit(int amount)
    {
        int totalFruit = TotalFruitEaten + amount;
        PlayerPrefs.SetInt("TotalFruit", totalFruit);
        CheckStageUnlock(totalFruit);
    }

    public void CheckStageUnlock(int totalFruit)
    {
        int highestUnlocked = HighestUnlockedStage;
        foreach (FoodCountData stage in stages)
        {
            if (totalFruit >= stage.requiredFruit && stage.stageIndex > highestUnlocked)
            {
                highestUnlocked = stage.stageIndex;
            }
        }
        if (highestUnlocked > HighestUnlockedStage)
        {
            UnlockStage(highestUnlocked);
        }
    }

    private void UnlockStage(int stage)
    {
        PlayerPrefs.SetInt("HighestStage", stage);
        PlayerPrefs.SetInt("PendingUnlockStage", stage);
        PlayerPrefs.Save();
    }
}
