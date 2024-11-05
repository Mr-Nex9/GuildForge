using Assets.PixelFantasy.PixelHeroes.Common.Scripts.CharacterScripts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

public class RosterController
{
    #region RosterPage
    VisualTreeAsset m_listEntryTemplate;
    public ListView m_rosterList;
    Button RecruitButton;
    bool hasOpened = false;
    VisualElement thisPage;

    List<Adventurer> m_Roster;
    Adventurer CurSelection;
    int curIndex;

    Button NameSort;
    Button ClassSort;
    Button LVSort;
    string Sorted;
    Button CurrentSort;
    UnityEngine.UIElements.StyleColor Default;
    UnityEngine.UIElements.StyleColor Clicked;

    public void InitializeRosterList(VisualElement root, VisualTreeAsset listElementTemplate, List<Adventurer> roster)
    {
        m_Roster = new List<Adventurer>(roster);
        thisPage = root;
        if (hasOpened == false)
        {
            m_listEntryTemplate = listElementTemplate;

            m_rosterList = root.Q<ListView>("Roster-List");
            RecruitButton = root.Q<Button>("RecruitButton");
            NameSort = root.Q<Button>("NameSort");
            ClassSort = root.Q<Button>("ClassSort");
            LVSort = root.Q<Button>("LvSort");

            Default = NameSort.style.backgroundColor;
            Clicked = NameSort.style.borderTopColor;
            CurrentSort = NameSort;


            m_rosterList.selectionChanged += OnAdventurerSelected;

            RecruitButton.clicked += OpenRecruitPage;
            
            NameSort.RegisterCallback<ClickEvent>(e => SortAdventurers("Name"));
            ClassSort.RegisterCallback<ClickEvent>(e => SortAdventurers("Class"));
            LVSort.RegisterCallback<ClickEvent>(e => SortAdventurers("LV"));

        }


        FillAdventurerList();




    }
    private void FillAdventurerList()
    {
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
        CurSelection = m_Roster[m_rosterList.selectedIndex];
        curIndex = m_rosterList.selectedIndex;
        Debug.Log($"Loading {CurSelection.Name}");

        GameObject soundMaster = GameObject.FindGameObjectWithTag("SoundManager");
        SoundManager soundManager = soundMaster.GetComponent<SoundManager>();
        soundManager.ButtonSound();

        GameObject UIMaster = GameObject.FindGameObjectWithTag("UI Manager");
        UIMaster.GetComponent<UIManager>().ShowAdventurerStats();
        m_rosterList.selectedIndex = -1;

    }
     void OpenRecruitPage()
    {
        GameObject soundMaster = GameObject.FindGameObjectWithTag("SoundManager");
        SoundManager soundManager = soundMaster.GetComponent<SoundManager>();
        soundManager.ButtonSound();

        GameObject UIMaster = GameObject.FindGameObjectWithTag("UI Manager");
        UIMaster.GetComponent<UIManager>().ShowRecruitPage();
    }
    void SortAdventurers(string sortBy)
    {
        GameObject soundMaster = GameObject.FindGameObjectWithTag("SoundManager");
        SoundManager soundManager = soundMaster.GetComponent<SoundManager>();
        soundManager.ButtonSound();

        Debug.Log("Sorting");
        switch (sortBy)
        {
            case "Name":
                {
                    CurrentSort.style.backgroundColor = Default;
                    ClassSort.style.backgroundColor = Clicked;
                    CurrentSort = ClassSort;

                    if(Sorted == "Name")
                    {
                        for (int i = 0; i < m_Roster.Count; i++)
                        {
                            int swap = i;
                            for (int j = i + 1; j < m_Roster.Count; j++)
                            {
                                if (m_Roster[i].Name.CompareTo(m_Roster[j].Name) < 0)
                                {
                                    swap = j;

                                }
                            }
                            if (i != swap)
                            {
                                Adventurer temp = m_Roster[i];
                                m_Roster[i] = m_Roster[swap];
                                m_Roster[swap] = temp;
                            }
                        }

                        Sorted = null;
                    }
                    else
                    {

                        for(int i = 0; i < m_Roster.Count; i++)
                        {
                            int swap = i;
                            for (int j = i+1; j < m_Roster.Count;j++)
                            {
                                if (m_Roster[i].Name.CompareTo(m_Roster[j].Name) > 0)
                                {
                                    swap = j;

                                }
                            }
                            if (i != swap)
                            {
                                Adventurer temp = m_Roster[i];
                                m_Roster[i] = m_Roster[swap];
                                m_Roster[swap] = temp;
                            }
                        }
                        Sorted = "Name";
                    }
                }break;
            case "Class":
                {
                    CurrentSort.style.backgroundColor = Default;
                    NameSort.style.backgroundColor = Clicked;
                    CurrentSort = NameSort;

                    if (Sorted == "Class")
                    {
                        for (int i = 0; i < m_Roster.Count; i++)
                        {
                            int swap = i;
                            for (int j = i + 1; j < m_Roster.Count; j++)
                            {
                                if (m_Roster[i].Class < m_Roster[j].Class)
                                {
                                    swap = j;

                                }
                            }
                            if (i != swap)
                            {
                                Adventurer temp = m_Roster[i];
                                m_Roster[i] = m_Roster[swap];
                                m_Roster[swap] = temp;
                            }
                        }

                        Sorted = null;
                    }
                    else
                    {

                        for (int i = 0; i < m_Roster.Count; i++)
                        {
                            int swap = i;
                            for (int j = i + 1; j < m_Roster.Count; j++)
                            {
                                if (m_Roster[i].Class > m_Roster[j].Class)
                                {
                                    swap = j;


                                }
                            }
                            if (i != swap)
                            {
                                Adventurer temp = m_Roster[i];
                                m_Roster[i] = m_Roster[swap];
                                m_Roster[swap] = temp;
                            }
                        }
                        Sorted = "Class";
                    }
                    
                }
                break;
            case "LV":
                {
                    CurrentSort.style.backgroundColor = Default;
                    LVSort.style.backgroundColor = Clicked;
                    CurrentSort = LVSort;

                    if (Sorted == "Class")
                    {
                        for (int i = 0; i < m_Roster.Count; i++)
                        {
                            int swap = i;
                            for (int j = i + 1; j < m_Roster.Count; j++)
                            {
                                if (m_Roster[i].Level < m_Roster[j].Level)
                                {
                                    swap = j;

                                }
                            }
                            if (i != swap)
                            {
                                Adventurer temp = m_Roster[i];
                                m_Roster[i] = m_Roster[swap];
                                m_Roster[swap] = temp;
                            }
                        }

                        Sorted = null;
                    }
                    else
                    {

                        for (int i = 0; i < m_Roster.Count; i++)
                        {
                            int swap = i;
                            for (int j = i + 1; j < m_Roster.Count; j++)
                            {
                                if (m_Roster[i].Level > m_Roster[j].Level)
                                {
                                    swap = j;


                                }
                            }
                            if (i != swap)
                            {
                                Adventurer temp = m_Roster[i];
                                m_Roster[i] = m_Roster[swap];
                                m_Roster[swap] = temp;
                            }
                        }
                        Sorted = "Class";
                    }
                }
                break;
        }
        Debug.Log("Sorted");
        m_rosterList.Rebuild();
    }
    #endregion

    #region Stats PopUp
    #region Variables
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

    Label hpBonus;
    Label manaBonus;
    Label atkBonus;
    Label defBonus;
    Label magBonus;
    Label spdBonus;

    Button WeaponButton;
    Button ArmorButton;
    Button Accessory1Button;
    Button Accessory2Button;
    Button LevelUpButton;
    Button ExitButton;
    Button BackButton;
    Button ForwardButton;

    string ItemType;
    Item curEquippedItem;
    int curSlot;
    #endregion
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

            hpBonus = root.Q<Label>("HPBonus");
            manaBonus = root.Q<Label>("ManaBonus");
            atkBonus = root.Q<Label>("AttackBonus");
            defBonus = root.Q<Label>("DefenseBonus");
            magBonus = root.Q<Label>("MagicBonus");
            spdBonus = root.Q<Label>("SpeedBonus");

            WeaponButton = root.Q<Button>("WeaponButton");
            ArmorButton = root.Q<Button>("ArmorButton");
            Accessory1Button = root.Q<Button>("Accessory1Button");
            Accessory2Button = root.Q<Button>("Accessory2Button");
            LevelUpButton = root.Q<Button>("LevelUpButton");
            ExitButton = root.Q<Button>("ExitButton");
            BackButton = root.Q<Button>("BackButton");
            ForwardButton = root.Q<Button>("ForwardButton");

            WeaponButton.RegisterCallback<ClickEvent>(e => OpenItemList("Weapon"));
            ArmorButton.RegisterCallback<ClickEvent>(e => OpenItemList("Armor"));
            Accessory1Button.RegisterCallback<ClickEvent>(e => OpenItemList("Accessory", 1));
            Accessory2Button.RegisterCallback<ClickEvent>(e => OpenItemList("Accessory" , 2));
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
        HpLabel.text = CurSelection.ActualHP.ToString();
        ManaLabel.text = CurSelection.ActualMana.ToString();
        AttackLabel.text = CurSelection.ActualAttack.ToString();
        DefenseLabel.text = CurSelection.ActualDefense.ToString();
        MagicLabel.text = CurSelection.ActualMagic.ToString();
        SpeedLabel.text = CurSelection.ActualSpeed.ToString();

        if(CurSelection.ArmorItem != null)
        {
            ArmorButton.style.backgroundImage = (StyleBackground)CurSelection.ArmorItem.Icon;
            ArmorButton.text = null;
        }
        else
        {
            ArmorButton.style.backgroundImage = null;
            ArmorButton.text = "Armor";
        }
        if (CurSelection.WeaponItem != null)
        {
            WeaponButton.style.backgroundImage = (StyleBackground)CurSelection.WeaponItem.Icon;
            WeaponButton.text = null;
        }
        else
        {
            WeaponButton.style.backgroundImage = null;
            WeaponButton.text = "Weapon";
        }
        if (CurSelection.AccessorySlot1 != null)
        {
            Accessory1Button.style.backgroundImage = (StyleBackground)CurSelection.AccessorySlot1.Icon;
            Accessory1Button.text = null;
        }
        else
        {
            Accessory1Button.style.backgroundImage = null;
            Accessory1Button.text = "Acessory";
        }
        if (CurSelection.AccessorySlot2 != null)
        {
            Accessory2Button.style.backgroundImage = (StyleBackground)CurSelection.AccessorySlot2.Icon;
            Accessory2Button.text = null;
        }
        else
        {
            Accessory2Button.style.backgroundImage = null;
            Accessory2Button.text = "Acessory";
        }


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

        if (CurSelection.ReadyToLevel == true)
        {
            LevelUpButton.style.visibility = Visibility.Visible;
        }
        else
        {
            LevelUpButton.style.visibility = Visibility.Hidden;
        }

    }
    void OpenItemList(String itemType, int slot = 0)
    {
        GameObject soundMaster = GameObject.FindGameObjectWithTag("SoundManager");
        SoundManager soundManager = soundMaster.GetComponent<SoundManager>();
        soundManager.ButtonSound();

        ItemType = itemType;
        curSlot = slot;
        GameObject UIMaster = GameObject.FindGameObjectWithTag("UI Manager");
        UIMaster.GetComponent<UIManager>().ShowEquipItemList();

        switch(itemType)
        {
            case "Armor":
                {
                    if(CurSelection.ArmorItem != null)
                    {
                        curEquippedItem = CurSelection.ArmorItem;
                    }
                    
                }
                break;
            case "Weapon":
                {
                    if (CurSelection.WeaponItem != null)
                    {
                        curEquippedItem = CurSelection.WeaponItem;
                    }
                }
                break;
            case "Accessory":
                {
                    if (slot == 1)
                    {
                        if (CurSelection.AccessorySlot1 != null)
                        {
                            curEquippedItem = CurSelection.AccessorySlot1;
                        }
                    }
                    else
                    {
                        if (CurSelection.AccessorySlot2 != null)
                        {
                            curEquippedItem = CurSelection.AccessorySlot2;
                        }
                    }

                }
                break;
        }

    }
    #region Buttons
    void PreviousAdventurer()
    {
        GameObject soundMaster = GameObject.FindGameObjectWithTag("SoundManager");
        SoundManager soundManager = soundMaster.GetComponent<SoundManager>();
        soundManager.ButtonSound();

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
        GameObject soundMaster = GameObject.FindGameObjectWithTag("SoundManager");
        SoundManager soundManager = soundMaster.GetComponent<SoundManager>();
        soundManager.ButtonSound();

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
        GameObject soundMaster = GameObject.FindGameObjectWithTag("SoundManager");
        SoundManager soundManager = soundMaster.GetComponent<SoundManager>();
        soundManager.LevelUpSound();

        CurSelection.LevelUp();
        FillAdventurerInfo();
    }

    void CloseStatsPage()
    {
        GameObject soundMaster = GameObject.FindGameObjectWithTag("SoundManager");
        SoundManager soundManager = soundMaster.GetComponent<SoundManager>();
        soundManager.ButtonSound();
        soundManager.LevelUpSound();

        StatsPopUp.style.display = DisplayStyle.None;
        
    }
    #endregion
    #endregion

    #region EquipItem

    VisualElement EquipScreen;
    VisualTreeAsset m_ItemListEntryTemplate;
    ListView ItemList;
    bool isItemOpen = false;
    List<Item> Inventory = new List<Item>();
    public List<Item> FilteredInventory = new List<Item>();
    Item NewItem;
    int curItemIndex;

    Button exitItems;
    Button EquipButton;
    Button ClearButton;

    public void InitializeEquipItemPopUp(VisualElement root, VisualTreeAsset listElementTemplate, List<Item> inventory)
    {
        EquipScreen = root;
        m_ItemListEntryTemplate = listElementTemplate;
        Inventory = inventory;

        if (isItemOpen == false)
        {
            ItemList = root.Q<ListView>("ItemList");

            exitItems = root.Q<Button>("ExitButton");
            EquipButton = root.Q<Button>("EquipButton");
            ClearButton = root.Q<Button>("ClearButton");

            ItemList.selectionChanged += OnItemSelected;
            exitItems.clicked += ExitItems;
            EquipButton.clicked += EquipItem;
            ClearButton.clicked += UnequipItem;
        }

        CreateFilteredInventory();
        FillItemList();
    }
    void CreateFilteredInventory()
    {
        FilteredInventory.Clear();
        foreach (Item item in Inventory)
        {
            if(item.itemtype.ToString() == ItemType &&  item != curEquippedItem)
            {
                FilteredInventory.Add(item);
            }
        }
    }
    void FillItemList()
    {
        ItemList.makeItem = () =>
        {
            var newListEntry = m_ItemListEntryTemplate.Instantiate();
            var newListEntryLogic = new EquipItemListEntry();
            newListEntry.userData = newListEntryLogic;
            newListEntryLogic.SetVisualElement(newListEntry);
            return newListEntry;
        };

        ItemList.bindItem = (item, index) =>
        {
            (item.userData as EquipItemListEntry)?.SetItemData(FilteredInventory[index]);
            item.userData = index;

        };

        ItemList.itemsSource = FilteredInventory;
    }
    void OnItemSelected(IEnumerable<object> item)
    {
        GameObject soundMaster = GameObject.FindGameObjectWithTag("SoundManager");
        SoundManager soundManager = soundMaster.GetComponent<SoundManager>();
        soundManager.ButtonSound();

        NewItem = FilteredInventory[ItemList.selectedIndex];
        curItemIndex = ItemList.selectedIndex;

        if (curEquippedItem != null) //there is an item there
        {
            if (curEquippedItem.HPBonus - NewItem.HPBonus != 0)
            {

                hpBonus.style.visibility = Visibility.Visible;

                if (NewItem.HPBonus - curEquippedItem.HPBonus > 0)
                {
                    hpBonus.text = "+" + (NewItem.HPBonus - curEquippedItem.HPBonus).ToString();
                    hpBonus.style.color = Color.green;
                }
                else
                {
                    hpBonus.text = "-" + (curEquippedItem.HPBonus - NewItem.HPBonus).ToString();
                    hpBonus.style.color = Color.red;
                }
            }
            else
            {
                hpBonus.style.visibility = Visibility.Hidden;
            }
            if (curEquippedItem.ManaBonus - NewItem.ManaBonus != 0)
            {
                manaBonus.style.visibility = Visibility.Visible;
                if (NewItem.ManaBonus - curEquippedItem.ManaBonus > 0)
                {
                    manaBonus.text = "+" + (NewItem.ManaBonus - curEquippedItem.ManaBonus).ToString();
                    manaBonus.style.color = Color.green;
                }
                else
                {
                    manaBonus.text = "-" + (curEquippedItem.ManaBonus - NewItem.ManaBonus).ToString();
                    manaBonus.style.color = Color.red;
                }
            }
            else
            {
                manaBonus.style.visibility = Visibility.Hidden;
            }
            if (NewItem.AtkBonus - curEquippedItem.AtkBonus != 0)
            {
                atkBonus.style.visibility = Visibility.Visible;
                if (NewItem.AtkBonus - curEquippedItem.AtkBonus > 0)
                {
                    atkBonus.text = "+" + (NewItem.AtkBonus - curEquippedItem.AtkBonus).ToString();
                    atkBonus.style.color = Color.green;
                }
                else
                {
                    atkBonus.text = "-" + (curEquippedItem.AtkBonus - NewItem.AtkBonus).ToString();
                    atkBonus.style.color = Color.red;
                }
            }
            else
            {
                atkBonus.style.visibility = Visibility.Hidden;
            }
            if (curEquippedItem.DefBonus - NewItem.DefBonus != 0)
            {
                defBonus.style.visibility = Visibility.Visible;
                if (NewItem.DefBonus - curEquippedItem.DefBonus > 0)
                {
                    defBonus.text = "+" + (NewItem.DefBonus - curEquippedItem.DefBonus).ToString();
                    defBonus.style.color = Color.green;
                }
                else
                {
                    defBonus.text = "-" + (curEquippedItem.DefBonus - NewItem.DefBonus).ToString();
                    defBonus.style.color = Color.red;
                }
            }
            else
            {
                defBonus.style.visibility = Visibility.Hidden;
            }
            if (curEquippedItem.MagBonus - NewItem.MagBonus != 0)
            {
                magBonus.style.visibility = Visibility.Visible;
                if (NewItem.MagBonus - curEquippedItem.MagBonus > 0)
                {
                    magBonus.text = "+" + (NewItem.MagBonus - curEquippedItem.MagBonus).ToString();
                    magBonus.style.color = Color.green;
                }
                else
                {
                    magBonus.text = "-" + (curEquippedItem.MagBonus - NewItem.MagBonus).ToString();
                    magBonus.style.color = Color.red;
                }
            }
            else
            {
                magBonus.style.visibility = Visibility.Hidden;
            }
            if (curEquippedItem.SpdBonus - NewItem.SpdBonus != 0)
            {
                spdBonus.style.visibility = Visibility.Visible;
                if (NewItem.SpdBonus - curEquippedItem.SpdBonus > 0)
                {
                    spdBonus.text = "+" + (NewItem.SpdBonus - curEquippedItem.SpdBonus).ToString();
                    spdBonus.style.color = Color.green;
                }
                else
                {
                    spdBonus.text = "-" + (curEquippedItem.SpdBonus - NewItem.SpdBonus).ToString();
                    spdBonus.style.color = Color.red;
                }
            }
            else
            {
                spdBonus.style.visibility = Visibility.Hidden;
            }
        }
        else // item slot is empty
        {
            if (NewItem.HPBonus != 0)
            {

                hpBonus.style.visibility = Visibility.Visible;

                if (NewItem.HPBonus > 0)
                {
                    hpBonus.text = "+" + (NewItem.HPBonus).ToString();
                    hpBonus.style.color = Color.green;
                }
                else
                {
                    hpBonus.text = (NewItem.HPBonus).ToString();
                    hpBonus.style.color = Color.red;
                }
            }
            else
            {
                hpBonus.style.visibility = Visibility.Hidden;
            }
            if (NewItem.ManaBonus != 0)
            {
                manaBonus.style.visibility = Visibility.Visible;
                if (NewItem.ManaBonus> 0)
                {
                    manaBonus.text = "+" + (NewItem.ManaBonus).ToString();
                    manaBonus.style.color = Color.green;
                }
                else
                {
                    manaBonus.text = (NewItem.ManaBonus).ToString();
                    manaBonus.style.color = Color.red;
                }
            }
            else
            {
                manaBonus.style.visibility = Visibility.Hidden;
            }
            if (NewItem.AtkBonus != 0)
            {
                atkBonus.style.visibility = Visibility.Visible;
                if (NewItem.AtkBonus > 0)
                {
                    atkBonus.text = "+" + (NewItem.AtkBonus).ToString();
                    atkBonus.style.color = Color.green;
                }
                else
                {
                    atkBonus.text = (NewItem.AtkBonus).ToString();
                    atkBonus.style.color = Color.red;
                }
            }
            else
            {
                atkBonus.style.visibility = Visibility.Hidden;
            }
            if (NewItem.DefBonus != 0)
            {
                defBonus.style.visibility = Visibility.Visible;
                if (NewItem.DefBonus > 0)
                {
                    defBonus.text = (NewItem.DefBonus).ToString();
                    defBonus.style.color = Color.green;
                }
                else
                {
                    defBonus.text = (NewItem.DefBonus).ToString();
                    defBonus.style.color = Color.red;
                }
            }
            else
            {
                defBonus.style.visibility = Visibility.Hidden;
            }
            if (NewItem.MagBonus != 0)
            {
                magBonus.style.visibility = Visibility.Visible;
                if (NewItem.MagBonus > 0)
                {
                    magBonus.text = "+" + (NewItem.MagBonus).ToString();
                    magBonus.style.color = Color.green;
                }
                else
                {
                    magBonus.text = (NewItem.MagBonus).ToString();
                    magBonus.style.color = Color.red;
                }
            }
            else
            {
                magBonus.style.visibility = Visibility.Hidden;
            }
            if (NewItem.SpdBonus != 0)
            {
                spdBonus.style.visibility = Visibility.Visible;
                if (NewItem.SpdBonus > 0)
                {
                    spdBonus.text = "+" + (NewItem.SpdBonus).ToString();
                    spdBonus.style.color = Color.green;
                }
                else
                {
                    spdBonus.text =(NewItem.SpdBonus).ToString();
                    spdBonus.style.color = Color.red;
                }
            }
            else
            {
                spdBonus.style.visibility = Visibility.Hidden;
            }
        }
    }

    void EquipItem()
    {
        GameObject soundMaster = GameObject.FindGameObjectWithTag("SoundManager");
        SoundManager soundManager = soundMaster.GetComponent<SoundManager>();
        soundManager.ButtonSound();

        if (CurSelection != null)
        {
            switch (ItemType)
            {
                case "Armor":
                    {
                        CurSelection.ArmorItem = (Armor)NewItem;
                    }
                    break;
                case "Weapon":
                    {
                        CurSelection.WeaponItem = (Weapon)NewItem;
                    }
                    break;
                case "Accessory":
                    {
                        if (curSlot == 1)
                        {
                            CurSelection.AccessorySlot1 = (Accessory)NewItem;
                        }
                        else
                        {
                            CurSelection.AccessorySlot2 = (Accessory)NewItem;
                        }

                    }
                    break;
            }
            ExitItems();

            GameObject GameMaster = GameObject.FindGameObjectWithTag("GameController");
            GameManager GameManager = GameMaster.GetComponent<GameManager>();
            if (curEquippedItem != null)
            {
                GameManager.AddToInventory(curEquippedItem);
            }
            GameManager.RemoveFromInventory(NewItem);
            CurSelection.RecalulateActualStats();
            FillAdventurerInfo();

        }       
    }
    void UnequipItem()
    {
        GameObject soundMaster = GameObject.FindGameObjectWithTag("SoundManager");
        SoundManager soundManager = soundMaster.GetComponent<SoundManager>();
        soundManager.ButtonSound();

        switch (ItemType)
        {
            case "Armor":
                {
                    CurSelection.ArmorItem = null;
                }
                break;
            case "Weapon":
                {
                    CurSelection.WeaponItem = null;
                }
                break;
            case "Accessory":
                {
                    if (curSlot == 1)
                    {
                        CurSelection.AccessorySlot1 = null;
                    }
                    else
                    {
                        CurSelection.AccessorySlot2 = null;
                    }

                }
                break;
        }
        ExitItems();

        GameObject GameMaster = GameObject.FindGameObjectWithTag("GameController");
        GameManager GameManager = GameMaster.GetComponent<GameManager>();
        GameManager.AddToInventory(curEquippedItem);
        
        CurSelection.RecalulateActualStats();
        FillAdventurerInfo();

    }
    void ExitItems()
    {
        GameObject soundMaster = GameObject.FindGameObjectWithTag("SoundManager");
        SoundManager soundManager = soundMaster.GetComponent<SoundManager>();
        soundManager.ButtonSound();

        hpBonus.style.visibility = Visibility.Hidden;
        manaBonus.style.visibility = Visibility.Hidden;
        atkBonus.style.visibility = Visibility.Hidden;
        defBonus.style.visibility = Visibility.Hidden;
        magBonus.style.visibility = Visibility.Hidden;
        spdBonus.style.visibility = Visibility.Hidden;

        EquipScreen.style.display = DisplayStyle.None;

    }
    #endregion
}
