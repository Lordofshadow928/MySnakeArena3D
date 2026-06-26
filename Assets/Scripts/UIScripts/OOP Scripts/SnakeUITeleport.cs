using System.Collections;
using UnityEngine;

public class SnakeUITeleport : MonoBehaviour
{
    [SerializeField] private ParticleSystem disappearSmokePrefab;
    [SerializeField] private ParticleSystem appearSmokePrefab;

    private Renderer[] renderers;
    private bool teleporting;

    private void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();
    }

    public bool TeleportTo(MenuIsland island)
    {
        if (teleporting || island.IsLocked)
            return false;

        StartCoroutine(TeleportRoutine(island.PositionForSnake));
        return true;
    }

    private IEnumerator TeleportRoutine(Transform target)
    {
        teleporting = true;

        if (disappearSmokePrefab != null)
            Instantiate(disappearSmokePrefab, transform.position, Quaternion.identity);

        SetVisible(false);

        yield return new WaitForSeconds(0.25f);

        transform.position = target.position;

        if (appearSmokePrefab != null)
            Instantiate(appearSmokePrefab, transform.position, Quaternion.identity);

        SetVisible(true);

        teleporting = false;
    }

    private void SetVisible(bool visible)
    {
        foreach (Renderer r in renderers)
        {
            if (r != null)
                r.enabled = visible;
        }
    }
}