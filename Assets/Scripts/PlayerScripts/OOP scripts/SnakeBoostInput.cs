using UnityEngine;

public class SnakeBoostInput : MonoBehaviour
{
    [SerializeField] private SnakeBoost boost;
    [SerializeField] private SnakeEnergy energy;
    [SerializeField] private SnakeInvincible2 invincible;

    private void Update()
    {
        bool canBoost = energy.CurrentEnergy > 0 || invincible.IsInvincible;

        if (Input.GetKey(KeyCode.Space) && canBoost)
        {
            boost.ActivateBoost();
        }
        else
        {
            boost.DeactivateBoost();
        }
    }
}
