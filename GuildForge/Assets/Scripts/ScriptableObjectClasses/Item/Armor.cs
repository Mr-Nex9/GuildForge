using UnityEngine;

[CreateAssetMenu(fileName = "Armor", menuName = "Scriptable Objects/Item/Armor")]
public class Armor : Item
{
    private void OnEnable()
    {
        itemtype = ItemType.Armor;
    }
}
