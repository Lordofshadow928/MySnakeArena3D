using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RequirementUI : MonoBehaviour
{
    [SerializeField] private Image stageImage;
    [SerializeField] private TMP_Text requirementText;

    [SerializeField] private Vector3 offset;

    public void Show(MenuIsland island)
    {
        FoodCountData data = island.StageData;

        stageImage.sprite = data.requirementSprite;

        requirementText.text = $"{FoodCountManager.Instance.CurrentFruitProgress}/{data.requiredFruit}";

        transform.position = island.LockController.transform.position + offset;

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}