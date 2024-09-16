using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class MissionListEntryController
{
    Label m_NameLabel;
    Label m_MissionTypelLabel;


    public void SetVisualElement(VisualElement visualElement)
    {
        m_NameLabel = visualElement.Q<Label>("MissionName");
        m_MissionTypelLabel = visualElement.Q<Label>("MissionType");


    }

    public void SetMissionData(Mission mission)
    {
        m_NameLabel.text = mission.Name;
        m_MissionTypelLabel.text = mission.Type.ToString();

    }
}