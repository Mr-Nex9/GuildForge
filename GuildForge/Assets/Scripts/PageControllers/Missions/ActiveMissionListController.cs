using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ActiveMissionListController
{
    VisualElement activeMissionDisplay;
    List<Mission> activeMissions;

    public void InitializeMissionList(VisualElement root, VisualTreeAsset listElementTemplate, List<Mission> missions)
    {
        activeMissions = new List<Mission>(missions);
        activeMissionDisplay = root;
        FillMissionUI();
    }



    private void FillMissionUI()
    {
        if (activeMissions.Count > 0)
        {
            for (int i = 1; i <= 5; i++)
            {
                if(i <= activeMissions.Count) // if there are active missions 
                {
                    VisualElement missionBox = activeMissionDisplay.Q<VisualElement>("Mission" + i);
                    Label emptySlot = missionBox.Q<Label>("EmptySlot");
                    emptySlot.style.display = DisplayStyle.None;

                    missionBox = missionBox.Q<VisualElement>("MissionActive");
                    missionBox.style.display = DisplayStyle.Flex;
                    Label missionName = missionBox.Q<Label>("Name");
                    Label missionTimeRemaining = missionBox.Q<Label>("TimeRemaining"); ;
                    VisualElement progressBar = missionBox.Q<VisualElement>("ProgressBar");
                    Button completeButton = missionBox.Q<Button>("CompleteButton");

                    missionName.text = activeMissions[i - 1].Name;
                    progressBar.style.width = Length.Percent(activeMissions[i - 1].CompletionPercent);

                    if (activeMissions[i - 1].EndTime <= DateTime.Now)
                    {
                        completeButton.style.display = DisplayStyle.Flex;
                        completeButton.RegisterCallback<ClickEvent>(e => MissionComplete(i - 1));
                        missionTimeRemaining.text = "Completed!";
                    }
                    else
                    {
                        missionTimeRemaining.text = ConvertTimeToString((int)(activeMissions[i - 1].EndTime - DateTime.Now).TotalSeconds);
                    }
                }
                else // reset the button
                {
                    VisualElement missionBox = activeMissionDisplay.Q<VisualElement>("Mission" + i);
                    Label emptySlot = missionBox.Q<Label>("EmptySlot");
                    emptySlot.style.display = DisplayStyle.Flex;

                    missionBox = missionBox.Q<VisualElement>("MissionActive");
                    missionBox.style.display = DisplayStyle.None;
                }

            }
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

    void MissionComplete(int index)
    {
        //call ui manager to load complete mission popup
        //UI manager can call a function here that will load the info into the popup
        //the button there that says congrats or complete will then call the missions complete Mission method.
        //reinitialize the page
    }

    public void MissionUIUpdate()
    {
        for (int i = 1; i <= activeMissions.Count; i++)
        {
            VisualElement missionBox = activeMissionDisplay.Q<VisualElement>("Mission" + i);

            Label missionTimeRemaining = missionBox.Q<Label>("TimeRemaining"); ;
            VisualElement progressBar = missionBox.Q<VisualElement>("ProgressBar");
            Button completeButton = missionBox.Q<Button>("CompleteButton");


            progressBar.style.width = Length.Percent(activeMissions[i - 1].CompletionPercent);

            if (activeMissions[i - 1].EndTime <= DateTime.Now)
            {
                completeButton.style.display = DisplayStyle.Flex;
                completeButton.RegisterCallback<ClickEvent>(e => MissionComplete(i - 1));
                missionTimeRemaining.text = "Completed!";
            }
            else
            {
                missionTimeRemaining.text = ConvertTimeToString((int)(activeMissions[i - 1].EndTime - DateTime.Now).TotalSeconds);
            }
        }
    }
}

