using UnityEngine;

[CreateAssetMenu(menuName = "Snake/Skin Database")]
public class SnakeSkinDatabase : ScriptableObject
{
    public SnakeSkinData[] skins;

    public SnakeSkinData GetSkin(int index)
    {
        if (skins == null || skins.Length == 0)
            return null;

        index = Mathf.Clamp(index, 0, skins.Length - 1);

        return skins[index];
    }

    public int Count => skins.Length;
}