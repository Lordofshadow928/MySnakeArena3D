using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinPanel : ResultHandler
{
    [SerializeField] private GameObject panel;
    public override void HandleResult()
    {
        Time.timeScale = 0f;
        panel.SetActive(true);
    }
}
