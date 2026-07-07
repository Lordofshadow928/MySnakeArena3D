using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField] private LevelData[] levelDatas;
    [SerializeField] private FoodCountData[] stageDatas;
    private int currentStageIndex = 1;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public LevelData CurrentLevelData => GetLevelData(currentStageIndex);

    public void SetCurrentStage(int stageIndex)
    {
        currentStageIndex = stageIndex;
    }
    public LevelData GetLevelData(int stageIndex)
    {
        if (stageIndex < 1 || stageIndex > levelDatas.Length)
            return null;

        return levelDatas[stageIndex - 1];
    }

    public FoodCountData GetStageData(int stageIndex)
    {
        if (stageIndex < 1 || stageIndex > stageDatas.Length)
            return null;

        return stageDatas[stageIndex - 1];
    }
}