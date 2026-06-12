using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeSkinController : MonoBehaviour
{
    public SnakeSkinData CurrentSkin { get; private set; }

    public void ApplySkin(SnakeSkinData skin)
    {
        CurrentSkin = skin;

        // Spawn head here
    }
}
