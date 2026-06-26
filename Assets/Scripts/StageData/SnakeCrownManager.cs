using UnityEngine;

public class SnakeCrownManager : MonoBehaviour
{
    private SnakeCrown[] snakes;
    private SnakeCrown currentLeader;

    private void Start()
    {
        snakes = FindObjectsOfType<SnakeCrown>();
        foreach (SnakeCrown snake in snakes)
        {
            snake.FoodStorage.OnFoodChanged += UpdateCrowns;
        }
        UpdateCrowns();
    }

    private void OnDestroy()
    {
        if (snakes == null) return;
        foreach (SnakeCrown snake in snakes)
        {
            snake.FoodStorage.OnFoodChanged -= UpdateCrowns;
        }
    }

    private void UpdateCrowns()
    {
        if (currentLeader == null)
        {
            foreach (SnakeCrown snake in snakes)
            {
                if (snake.FoodStorage.StoredFood > 0)
                {
                    currentLeader = snake;
                    break;
                }
            }
        }

        foreach (SnakeCrown snake in snakes)
        {
            if (currentLeader == null)
                continue;

            if (snake == currentLeader)
                continue;

            if (snake.FoodStorage.StoredFood >
                currentLeader.FoodStorage.StoredFood)
            {
                currentLeader = snake;
            }
        }

        foreach (SnakeCrown snake in snakes)
        {
            snake.SetCrown(snake == currentLeader);
        }
    }
}