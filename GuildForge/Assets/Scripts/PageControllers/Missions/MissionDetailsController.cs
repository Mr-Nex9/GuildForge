using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class MissionDetailsController
{
    VisualElement thisPage;
    Mission CurMission;
    List<Adventurer> m_AssignedAdventurers;
    int CurIndex;
    int DetailsPopCount = 0;

    VisualElement m_BaseDetails;
    Label m_MissionName;
    Label m_MissionType;
    Label m_MissionRank;

    VisualElement m_ROIDetails;
    Label m_GoldValue;
    Label m_ReputationValue;
    Label m_ExpValue;

    VisualElement m_AdventurerIcons;
    Button m_Slot1;
    Button m_Slot2;
    Button m_Slot3;
    VisualElement m_AdventurerNames;
    Label m_Name1;
    Label m_Name2;
    Label m_Name3;

    VisualElement m_TimeBox;
    Label m_EstTimeToComplete;

    Button m_AcceptButton;
    Button m_ExitButton;
    UnityEngine.UIElements.StyleColor m_Default;

    public void InitializePage(VisualElement root, int missionID)
    {
        thisPage = root;
        m_AssignedAdventurers = new List<Adventurer>();
        CurMission = FindSelectedMission(missionID);

        // Assign buttons and Labels
        if (DetailsPopCount == 0) 
        {
            m_BaseDetails = root.Q<VisualElement>("BaseDetails");
            m_MissionName = m_BaseDetails.Q<Label>("MissionName");
            m_MissionType = m_BaseDetails.Q<Label>("MissionType");
            m_MissionRank = m_BaseDetails.Q<Label>("MissionRank");

            m_ROIDetails = root.Q<VisualElement>("ROIDetails");
            m_GoldValue = m_ROIDetails.Q<Label>("GoldLabel");
            m_ReputationValue = m_ROIDetails.Q<Label>("ReputationLabel");
            m_ExpValue = m_ROIDetails.Q<Label>("EXPLabel");

            var temp = root.Q<VisualElement>("AdventurerBox");
            m_AdventurerIcons = temp.Q<VisualElement>("IconBox");
            m_AdventurerNames = temp.Q<VisualElement>("NameBox");

            m_Slot1 = m_AdventurerIcons.Q<Button>("Slot1");
            m_Slot2 = m_AdventurerIcons.Q<Button>("Slot2");
            m_Slot3 = m_AdventurerIcons.Q<Button>("Slot3");

            m_Name1 = m_AdventurerNames.Q<Label>("Name1");
            m_Name2 = m_AdventurerNames.Q<Label>("Name2");
            m_Name3 = m_AdventurerNames.Q<Label>("Name3");

            m_TimeBox = root.Q<VisualElement>("TimeBox");
            m_EstTimeToComplete = m_TimeBox.Q<Label>("CompletionTime");


            m_AcceptButton = root.Q<Button>("StartButton");
            m_ExitButton = root.Q<Button>("ExitButton");

            m_AcceptButton.clicked += OnAcceptButtonClicked;
            m_ExitButton.clicked += ExitPopup;
            m_Slot1.clicked += OnSlot1Selected;
            m_Slot2.clicked += OnSlot2Selected;
            m_Slot3.clicked += OnSlot3Selected;
            DetailsPopCount = 1;
        }

        m_MissionName.text = CurMission.Name;
        m_MissionType.text = CurMission.Type.ToString();
        m_MissionRank.text = CurMission.Rank.ToString();
        m_GoldValue.text = CurMission.GoldValue.ToString();
        m_ReputationValue.text = CurMission.ReputationValue.ToString();
        m_ExpValue.text = CurMission.EXPValue.ToString();
        m_EstTimeToComplete.text = ConvertTimeToString(CurMission.CalculateTimeToComplete(m_AssignedAdventurers));

    }

    private Mission FindSelectedMission(int missionID)
    {
        List<Mission> temp = new List<Mission>();
        temp.AddRange(Resources.LoadAll<Mission>("Missions"));

        foreach (Mission mission in temp)
        {
            if(mission.ID == missionID)
            {
                return mission;
            }
        }
        return null;
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
            else if(seconds > 0)
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

    public void NewAdventurerAssigned(Adventurer assigned)
    {
        if (assigned == null)
        {
            switch (CurIndex)
            {
                case 0:
                    {
                        m_Slot1.style.backgroundImage = null;
                        m_Name1.text = "select";
                    }
                    break;
                case 1:
                    {
                        m_Slot2.style.backgroundImage = null;
                        m_Name2.text = "select";

                    }
                    break;
                case 2:
                    {
                        m_Slot3.style.backgroundImage = null;
                        m_Name3.text = "select";

                    }
                    break;
            }
        }
        else
        {
            m_AssignedAdventurers.Insert(CurIndex, assigned);
            switch (CurIndex)
            {
                case 0:
                    {
                        m_Slot1.style.backgroundImage = (StyleBackground)assigned.Icon;
                        m_Name1.text = assigned.Name;

                    }
                    break;
                case 1:
                    {
                        m_Slot2.style.backgroundImage = (StyleBackground)assigned.Icon;
                        m_Name2.text = assigned.Name;
                    }
                    break;
                case 2:
                    {
                        m_Slot3.style.backgroundImage = (StyleBackground)assigned.Icon;
                        m_Name3.text = assigned.Name;
                    }
                    break;
            }
        }
        m_EstTimeToComplete.text = ConvertTimeToString(CurMission.CalculateTimeToComplete(m_AssignedAdventurers));
    }
    void DisplayRoster()
    {
        GameObject UIMaster = GameObject.FindGameObjectWithTag("UI Manager");
        UIMaster.GetComponent<UIManager>().SelectAdventurer();
    }
    #region Button Methods
    void OnSlot1Selected()
    {
        if(m_AssignedAdventurers.Count > 0)
        {
            m_AssignedAdventurers.RemoveAt(0);
        }
        CurIndex = 0;
        DisplayRoster();
    }
    void OnSlot2Selected()
    {
        if (m_AssignedAdventurers.Count > 1)
        {
            m_AssignedAdventurers.RemoveAt(1);
            CurIndex = 1;
        }
        else if(m_AssignedAdventurers.Count > 0)
        {
            CurIndex = 1;
        }
        else
        {
            CurIndex = 0;
        }
        DisplayRoster();
    }
    void OnSlot3Selected()
    {
        if (m_AssignedAdventurers.Count > 2)
        {
            m_AssignedAdventurers.RemoveAt(2);
            CurIndex = 2;
        }
        if (m_AssignedAdventurers.Count > 1)
        {
            CurIndex = 2;
        }
        else if (m_AssignedAdventurers.Count > 0)
        {
            CurIndex = 1;
        }
        else
        {
            CurIndex = 0;
        }
        DisplayRoster();
    }
    async void OnAcceptButtonClicked()
    {
        if (m_AssignedAdventurers.Count > 0)
        {
            m_Default = m_AcceptButton.style.backgroundColor;
            m_AcceptButton.style.backgroundColor = Color.blue;
            await Task.Delay(TimeSpan.FromSeconds(.05));
            m_AcceptButton.style.backgroundColor = m_Default;

            CurMission.MissionStart(m_AssignedAdventurers);

            GameObject GameMaster = GameObject.FindGameObjectWithTag("GameController");
            GameMaster.GetComponent<GameManager>().AddToActiveMissionList(CurMission);

            ExitPopup();
        }
    }

    void ExitPopup()
    {
        m_Slot1.style.backgroundImage = null;
        m_Slot2.style.backgroundImage = null;
        m_Slot3.style.backgroundImage = null;
        thisPage.style.display = DisplayStyle.None;

        GameObject UIMaster = GameObject.FindGameObjectWithTag("UI Manager");
        UIMaster.GetComponent<UIManager>().HomeBtn_clicked();
    }
    #endregion

    #region Roster PopUp-------------

    VisualElement RosterPopUp;
    VisualTreeAsset m_listEntryTemplate;

    ListView m_rosterList;

    List<Adventurer> m_Roster;
    List<Adventurer> m_RosterTemp;

    Button RosterExit;
    Button Confirm;
    Button Clear;
    Adventurer CurSelection;
    int RosterPopCount = 0;
    StyleColor DefaultColor;
    public void InitializeRosterList(VisualElement root, VisualTreeAsset listElementTemplate, List<Adventurer> roster)
    {
        CurSelection = null;
        RosterPopUp = root;
        m_RosterTemp = new List<Adventurer>(roster);
        CheckForCurrentlyAdded();
        if (RosterPopCount == 0)
        {
            m_listEntryTemplate = listElementTemplate;

            m_rosterList = root.Q<ListView>("Roster-List");
            RosterExit = root.Q<Button>("ExitButton");
            Confirm = root.Q<Button>("ConfirmButton");
            Clear = root.Q<Button>("ClearButton");

            m_rosterList.selectionChanged += OnAdventurerSelected;
            RosterExit.clicked += CloseRoster;
            Confirm.clicked += ConfirmSelection;
            Clear.clicked += RemoveSelection;
            RosterPopCount = 1;
        }


        FillAdventurerList();



    }
    private void FillAdventurerList()
    {
        // Set up a make item function for a list entry
        m_rosterList.makeItem = () =>
        {
            var newListEntry = m_listEntryTemplate.Instantiate();
            var newListEntryLogic = new AdventurerListEntryController();
            newListEntry.userData = newListEntryLogic;
            newListEntryLogic.SetVisualElement(newListEntry);
            return newListEntry;
        };

        m_rosterList.bindItem = (item, index) =>
        {
            (item.userData as AdventurerListEntryController)?.SetAdventurerData(m_Roster[index]);
        };

        m_rosterList.itemsSource = m_Roster;
    }

    //attempt at fix not working.....
    void CheckForCurrentlyAdded()
    {
        m_Roster = new List<Adventurer>();
        foreach (Adventurer hero in m_RosterTemp)
        {
            if (m_AssignedAdventurers.Contains(hero) || hero.OnMission == true)
            {
                continue;
            }
            else 
            {
                m_Roster.Add(hero);
            }

        }

        if (m_Roster.Count < 2)
        {
            CurSelection = m_Roster[0];
        }
    }

    VisualElement curElement;
    private void OnAdventurerSelected(IEnumerable<object> enumerable)
    {
        CurSelection = m_Roster[m_rosterList.selectedIndex];
        
        if (curElement != null)
        {

            curElement.style.unityBackgroundImageTintColor = DefaultColor;
            curElement = m_rosterList.GetRootElementForIndex(m_rosterList.selectedIndex);
            curElement.style.unityBackgroundImageTintColor = Color.white;
        }
        else
        {
            curElement = m_rosterList.GetRootElementForIndex(m_rosterList.selectedIndex);
            DefaultColor = curElement.style.unityBackgroundImageTintColor;
            curElement.style.unityBackgroundImageTintColor = Color.white;
        }
    }
    void CloseRoster()
    {
        RosterPopUp.style.display = DisplayStyle.None;
    }

    void ConfirmSelection()
    {
        if (CurSelection != null)
        {
            CloseRoster();
            NewAdventurerAssigned(CurSelection);
        }

    }

    void RemoveSelection()
    {
        CloseRoster();
        NewAdventurerAssigned(null);
    }
    #endregion


}
