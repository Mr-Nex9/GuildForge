using UnityEngine;


[CreateAssetMenu(fileName = "Accessory", menuName = "Scriptable Objects/Item/Accessory")]
public class Accessory: Item
{

    private void OnEnable()
    {
        itemtype = ItemType.Accessory;
    }

}
