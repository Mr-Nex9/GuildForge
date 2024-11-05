using UnityEngine;
using UnityEngine.UIElements;

public class EquipItemListEntry
{
    Label m_NameLabel;
    VisualElement itemBox;
    UnityEngine.UIElements.StyleColor Default;
    
    public void SetVisualElement(VisualElement visualElement)
    {
        itemBox = visualElement;
        m_NameLabel = visualElement.Q<Label>("ItemName");
        Default = itemBox.style.backgroundColor;
    }

    public void SetItemData(Item item)
    {
        m_NameLabel.text = item.Name;
    }
    public void setItemColor(bool isSelected)
    {
        if (isSelected)
        {
            itemBox.style.backgroundColor = Color.yellow;
        }
        else
        {
            itemBox.style.backgroundColor = Default;
        }
    }
}
