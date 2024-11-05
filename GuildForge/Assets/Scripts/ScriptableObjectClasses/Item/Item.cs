using System.Threading;
using UnityEngine;

public class Item : ScriptableObject
{
    public enum ItemType { Weapon, Armor, Accessory }
    public enum ItemRank { S, A, B, C, D }

    public bool inStore;
    public int ID;
    public Texture Icon;
    public string Name;
    public ItemType itemtype;
    public ItemRank itemRank;
    public int Cost;
    public string characterBuilderName;

    public int AtkBonus = 0;
    public int DefBonus = 0;
    public int HPBonus = 0;
    public int ManaBonus = 0;
    public int MagBonus = 0;
    public int SpdBonus = 0;
}
