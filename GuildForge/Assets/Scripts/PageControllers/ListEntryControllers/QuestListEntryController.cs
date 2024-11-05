using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

public class QuestListEntryController
{
    Label nameLabel;
    Label progressLabel;
    public void SetVisualElement(VisualElement element)
    {
        nameLabel = element.Q<Label>("Name");
        progressLabel = element.Q<Label>("Progress");
    }
    public void SetQuestData(Quest quest)
    {
        nameLabel.text = quest.Name;
        progressLabel.text = $"{quest.currentProgress} of {quest.requiredForCompletion}";
    }

    public void RemoveQuestData()
    {
        nameLabel.text = "";
        progressLabel.text = "";
    }
}
