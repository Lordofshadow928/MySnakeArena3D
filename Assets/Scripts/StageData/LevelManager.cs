using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField] private LevelData[] levelDatas;
    [SerializeField] private FoodCountData[] stageDatas;

    [SerializeField] private Transform mapRoot;

    private GameObject currentMap;

    private int currentStageIndex = 1;

    public LevelData CurrentLevelData => GetLevelData(currentStageIndex);

    public MapData CurrentMapData { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        SpawnCurrentMap();
    }

    public void SetCurrentStage(int stageIndex)
    {
        currentStageIndex = stageIndex;
    }

    void SpawnCurrentMap()
    {
        LevelData level = CurrentLevelData;

        currentMap = Instantiate(level.mapPrefab, mapRoot);

        CurrentMapData = currentMap.GetComponentInChildren<MapData>();
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