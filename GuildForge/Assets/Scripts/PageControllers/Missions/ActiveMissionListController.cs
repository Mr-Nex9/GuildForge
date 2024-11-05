using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ActiveMissionListController
{
    VisualElement activeMissionDisplay;
    List<Mission> activeMissions;
    Mission curMission;
    UnityEngine.UIElements.StyleColor Default;

    public void InitializeMissionList(VisualElement root, List<Mission> missions)
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
                        completeButton.RegisterCallback<ClickEvent>(e => MissionComplete(i-1, completeButton));
                        missionTimeRemaining.text = "Completed!";
                    }
                    else
                    {
                        completeButton.style.display = DisplayStyle.None;
                        completeButton.UnregisterCallback<ClickEvent>(e => MissionComplete(i - 1, completeButton));
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
        else
        {
            for (int i = 1; i <= 5; i++)
            {
                VisualElement missionBox = activeMissionDisplay.Q<VisualElement>("Mission" + i);
                Label emptySlot = missionBox.Q<Label>("EmptySlot");
                emptySlot.style.display = DisplayStyle.Flex;

                missionBox = missionBox.Q<VisualElement>("MissionActive");
                missionBox.style.display = DisplayStyle.None;
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

    async void MissionComplete(int index, Button btn)
    {
        GameObject soundMaster = GameObject.FindGameObjectWithTag("SoundManager");
        SoundManager soundManager = soundMaster.GetComponent<SoundManager>();
        soundManager.ButtonSound();

        Default = btn.style.backgroundColor;
        btn.style.backgroundColor = Color.blue;
        await Task.Delay(TimeSpan.FromSeconds(.05));
        btn.style.backgroundColor = Color.green;

        Debug.Log(index);
        curMission = activeMissions[index];

        GameObject UIMaster = GameObject.FindGameObjectWithTag("UI Manager");
        UIMaster.GetComponent<UIManager>().ShowMissionCompletePopUp();
    }

    public void MissionUIUpdate()
    {
        if (activeMissions != null)
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
                    completeButton.RegisterCallback<ClickEvent>(e => MissionComplete(i - 2, completeButton));
                    missionTimeRemaining.text = "Completed!";
                }
                else
                {
                    completeButton.style.display = DisplayStyle.None;
                    completeButton.UnregisterCallback<ClickEvent>(e => MissionComplete(i - 1, completeButton));
                    missionTimeRemaining.text = ConvertTimeToString((int)(activeMissions[i - 1].EndTime - DateTime.Now).TotalSeconds);
                }
            }
        }
    }

    void ResetMissionSlots()
    {
        for (int i = 1; i <= 5; i++)
        {
            VisualElement missionBox = activeMissionDisplay.Q<VisualElement>("Mission" + i);
            Label emptySlot = missionBox.Q<Label>("EmptySlot");
            emptySlot.style.display = DisplayStyle.Flex;

            missionBox = missionBox.Q<VisualElement>("MissionActive");
            missionBox.style.display = DisplayStyle.None;
        }
    }

    #region Complete Mission Popup
    VisualElement popUp;
    Button exitButton;
    Button completeButton;
    
    Label missionRank;
    Label missionType;
    Label missionName;

    Label goldLabel;
    Label reputationLabel;
    Label expLabel;

    bool isSetup;
    public void InitializeMissionPopUp(VisualElement root)
    {
        popUp = root;

        if (isSetup == false)
        {
            isSetup = true;

            exitButton = root.Q<Button>("ExitButton"); 
            completeButton = root.Q<Button>("CompleteButton");

            missionRank = root.Q<Label>("MissionRank"); 
            missionType = root.Q<Label>("MissionType"); 
            missionName = root.Q<Label>("MissionName"); 

            goldLabel = root.Q<Label>("GoldLabel"); 
            reputationLabel = root.Q<Label>("ReputationLabel"); 
            expLabel = root.Q<Label>("EXPLabel"); 

            exitButton.clicked += ExitPopUp;
            completeButton.clicked += CompleteMission;
        }

        missionRank.text = curMission.Rank.ToString();
        missionType.text = curMission.Type.ToString();
        missionName.text = curMission.Name;

        goldLabel.text = curMission.GoldValue.ToString();
        reputationLabel.text = curMission.ReputationValue.ToString();
        expLabel.text = curMission.EXPValue.ToString();

    }
    void ExitPopUp()
    {
        GameObject soundMaster = GameObject.FindGameObjectWithTag("SoundManager");
        SoundManager soundManager = soundMaster.GetComponent<SoundManager>();
        soundManager.ButtonSound();
        popUp.style.display = DisplayStyle.None;
    }
    void CompleteMission()
    {
        GameObject soundMaster = GameObject.FindGameObjectWithTag("SoundManager");
        SoundManager soundManager = soundMaster.GetComponent<SoundManager>();
        soundManager.MoneySound();

        FillMissionUI();

        curMission.CompleteMission();

        ResetMissionSlots();
        ExitPopUp();
    }
    #endregion
}

