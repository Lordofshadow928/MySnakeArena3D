using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSnakeSkin : MonoBehaviour
{
    private GameObject currentHead;

    private void Awake()
    {
        SnakeSkinController controller = GetComponent<SnakeSkinController>();

        SnakeSkinData selectedSkin = PlayerSkinManager.Instance.EquippedSkin
            ;
        controller.ApplySkin(selectedSkin);

        currentHead = Instantiate(selectedSkin.headPrefab, transform);

        currentHead.transform.localPosition = Vector3.zero;
        currentHead.transform.localRotation = Quaternion.identity;
        currentHead.transform.localScale = Vector3.one;
    }
}
