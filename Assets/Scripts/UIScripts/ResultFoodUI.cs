using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultFoodUI : MonoBehaviour
{
    [Header("Win")]
    [SerializeField] private TMP_Text winFoodText;

    [Header("Lose")]
    [SerializeField] private TMP_Text loseFoodText;
    private SnakeFoodStorage storage;
    public void Initialize(SnakeFoodStorage playerStorage)
    {
        storage = playerStorage;
    }
    public void ShowWin()
    {
        winFoodText.text = $"{storage.StoredFood}";
    }

    public void ShowLose()
    {
        loseFoodText.text = storage.StoredFood.ToString();
    }
}
