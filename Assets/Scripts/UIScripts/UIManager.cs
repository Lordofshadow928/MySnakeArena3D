using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private Image lightningBoostImage;

    private void Awake()
    {
        Instance = this;
    }

    public Image LightningBoostImage => lightningBoostImage;
}
