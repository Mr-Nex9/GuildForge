using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AdventurerListController
{
    VisualTreeAsset m_listEntryTemplate;

    ListView m_rosterList;
    Label m_adventurerClassLabel;
    Label m_adventurerNameLabel;
    Label m_adventurerLevelLabel;
    VisualElement m_adventurerIcon;


    List<Adventurer> m_Roster;

    public void InitializeRosterList(VisualElement root, VisualTreeAsset listElementTemplate)
    {
        EnumerateAllAdventurers();

        m_listEntryTemplate = listElementTemplate;

        m_rosterList = root.Q<ListView>("Roster-List");

        m_adventurerClassLabel = root.Q<Label>("AdventurerClass");
        m_adventurerNameLabel = root.Q<Label>("AdventurerName");
        m_adventurerLevelLabel = root.Q<Label>("AdventurerLV");
        m_adventurerIcon = root.Q<VisualElement>("AdventurerIcon");


        FillAdventurerList();

        m_rosterList.selectionChanged += OnAdventurerSelected;

    }



    private void EnumerateAllAdventurers()
    {
        m_Roster = new List<Adventurer>();
        m_Roster.AddRange(Resources.LoadAll<Adventurer>("Adventurers"));
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
    private void OnAdventurerSelected(IEnumerable<object> enumerable)
    {
        Debug.Log(m_adventurerNameLabel.text);
    }

}
