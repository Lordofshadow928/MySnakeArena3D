using UnityEngine;
using UnityEngine.SceneManagement;

public class FoodCountManager : MonoBehaviour
{
    public static FoodCountManager Instance;

    public int HighestUnlockedStage => PlayerPrefs.GetInt("HighestStage", 1);

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (!PlayerPrefs.HasKey("HighestStage"))
            {
                PlayerPrefs.SetInt("HighestStage", 1);
                PlayerPrefs.Save();
            }
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

    #region Fruit Progress

    public int GetFruitCount(FruitType fruit)
    {
        return PlayerPrefs.GetInt(fruit.ToString(), 0);
    }

    public void AddFruit(FruitType fruit, int amount)
    {
        string key = fruit.ToString();

        int current = PlayerPrefs.GetInt(key, 0);
        current += amount;

        PlayerPrefs.SetInt(key, current);

        CheckStageUnlock();

        PlayerPrefs.Save();
    }

    #endregion

    private void CheckStageUnlock()
    {
        int nextStage = HighestUnlockedStage + 1;

        FoodCountData stageData = LevelManager.Instance.GetStageData(nextStage);

        if (stageData == null)
            return;

        if (GetFruitCount(stageData.fruitType) >= stageData.requiredFruit)
        {
            UnlockStage(nextStage);
        }
    }

    private void UnlockStage(int stageIndex)
    {
        PlayerPrefs.SetInt("HighestStage", stageIndex);
        PlayerPrefs.SetInt("PendingUnlockStage", stageIndex);
        PlayerPrefs.Save();

        if (MenuProgressManager.Instance != null)
        {
            MenuProgressManager.Instance.UpdateProgress();
        }

        Debug.Log($"Stage {stageIndex} Unlocked!");
    }

    public int GetNextStageRequirement()
    {
        FoodCountData stageData = LevelManager.Instance.GetStageData(HighestUnlockedStage + 1);

        if (stageData == null)
            return 0;

        return stageData.requiredFruit;
    }

    [ContextMenu("Clear All Progress")]
    public void ClearAllProgress()
    {
        PlayerPrefs.DeleteAll();

        PlayerPrefs.SetInt("HighestStage", 1);
        PlayerPrefs.Save();

        Debug.Log("All progress cleared.");
    }
}