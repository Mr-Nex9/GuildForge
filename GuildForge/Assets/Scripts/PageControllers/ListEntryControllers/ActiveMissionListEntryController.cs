using UnityEngine;
using UnityEngine.UIElements;

public class ActiveMissionListEntryController
{
    Label m_NameLabel;
    ProgressBar m_ProgressBar;
    Button m_CompleteButton;

    public void SetVisualElement(VisualElement visualElement)
    {
        m_NameLabel = visualElement.Q<Label>("MissionName");
    
        m_ProgressBar = visualElement.Q<ProgressBar>("MissionProgress");

        m_CompleteButton = visualElement.Q<Button>("CompleteButton");
    }

    public void SetMissionData(Mission mission)
    {
        m_NameLabel.text = mission.Name;
        m_ProgressBar.title = mission.TimeToCompleteInSeconds.ToString();
        m_ProgressBar.value = mission.CompletionPercent;

        if(mission.CompletionPercent == 100)
        {
            m_CompleteButton.SetEnabled(true);
        }
        else
        {
            m_CompleteButton.SetEnabled(false);
        }
    }
}