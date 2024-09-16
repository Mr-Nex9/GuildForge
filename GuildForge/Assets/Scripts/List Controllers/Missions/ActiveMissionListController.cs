using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ActiveMissionListController
{
    VisualTreeAsset m_listEntryTemplate;

    ListView m_ActivemissionList;


    List<Mission> m_missions;
    List<Mission> m_Activemissions;

    public void InitializeMissionList(VisualElement root, VisualTreeAsset listElementTemplate)
    {
        EnumerateAllMissions();

        m_listEntryTemplate = listElementTemplate;

        m_ActivemissionList = root.Q<ListView>("ActiveMissions-List");

        FillMissionList();

    }



    private void EnumerateAllMissions()
    {
        m_missions = new List<Mission>();
        m_missions.AddRange(Resources.LoadAll<Mission>("Missions"));


        m_Activemissions = new List<Mission>();
        foreach (Mission mission in m_missions)
        {
            if (mission.Active == true)
            {
                m_Activemissions.Add(mission);
            }
        }
    }
    private void FillMissionList()
    {
        // Set up a make item function for a list entry
        m_ActivemissionList.makeItem = () =>
        {
            var newListEntry = m_listEntryTemplate.Instantiate();
            var newListEntryLogic = new ActiveMissionListEntryController();
            newListEntry.userData = newListEntryLogic;
            newListEntryLogic.SetVisualElement(newListEntry);
            return newListEntry;
        };

        m_ActivemissionList.bindItem = (item, index) =>
        {
            (item.userData as ActiveMissionListEntryController)?.SetMissionData(m_Activemissions[index]);
        };

        m_ActivemissionList.itemsSource = m_Activemissions;
    }

}
