using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;

public class QuestsPageController
{
    VisualTreeAsset listEntryTemplate;
    public ListView listView;
    List<Quest> fullList = new List<Quest>();
    Button searchButton;
    Button resetButton;
    bool hasOpened = false;
    VisualElement thisPage;
    TextField searchBar;

    public void InitializeRosterList(VisualElement root, VisualTreeAsset listElementTemplate, List<Quest> list)
    {
        thisPage = root;
        fullList = new List<Quest>(list);

        if (hasOpened == false)
        {
            listEntryTemplate = listElementTemplate;

            listView = root.Q<ListView>("AchievementList");
            searchButton = root.Q<Button>("SearchButton");
            resetButton = root.Q<Button>("ResetButton");
            searchBar = root.Q<TextField>("SearchBox");

            searchButton.clicked += Search;
            resetButton.RegisterCallback<ClickEvent>(e => FillQuestList(fullList));

        }
        FillQuestList(fullList);
    }

    void Search()
    {
        string searchText = searchBar.value.ToLower();
        Debug.Log(searchText);
        
        List<Quest> filteredList = new List<Quest>();
        foreach (Quest quest in fullList)
        {
            if (quest.Name.ToLower().Contains(searchText))
            {
                filteredList.Add(quest);
                Debug.Log(quest.Name);
            }
        }

        FillQuestList(filteredList);
        FillQuestList(filteredList);
    }

    private void FillQuestList(List<Quest> list)
    {
        listView.makeItem = () =>
        {
            var newListEntry = listEntryTemplate.Instantiate();
            var newListEntryLogic = new QuestListEntryController();
            newListEntry.userData = newListEntryLogic;
            newListEntryLogic.SetVisualElement(newListEntry);
            return newListEntry;
        };

        listView.bindItem = (item, index) =>
        {
            Debug.Log(index);
            (item.userData as QuestListEntryController)?.SetQuestData(list[index]);
            item.userData = index;
        };

        listView.itemsSource = list;
    }
}
