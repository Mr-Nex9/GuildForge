using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class MissionListController
{
    VisualTreeAsset m_listEntryTemplate;

    ListView m_missionList;

    List<Mission> m_mission;

    public void InitializeMissionList(VisualElement root, VisualTreeAsset listElementTemplate)
    {
        EnumerateAllMissions();

        m_listEntryTemplate = listElementTemplate;
       
        m_missionList = root.Q<ListView>("NewMissions-List");



        FillMissionList();



        m_missionList.selectionChanged += OpenMissionDetails;

    }

    private void M_missionList_selectionChanged()
    {
        throw new NotImplementedException();
    }

    private void EnumerateAllMissions()
    {
        m_mission = new List<Mission>();
        List<Mission> temp = new List<Mission>();
        temp.AddRange(Resources.LoadAll<Mission>("Missions"));

        foreach (Mission mission in temp)
        {
            if (mission.Active == false)
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
            return newListEntry;
        };

        m_missionList.bindItem = (item, index) =>
        {
            (item.userData as MissionListEntryController)?.SetMissionData(m_mission[index]);
        };

        m_missionList.itemsSource = m_mission;
    }

    void OpenMissionDetails(IEnumerable<object> obj)
    {
        Debug.Log(m_missionList.selectedIndex);
    }
}
