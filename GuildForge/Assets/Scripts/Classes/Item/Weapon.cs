using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Scriptable Objects/Item/Weapon")]
public class Weapon:Item
{
    private void OnEnable()
    {
        _ItemType = ItemType.Weapon;
    }
}
