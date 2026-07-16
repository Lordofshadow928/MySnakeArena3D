using UnityEngine;
using UnityEngine.UI;

public class MenuDragEffect : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform canvasRect;

    [SerializeField] private RectTransform playButton;
    [SerializeField] private RectTransform powerupButton;
    [SerializeField] private RectTransform shopButton;

    [Header("Drag")]
    [SerializeField] private float maxDragDistance = 100f;

    [Header("Animation")]
    [SerializeField] private float outsidePadding = 50f;
    [SerializeField] private float returnSpeed = 10f;

    private Vector2 dragStart;

    private bool dragging;
    private bool returning;

    private float powerupStartX;
    private float shopStartX;

    private float powerupEndX;
    private float shopEndX;

    private Vector3 playStartScale;

    private void Start()
    {
        // Remember original positions
        powerupStartX = powerupButton.anchoredPosition.x;
        shopStartX = shopButton.anchoredPosition.x;
        playStartScale = playButton.localScale;

        // Calculate off-screen positions
        float halfCanvasWidth = canvasRect.rect.width * 0.5f;

        powerupEndX = -halfCanvasWidth - powerupButton.rect.width * 0.5f - outsidePadding;

        shopEndX = halfCanvasWidth + shopButton.rect.width * 0.5f + outsidePadding;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragging = true;
            returning = false;

            dragStart = Input.mousePosition;
        }

        if (dragging && Input.GetMouseButton(0))
        {
            float deltaX = Mathf.Abs(Input.mousePosition.x - dragStart.x);
            float deltaY = Mathf.Abs(Input.mousePosition.y - dragStart.y);

            // Only react if the drag is mostly vertical
            if (deltaY > deltaX)
            {
                float t = Mathf.Clamp01(deltaY / maxDragDistance);

                SetButtonPosition(powerupButton, Mathf.Lerp(powerupStartX, powerupEndX, t));

                SetButtonPosition(shopButton, Mathf.Lerp(shopStartX, shopEndX, t));

                playButton.localScale = Vector3.Lerp( playStartScale, Vector3.zero,t);
            }
        }

        if (dragging && Input.GetMouseButtonUp(0))
        {
            dragging = false;
            returning = true;
        }

        if (returning)
        {
            float lerp = returnSpeed * Time.deltaTime;

            float powerX = Mathf.Lerp( powerupButton.anchoredPosition.x, powerupStartX, lerp);

            float shopX = Mathf.Lerp( shopButton.anchoredPosition.x, shopStartX, lerp);

            SetButtonPosition(powerupButton, powerX);
            SetButtonPosition(shopButton, shopX);

            playButton.localScale = Vector3.Lerp( playButton.localScale, playStartScale, lerp);

            if (Mathf.Abs(powerX - powerupStartX) < 0.5f &&
                Mathf.Abs(shopX - shopStartX) < 0.5f &&
                Vector3.Distance(playButton.localScale, playStartScale) < 0.01f)
            {
                SetButtonPosition(powerupButton, powerupStartX);
                SetButtonPosition(shopButton, shopStartX);
                playButton.localScale = playStartScale;

                returning = false;
            }
        }
    }

    private void SetButtonPosition(RectTransform rect, float x)
    {
        Vector2 pos = rect.anchoredPosition;
        pos.x = x;
        rect.anchoredPosition = pos;
    }
}