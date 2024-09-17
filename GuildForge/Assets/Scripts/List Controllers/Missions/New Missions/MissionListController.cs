using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class MissionListController
{
    VisualTreeAsset m_listEntryTemplate;

    ListView m_missionList;

    List<Mission> m_mission;

    int m_count;


    public void InitializeMissionList(VisualElement root, VisualTreeAsset listElementTemplate)
    {
        EnumerateAllMissions();

        if (m_count == 0)
        {
            m_listEntryTemplate = listElementTemplate;

            

            m_count = 1;
        }
        m_missionList = root.Q<ListView>("NewMissions-List");
        FillMissionList();


    }
    private void EnumerateAllMissions()
    {
        m_mission = new List<Mission>();
        List<Mission> temp = new List<Mission>();
        temp.AddRange(Resources.LoadAll<Mission>("Missions"));

        foreach (Mission mission in temp)
        {
            if (mission.Active == false && mission.Offered == true)
            {
                m_mission.Add(mission);
            }
        }
    }
    private void FillMissionList()
    {

        // Set up a make item function for a list entry
        m_missionList.makeItem = () =>
        {
            var newListEntry = m_listEntryTemplate.Instantiate();
            var newListEntryLogic = new MissionListEntryController();
            newListEntry.userData = newListEntryLogic;
            newListEntryLogic.SetVisualElement(newListEntry);
            
            Button detailsButton = newListEntry.Q<Button>("DetailsButton");
            detailsButton.RegisterCallback<ClickEvent>(e => OnDetailsButtonClicked(newListEntry));
            return newListEntry;
        };

        m_missionList.bindItem = (item, index) =>
        {
            (item.userData as MissionListEntryController)?.SetMissionData(m_mission[index]);
            item.userData = index;
        };
        m_missionList.itemsSource = m_mission;

    }

    public void OnDetailsButtonClicked(VisualElement element)
    {
        
        int ID = m_mission[(int)element.userData].ID;
        GameObject UIMaster = GameObject.FindGameObjectWithTag("UI Manager");
        UIMaster.GetComponent<UIManager>().MissionDetails(ID);
    }
}
