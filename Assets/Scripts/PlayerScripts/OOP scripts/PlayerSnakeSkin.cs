using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSnakeSkin : MonoBehaviour
{
    [SerializeField] private SnakeSkinData selectedSkin;

    private void Awake()
    {
        SnakeSkinController controller = GetComponent<SnakeSkinController>();

        controller.ApplySkin(selectedSkin);
    }
}
