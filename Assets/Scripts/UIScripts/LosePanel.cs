using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LosePanel : ResultHandler
{
    [SerializeField] private GameObject panel;
    [SerializeField] private ResultFoodUI resultFoodUI;
    public override void HandleResult()
    {
        Time.timeScale = 0f;
        resultFoodUI.ShowLose();
        panel.SetActive(true);
    }
}
