using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeUITeleport : MonoBehaviour
{
    [SerializeField] private ParticleSystem disappearSmokePrefab;
    [SerializeField] private ParticleSystem appearSmokePrefab;
    [SerializeField] private GameObject snakeVisual; // SnakeShowUp

    private bool teleporting;

    public void TeleportTo(Vector3 newPos)
    {
        if (!teleporting)
            StartCoroutine(TeleportRoutine(newPos));
    }

    IEnumerator TeleportRoutine(Vector3 newPos)
    {
        teleporting = true;

        // Smoke at old location
        Instantiate(disappearSmokePrefab, transform.position, Quaternion.identity);

        // Hide snake
        snakeVisual.SetActive(false);

        // Small delay for effect
        yield return new WaitForSeconds(0.25f);

        // Teleport
        transform.position = newPos;

        // Smoke at new location
        Instantiate(appearSmokePrefab, transform.position, Quaternion.identity);

        // Show snake
        snakeVisual.SetActive(true);

        teleporting = false;
    }
}
