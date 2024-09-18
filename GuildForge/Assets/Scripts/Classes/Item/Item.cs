using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    public enum ItemType { Weapon, Armor, Accessory }

    public bool InStore;
    public int ID;
    public Texture Icon;
    public string Name;
    [ReadOnly]  public ItemType _ItemType;
    public int Cost;

    public int AtkBonus;
    public int DefBonus;
    public int HPBonus;
    public int ManaBonus;
    public int MagBonus;
    public int SpdBonus;
}
