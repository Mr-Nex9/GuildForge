using UnityEngine;
using UnityEngine.UIElements;

public class AdventurerListEntryController
{
    Label m_NameLabel;
    Label m_adventurerLevelLabel;
    Label m_adventurerClassLabel;
    VisualElement m_Icon;
    public void SetVisualElement(VisualElement visualElement)
    {
        m_NameLabel = visualElement.Q<Label>("AdventurerName");
        m_adventurerLevelLabel = visualElement.Q<Label>("AdventurerLV");
        m_adventurerClassLabel = visualElement.Q<Label>("AdventurerClass");
    }

    public void SetAdventurerData(Adventurer adventurer)
    {
        m_NameLabel.text = adventurer.Name;
        m_adventurerLevelLabel.text = adventurer.Level.ToString();
        m_adventurerClassLabel.text = adventurer.Class.ToString();
    }
}
