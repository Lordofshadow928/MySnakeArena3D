using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeUIPreview : MonoBehaviour
{
    [Header("Current Preview")]
    [SerializeField] private Transform headRoot;
    [SerializeField] private Transform bodyRoot;
    [SerializeField] private Transform tailRoot;

    private GameObject currentHead;
    private GameObject currentBody;
    private GameObject currentTail;
    private void Start()
    {
        ShowSkin(PlayerSkinManager.Instance.EquippedSkin);
    }
    public void ShowSkin(SnakeSkinData skin)
    {
        if (skin == null)
            return;

        if (currentHead != null)
            Destroy(currentHead);

        if (currentBody != null)
            Destroy(currentBody);

        if (currentTail != null)
            Destroy(currentTail);

        currentHead = Instantiate(skin.headPrefab, headRoot.position, headRoot.rotation, transform);

        currentBody = Instantiate(skin.bodyPrefab, bodyRoot.position, bodyRoot.rotation, transform);

        currentTail = Instantiate(skin.tailPrefab, tailRoot.position, tailRoot.rotation, transform);
    }
}
