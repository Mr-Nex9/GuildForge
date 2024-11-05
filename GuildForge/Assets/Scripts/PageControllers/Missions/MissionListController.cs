using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class MissionListController
{
    VisualTreeAsset m_listEntryTemplate;

    ListView m_missionList;

    [SerializeField]List<Mission> m_mission;

    bool isFirstLoad = true;


    public void InitializeMissionList(VisualElement root, VisualTreeAsset listElementTemplate)
    {
        if (isFirstLoad)
        {
            m_listEntryTemplate = listElementTemplate;

            m_missionList = root.Q<ListView>("NewMissions-List");

            isFirstLoad = false;
        }

        GameObject GameMaster = GameObject.FindGameObjectWithTag("GameController");
        m_mission = new List<Mission>(GameMaster.GetComponent<GameManager>().FindNewMissions());

        FillMissionList();
        m_missionList.Rebuild();

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
            detailsButton.RegisterCallback<ClickEvent>(e => OnDetailsButtonClicked(newListEntry, detailsButton));
            return newListEntry;
        };

        m_missionList.bindItem = (item, index) =>
        {
            (item.userData as MissionListEntryController)?.SetMissionData(m_mission[index]);
            item.userData = index;
        };
        m_missionList.itemsSource = m_mission;

    }

    void OnDetailsButtonClicked(VisualElement element, Button btn)
    {
        GameObject soundMaster = GameObject.FindGameObjectWithTag("SoundManager");
        SoundManager soundManager = soundMaster.GetComponent<SoundManager>();
        soundManager.ButtonSound();

        int id = m_mission[(int)element.userData].ID;
        GameObject uiMaster = GameObject.FindGameObjectWithTag("UI Manager");
        uiMaster.GetComponent<UIManager>().MissionDetails(id);
    }
}
