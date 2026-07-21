using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeUIPreview : MonoBehaviour
{
    [Header("Current Preview")]
    [SerializeField] private Transform headRoot;
    [SerializeField] private Transform bodyRoot;
    [SerializeField] private Transform tailRoot;
    [SerializeField] private LayerMask previewLayer;
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
        SetLayerRecursively(currentHead, LayerMask.NameToLayer("SnakePreview"));
        currentBody = Instantiate(skin.bodyPrefab, bodyRoot.position, bodyRoot.rotation, transform);
        SetLayerRecursively(currentBody, LayerMask.NameToLayer("SnakePreview"));
        currentTail = Instantiate(skin.tailPrefab, tailRoot.position, tailRoot.rotation, transform);
        SetLayerRecursively(currentTail, LayerMask.NameToLayer("SnakePreview"));
    }
    private void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }
}
