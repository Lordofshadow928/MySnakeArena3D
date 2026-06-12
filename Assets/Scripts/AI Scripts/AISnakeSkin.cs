using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISnakeSkin : MonoBehaviour
{
    [SerializeField] private SnakeSkinData[] skins;

    private void Awake()
    {
        SnakeSkinController controller = GetComponent<SnakeSkinController>();

        int randomIndex = Random.Range(0, skins.Length);

        controller.ApplySkin(skins[randomIndex]);
    }
}
