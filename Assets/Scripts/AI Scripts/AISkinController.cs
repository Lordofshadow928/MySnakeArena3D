using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISkinController : MonoBehaviour
{
    [Header("Available Skins")]
    [SerializeField] private GameObject[] skins;

    private void Start()
    {
        ApplyRandomSkin();
    }

    public void ApplySkin(int skinIndex)
    {
        if (skinIndex < 0 || skinIndex >= skins.Length)
            return;

        for (int i = 0; i < skins.Length; i++)
        {
            skins[i].SetActive(i == skinIndex);
        }
    }

    public void ApplyRandomSkin()
    {
        if (skins.Length == 0)
            return;

        int randomIndex = Random.Range(0, skins.Length);
        ApplySkin(randomIndex);
    }
}
