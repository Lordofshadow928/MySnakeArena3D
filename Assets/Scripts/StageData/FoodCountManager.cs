using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.SceneManagement;

//public class FoodCountManager : MonoBehaviour
//{
//    [SerializeField] private FoodCountData[] stages;
//    public static FoodCountManager Instance;

//    public int TotalFruitEaten => PlayerPrefs.GetInt("TotalFruit", 0);
//    public int HighestUnlockedStage => PlayerPrefs.GetInt("HighestStage", 1);

//    private void Awake()
//    {
//        if (Instance == null)
//        {
//            Instance = this;
//            DontDestroyOnLoad(gameObject);
//            if (!PlayerPrefs.HasKey("HighestStage"))
//                PlayerPrefs.SetInt("HighestStage", 1);
//        }
//        else
//        {
//            Destroy(gameObject);
//        }
//    }
//    public int GetBestProgress()
//    {
//        string key = $"LevelBest_{SceneManager.GetActiveScene().buildIndex}";
//        return PlayerPrefs.GetInt(key, 0);
//    }
//    public bool SaveBestProgress(int percent)
//    {
//        string key = $"LevelBest_{SceneManager.GetActiveScene().buildIndex}";
//        int best = PlayerPrefs.GetInt(key, 0);

//        if (percent > best)
//        {
//            PlayerPrefs.SetInt(key, percent);
//            PlayerPrefs.Save();
//            return true;
//        }

//        return false;
//    }

//    public void AddFruit(int amount)
//    {
//        int totalFruit = TotalFruitEaten + amount;
//        PlayerPrefs.SetInt("TotalFruit", totalFruit);
//        CheckStageUnlock(totalFruit);
//    }

//    public void CheckStageUnlock(int totalFruit)
//    {
//        int highestUnlocked = HighestUnlockedStage;
//        foreach (FoodCountData stage in stages)
//        {
//            if (totalFruit >= stage.requiredFruit && stage.stageIndex > highestUnlocked)
//            {
//                highestUnlocked = stage.stageIndex;
//            }
//        }
//        if (highestUnlocked > HighestUnlockedStage)
//        {
//            UnlockStage(highestUnlocked);
//        }
//    }

//    private void UnlockStage(int stage)
//    {
//        PlayerPrefs.SetInt("HighestStage", stage);
//        PlayerPrefs.SetInt("PendingUnlockStage", stage);
//        PlayerPrefs.Save();
//    }
//}

public class FoodCountManager : MonoBehaviour
{
    [SerializeField] private FoodCountData[] stages;

    public static FoodCountManager Instance;

    // Progress toward the NEXT stage unlock
    public int CurrentFruitProgress => PlayerPrefs.GetInt("CurrentFruitProgress", 0);

    // Stage 1 is unlocked by default
    public int HighestUnlockedStage => PlayerPrefs.GetInt("HighestStage", 1);

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (!PlayerPrefs.HasKey("HighestStage"))
                PlayerPrefs.SetInt("HighestStage", 1);

            if (!PlayerPrefs.HasKey("CurrentFruitProgress"))
                PlayerPrefs.SetInt("CurrentFruitProgress", 0);

            PlayerPrefs.Save();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #region Best Progress

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

    #endregion

    public void AddFruit(int amount)
    {
        int progress = CurrentFruitProgress + amount;

        PlayerPrefs.SetInt("CurrentFruitProgress", progress);

        CheckStageUnlock();

        PlayerPrefs.Save();
    }

    private void CheckStageUnlock()
    {
        int nextStage = HighestUnlockedStage + 1;

        FoodCountData stageData = GetStageData(nextStage);

        if (stageData == null)
            return;

        if (CurrentFruitProgress >= stageData.requiredFruit)
        {
            UnlockStage(nextStage);
        }
    }

    private FoodCountData GetStageData(int stageIndex)
    {
        foreach (FoodCountData stage in stages)
        {
            if (stage.stageIndex == stageIndex)
                return stage;
        }

        return null;
    }

    private void UnlockStage(int stageIndex)
    {
        PlayerPrefs.SetInt("HighestStage", stageIndex);

        // Reset progress toward the next stage
        PlayerPrefs.SetInt("CurrentFruitProgress", 0);

        // Used by menu animation
        PlayerPrefs.SetInt("PendingUnlockStage", stageIndex);

        PlayerPrefs.Save();

        // Refresh the menu immediately if it exists
        if (MenuProgressManager.Instance != null)
        {
            MenuProgressManager.Instance.UpdateProgress();
        }

        Debug.Log($"Stage {stageIndex} Unlocked!");
    }

    public int GetNextStageRequirement()
    {
        FoodCountData stage = GetStageData(HighestUnlockedStage + 1);

        if (stage == null)
            return 0;

        return stage.requiredFruit;
    }
   
    [ContextMenu("Clear All Progress")]
    public void ClearAllProgress()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        Debug.Log("DeleteAll called.");

        Debug.Log("HighestStage = " + PlayerPrefs.GetInt("HighestStage", -1));
        Debug.Log("CurrentFruitProgress = " + PlayerPrefs.GetInt("CurrentFruitProgress", -1));
        Debug.Log("LevelBest_0 = " + PlayerPrefs.GetInt("LevelBest_0", -1));
        Debug.Log("LevelBest_1 = " + PlayerPrefs.GetInt("LevelBest_1", -1));
        Debug.Log("LevelBest_2 = " + PlayerPrefs.GetInt("LevelBest_2", -1));
    }
}