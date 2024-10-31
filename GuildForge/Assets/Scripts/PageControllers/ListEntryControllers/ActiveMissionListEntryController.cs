using System.Reflection;

using UnityEngine;
using UnityEngine.UIElements;
using System;
using Unity.VisualScripting;

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
        m_ProgressBar.title = ConvertTimeToString(mission.TimeToCompleteInSeconds);
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
    private string ConvertTimeToString(int timeInSeconds)
    {

        int seconds = timeInSeconds;
        int hours = 0;
        int minutes = 0;
        string CompletionTime;

        while (seconds > 60)
        {
            minutes += 1;
            seconds -= 60;

            if (minutes == 60)
            {
                hours += 1;
                minutes -= 60;
            }
        }
        if (hours > 0)
        {
            if (minutes > 0)
            {
                if (seconds > 0)
                {
                    CompletionTime = $"{hours} hours, {minutes} minutes, {seconds} seconds";
                }
                else
                {
                    CompletionTime = $"{hours} hours, {minutes} minutes";
                }
            }
            else if (seconds > 0)
            {
                CompletionTime = $"{hours} hours, {seconds} seconds";
            }
            else
            {
                CompletionTime = $"{hours} hours";
            }
        }
        else if (minutes > 0)
        {
            if (seconds > 0)
            {
                CompletionTime = $"{minutes} minutes, {seconds} seconds";
            }
            else
            {
                CompletionTime = $"{minutes} minutes";
            }
        }
        else
        {
            CompletionTime = $"{seconds} seconds";
        }

        return CompletionTime;
    }
}