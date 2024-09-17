using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AdventurerListController
{
    VisualTreeAsset m_listEntryTemplate;

    ListView m_rosterList;

    List<Adventurer> m_Roster;
    Adventurer CurSelection;

    public void InitializeRosterList(VisualElement root, VisualTreeAsset listElementTemplate, List<Adventurer> roster)
    {
        //EnumerateAllAdventurers();
        m_Roster = new List<Adventurer>(roster);

        m_listEntryTemplate = listElementTemplate;

        m_rosterList = root.Q<ListView>("Roster-List");

        FillAdventurerList();

        m_rosterList.selectionChanged += OnAdventurerSelected;

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
            item.userData = index;
        };

        m_rosterList.itemsSource = m_Roster;
    }
    private void OnAdventurerSelected(IEnumerable<object> index)
    {
        Debug.Log(m_rosterList.selectedIndex);
        CurSelection = m_Roster[m_rosterList.selectedIndex];

        GameObject UIMaster = GameObject.FindGameObjectWithTag("UI Manager");
        UIMaster.GetComponent<UIManager>().ShowAdventurerStats();
    }

}
