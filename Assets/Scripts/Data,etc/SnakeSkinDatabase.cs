using UnityEngine;

[CreateAssetMenu(menuName = "Snake/Skin Database")]
public class SnakeSkinDatabase : ScriptableObject
{
    public SnakeSkinData[] skins;
    public int Count => skins.Length;
    public SnakeSkinData GetSkin(int index)
    {
        if (skins == null || skins.Length == 0)
            return null;

        index = Mathf.Clamp(index, 0, skins.Length - 1);

        return skins[index];
    }
    public SnakeSkinData GetSkinByID(string skinID)
    {
        foreach (SnakeSkinData skin in skins)
        {
            if (skin.skinID == skinID)
                return skin;
        }

        return null;
    }
}