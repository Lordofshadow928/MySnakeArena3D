using UnityEngine;
using UnityEngine.UI;

public class ShopPageController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject[] pages;

    [Header("Buttons")]
    [SerializeField] private Button nextButton;
    [SerializeField] private Button previousButton;

    [Header("Indicator")]
    [SerializeField] private RectTransform selectedDot;
    [SerializeField] private RectTransform[] circles;

    [Header("Animation")]
    [SerializeField] private float panelChangedSpeed = 10f;

    private int currentPage;

    private Vector2 targetPosition;

    private void Start()
    {
        currentPage = 0;

        ShowPage(currentPage);

        nextButton.onClick.AddListener(NextPage);
        previousButton.onClick.AddListener(PreviousPage);
    }

    private void Update()
    {
        selectedDot.anchoredPosition = Vector2.Lerp(selectedDot.anchoredPosition, targetPosition, Time.deltaTime * panelChangedSpeed);
    }

    public void NextPage()
    {
        if (currentPage >= pages.Length - 1)
            return;

        currentPage++;

        ShowPage(currentPage);
    }

    public void PreviousPage()
    {
        if (currentPage <= 0)
            return;

        currentPage--;

        ShowPage(currentPage);
    }

    private void ShowPage(int index)
    {
        for (int i = 0; i < pages.Length; i++)
            pages[i].SetActive(i == index);

        targetPosition = circles[index].anchoredPosition;

        previousButton.gameObject.SetActive(index > 0);
        nextButton.gameObject.SetActive(index < pages.Length - 1);
    }
}