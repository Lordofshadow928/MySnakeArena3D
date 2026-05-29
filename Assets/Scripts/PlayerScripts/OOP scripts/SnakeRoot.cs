using UnityEngine;

public class SnakeRoot : MonoBehaviour
{
    private SnakeGrowth growth;
    private SnakeShrink shrink;
    private SnakeUI snakeUI;

    private void Awake()
    {
        growth = GetComponent<SnakeGrowth>();
        shrink = GetComponent<SnakeShrink>();
        snakeUI = GetComponent<SnakeUI>();
    }

    private void OnEnable()
    {
        if (growth != null)
        {
            growth.OnGrow += HandleGrow;
        }

        if (shrink != null)
        {
            shrink.OnShrink += HandleShrink;
        }
    }

    private void OnDisable()
    {
        if (growth != null)
        {
            growth.OnGrow -= HandleGrow;
        }

        if (shrink != null)
        {
            shrink.OnShrink -= HandleShrink;
        }
    }

    private void HandleGrow()
    {
        if (snakeUI != null)
        {
            snakeUI.AddProgress();
        }
    }

    private void HandleShrink()
    {
        if (snakeUI != null)
        {
            snakeUI.RemoveProgress();
        }
    }
}