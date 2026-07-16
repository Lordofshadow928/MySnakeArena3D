using UnityEngine;

public class SnakeSkinController : MonoBehaviour
{
    public SnakeSkinData CurrentSkin { get; private set; }
    public void ApplySkin(SnakeSkinData skin)
    {
        if (skin == null)
            return;

        CurrentSkin = skin;
    }
}