using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinPanel : ResultHandler
{
    [SerializeField] private GameObject panel;
    [SerializeField] private ResultFoodUI resultFoodUI;
    public override void HandleResult()
    {
        Time.timeScale = 0f;
        resultFoodUI.ShowWin();
        panel.SetActive(true);
    }
}
