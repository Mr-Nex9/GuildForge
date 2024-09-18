using Assets.PixelFantasy.PixelHeroes.Common.Scripts.CharacterScripts;
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
    int curIndex;

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
        curIndex = m_rosterList.selectedIndex;

        GameObject UIMaster = GameObject.FindGameObjectWithTag("UI Manager");
        UIMaster.GetComponent<UIManager>().ShowAdventurerStats();
    }
    #region
    int StatsCount;
    VisualElement StatsPopUp;

    Label TopNameLabel;
    Label BotNameLabel;
    Label ClassLabel;
    Label LvLabel;
    Label RankLabel;
    Label HpLabel;
    Label ManaLabel;
    Label AttackLabel;
    Label DefenseLabel;
    Label MagicLabel;
    Label SpeedLabel;
    Label StatusLabel;

    Button WeaponButton;
    Button ArmorButton;
    Button Acessory1Button;
    Button Acessory2Button;
    Button LevelUpButton;
    Button ExitButton;
    Button BackButton;
    Button ForwardButton;

    public void InitializeStatsPage(VisualElement root)
    {
        StatsPopUp = root;
        if (StatsCount == 0)
        {
            TopNameLabel = root.Q<Label>("BigName");
            BotNameLabel = root.Q<Label>("Name");
            ClassLabel = root.Q<Label>("Class");
            LvLabel = root.Q<Label>("Level");
            RankLabel = root.Q<Label>("RankLabel");
            HpLabel = root.Q<Label>("HPLabel");
            ManaLabel = root.Q<Label>("ManaLabel");
            AttackLabel = root.Q<Label>("AttackLabel");
            DefenseLabel = root.Q<Label>("DefenseLabel");
            MagicLabel = root.Q<Label>("MagicLabel");
            SpeedLabel = root.Q<Label>("SpeedLabel");
            StatusLabel = root.Q<Label>("CurrentStatus");

            WeaponButton = root.Q<Button>("WeaponButton");
            ArmorButton = root.Q<Button>("ArmorButton");
            Acessory1Button = root.Q<Button>("Acessory1Button");
            Acessory2Button = root.Q<Button>("Acessory2Button");
            LevelUpButton = root.Q<Button>("LevelUpButton");
            ExitButton = root.Q<Button>("ExitButton");
            BackButton = root.Q<Button>("BackButton");
            ForwardButton = root.Q<Button>("ForwardButton");

            WeaponButton.RegisterCallback<ClickEvent>(e => EquipItem("Weapon"));  
            ArmorButton.RegisterCallback<ClickEvent>(e => EquipItem("Weapon"));
            Acessory1Button.RegisterCallback<ClickEvent>(e => EquipItem("Weapon"));
            Acessory2Button.RegisterCallback<ClickEvent>(e => EquipItem("Weapon"));
            LevelUpButton.clicked += LevelUpPressed;
            ExitButton.clicked += CloseStatsPage;
            BackButton.clicked += PreviousAdventurer;
            ForwardButton.clicked += NextAdventurer;
        }
        FillAdventurerInfo();

    }
    
    void FillAdventurerInfo()
    {
        TopNameLabel.text = CurSelection.Name;
        BotNameLabel.text = CurSelection.Name;
        ClassLabel.text = CurSelection.Class.ToString();
        LvLabel.text = CurSelection.Level.ToString();
        RankLabel.text = CurSelection.Rank.ToString();
        HpLabel.text = CurSelection.HP.ToString();
        ManaLabel.text = CurSelection.Mana.ToString();
        AttackLabel.text = CurSelection.Attack.ToString();
        DefenseLabel.text = CurSelection.Defense.ToString();
        MagicLabel.text = CurSelection.Magic.ToString();
        SpeedLabel.text = CurSelection.Speed.ToString();

        if (CurSelection.OnMission == false)
        {
            StatusLabel.text = "Home Doing Odd Jobs";
        }
        else
        {
            StatusLabel.text = $"Out on {CurSelection.CurrentMission} Mission";
        }

        GameObject Character = GameObject.FindGameObjectWithTag("Character");
        CharacterBuilder characterBuilder = Character.GetComponent<CharacterBuilder>();

        characterBuilder.Head = CurSelection.Head;
        characterBuilder.Ears = CurSelection.Ears;
        characterBuilder.Eyes = CurSelection.Eyes;
        characterBuilder.Hair = CurSelection.Hair;
        characterBuilder.Armor = CurSelection.Armor;
        characterBuilder.Helmet = CurSelection.Helmet;
        characterBuilder.Weapon = CurSelection.Weapon;
        characterBuilder.Shield = CurSelection.Shield;
        characterBuilder.Cape = CurSelection.Cape;
        characterBuilder.Back = CurSelection.Back;
        characterBuilder.Mask = CurSelection.Mask;

        characterBuilder.Rebuild();


    }
    void EquipItem(String itemType)
    {

    }
    void PreviousAdventurer()
    {
        curIndex -= 1;
        if (curIndex < 0)
        {
            curIndex = m_Roster.Count - 1;
        }
        CurSelection = m_Roster[curIndex];

        FillAdventurerInfo();
    }

    void NextAdventurer()
    {
        curIndex += 1;
        if (curIndex > m_Roster.Count - 1)
        {
            curIndex = 0;
        }
        CurSelection = m_Roster[curIndex];

        FillAdventurerInfo();
    }
    void LevelUpPressed()
    {

    }

    void CloseStatsPage()
    {
        StatsPopUp.style.display = DisplayStyle.None;
    }
    #endregion
}
