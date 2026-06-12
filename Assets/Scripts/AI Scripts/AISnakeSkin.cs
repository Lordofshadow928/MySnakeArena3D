using UnityEngine;

public class AISnakeSkin : MonoBehaviour
{
    [SerializeField] private SnakeSkinData[] skins;

    private GameObject currentHead;

    private void Awake()
    {
        SnakeSkinController controller = GetComponent<SnakeSkinController>();

        int randomIndex = Random.Range(0, skins.Length);

        SnakeSkinData selectedSkin = skins[randomIndex];

        controller.ApplySkin(selectedSkin);

        currentHead = Instantiate(selectedSkin.headPrefab, transform);

        currentHead.transform.localPosition = Vector3.zero;
        currentHead.transform.localRotation = Quaternion.identity;
        currentHead.transform.localScale = Vector3.one;
    }
}