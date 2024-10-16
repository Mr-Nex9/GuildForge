using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameState gameState;
    [SerializeField] VisualTreeAsset m_RosterListEntryTemplate;
    [SerializeField] VisualTreeAsset m_NewMissionListEntryTemplate;
    [SerializeField] VisualTreeAsset m_EquipItemListEntryTemplate;
    private VisualElement curPage;
    private VisualElement curPopUp;
    private float timePassed;
    bool franchiseButtonActivated;

    MissionDetailsController missionDetailsController;
    MissionListController missionListController;
    ActiveMissionListController activemissionListController;
    RosterController rosterController;
    RecruitPageController recruitPageController;
    StorePageController storePageController;
    SettingsPageController settingsPageController;
    bool alreadyLoaded = false;
    #endregion

    private void OnEnable()
    {
        ClosePopUps();
        var uiDocument = GetComponent<UIDocument>();
        var HomePage = uiDocument.rootVisualElement.Q<VisualElement>("HomePage");
        var Footer = uiDocument.rootVisualElement.Q<ToggleButtonGroup>("Footer");
        curPage = HomePage;
        HomeBtn_clicked();
        alreadyLoaded = true;

        var AchievementsBtn = Footer.Q<Button>("AchievementsButton");
        var StoreBtn = Footer.Q<Button>("InventoryButton");
        var HomeBtn = Footer.Q<Button>("HomeButton");
        var RosterBtn = Footer.Q<Button>("RosterButton");
        var SettingsBtn = Footer.Q<Button>("SettingsButton");

        AchievementsBtn.clicked += AchievementsBtn_onClick;
        StoreBtn.clicked += StoreBtn_clicked;
        HomeBtn.clicked += HomeBtn_clicked;
        RosterBtn.clicked += RosterBtn_clicked;
        SettingsBtn.clicked += SettingsBtn_clicked;
        

    }
    private void LateUpdate()
    {
        timePassed += Time.deltaTime;
        if (timePassed > 1)
        {
            if (activemissionListController == null)
            {
                activemissionListController = new ActiveMissionListController();
                activemissionListController.MissionUIUpdate();
            }
            else
            {
                activemissionListController.MissionUIUpdate();
            };
            timePassed = 0;
        }

    }
    #region Home Methods
    public void HomeBtn_clicked()
    {
        if (alreadyLoaded)
        {
            GameObject soundMaster = GameObject.FindGameObjectWithTag("SoundManager");
            SoundManager soundManager = soundMaster.GetComponent<SoundManager>();
            soundManager.ButtonSound();
        }
        curPage.style.display = DisplayStyle.None;
        ClosePopUps();

        var uiDocument = GetComponent<UIDocument>();
        var HomePage = uiDocument.rootVisualElement.Q<VisualElement>("HomePage");
        curPage = HomePage;
        curPage.style.display = DisplayStyle.Flex;


        if (missionListController == null)
        {
            missionListController = new MissionListController();
            missionListController.InitializeMissionList(HomePage, m_NewMissionListEntryTemplate);
        }
        else
        {
            missionListController.InitializeMissionList(HomePage, m_NewMissionListEntryTemplate);
        }

        if (activemissionListController == null)
        {
            activemissionListController = new ActiveMissionListController();
            activemissionListController.InitializeMissionList(HomePage,gameState.activeMissions);
        }
        else
        {
            activemissionListController.InitializeMissionList(HomePage, gameState.activeMissions);
        }

    }
    public void MissionDetails(int missionId)
    {
        var uiDocument = GetComponent<UIDocument>();
        var MissionDetails = uiDocument.rootVisualElement.Q<VisualElement>("MissionDetailsPopUp");
        MissionDetails.style.display = DisplayStyle.Flex;
        if (missionDetailsController == null)
        {
            missionDetailsController = new MissionDetailsController();
            missionDetailsController.InitializePage(MissionDetails, missionId);
        }
        else
        {
            missionDetailsController.InitializePage(MissionDetails, missionId);
        }

    }
    public void SelectAdventurer()
    {
        var uiDocument = GetComponent<UIDocument>();
        var RosterPopUp = uiDocument.rootVisualElement.Q<VisualElement>("RosterPopUp");
        RosterPopUp.style.display = DisplayStyle.Flex;

        missionDetailsController.InitializeRosterList(RosterPopUp, m_RosterListEntryTemplate, gameState.roster);
    }
    public void ShowMissionCompletePopUp()
    {
        var uiDocument = GetComponent<UIDocument>();
        var missionComplete = uiDocument.rootVisualElement.Q<VisualElement>("MissionCompletePopUp");
        missionComplete.style.display = DisplayStyle.Flex;

        activemissionListController.InitializeMissionPopUp(missionComplete);
    }
    #endregion

    #region Roster Methods
    public void RosterBtn_clicked()
    {
        GameObject soundMaster = GameObject.FindGameObjectWithTag("SoundManager");
        SoundManager soundManager = soundMaster.GetComponent<SoundManager>();
        soundManager.ButtonSound();

        curPage.style.display = DisplayStyle.None;
        ClosePopUps();

        var uiDocument = GetComponent<UIDocument>();
        var RosterPage = uiDocument.rootVisualElement.Q<VisualElement>("RosterPage");
        curPage = RosterPage;
        curPage.style.display = DisplayStyle.Flex;

        if (rosterController == null)
        {
            rosterController = new RosterController();
            rosterController.InitializeRosterList(RosterPage, m_RosterListEntryTemplate, gameState.roster);
        }
        else
        {
            rosterController.InitializeRosterList(RosterPage, m_RosterListEntryTemplate, gameState.roster);
        };
    }
    public void ShowAdventurerStats()
    {
        var uiDocument = GetComponent<UIDocument>();
        var StatsDisplay = uiDocument.rootVisualElement.Q<VisualElement>("AdventurerStatsPopUp");
        StatsDisplay.style.display = DisplayStyle.Flex;

        rosterController.InitializeStatsPage(StatsDisplay);
    }
    public void ShowRecruitPage()
    {
        var uiDocument = GetComponent<UIDocument>();
        var RecruitPopUp = uiDocument.rootVisualElement.Q<VisualElement>("RecruitPopUp");
        RecruitPopUp.style.display = DisplayStyle.Flex;

        if (recruitPageController == null)
        {
            recruitPageController = new RecruitPageController();
            recruitPageController.InitializeRecruitPage(RecruitPopUp);
        }
        else
        {
            recruitPageController.InitializeRecruitPage(RecruitPopUp);
        };
    }

    public void ShowEquipItemList()
    {
        var uiDocument = GetComponent<UIDocument>();
        var ItemDisplay = uiDocument.rootVisualElement.Q<VisualElement>("ItemEquipPopUp");
        ItemDisplay.style.display = DisplayStyle.Flex;

        rosterController.InitializeEquipItemPopUp(ItemDisplay, m_EquipItemListEntryTemplate, gameState.inventory);
    }
    #endregion

    #region Store Methods
    private void StoreBtn_clicked()
    {
        GameObject soundMaster = GameObject.FindGameObjectWithTag("SoundManager");
        SoundManager soundManager = soundMaster.GetComponent<SoundManager>();
        soundManager.ButtonSound();

        curPage.style.display = DisplayStyle.None;
        ClosePopUps();

        var uiDocument = GetComponent<UIDocument>();
        var storePage = uiDocument.rootVisualElement.Q<VisualElement>("StorePage");
        curPage = storePage;
        curPage.style.display = DisplayStyle.Flex;

        if (storePageController == null)
        {
            storePageController = new StorePageController();
            storePageController.InitializeStorePage(storePage);
        }
        else
        {
            storePageController.InitializeStorePage(storePage);
        };
    }
    public void ShowItemDetailsPopUp()
    {
        var uiDocument = GetComponent<UIDocument>();
        var ItemDetails = uiDocument.rootVisualElement.Q<VisualElement>("ItemDetailsPopUp");
        ItemDetails.style.display = DisplayStyle.Flex;

        storePageController.InitializeDetailsPopUp(ItemDetails);
    }
    #endregion

    #region Franchise Button Methods
    public void ActivateFranchiseButton(bool on)
    {
        var uiDocument = GetComponent<UIDocument>();
        var header = uiDocument.rootVisualElement.Q<VisualElement>("Header");

        Button franchiseButton = header.Q<Button>("FranchiseButton");

        if (franchiseButtonActivated == false)
        {
            franchiseButtonActivated = true;
            franchiseButton.clicked += ShowResetPopUp;
        }

        if (on)
        {
            franchiseButton.style.display = DisplayStyle.Flex;
        }
        else
        {
            franchiseButton.style.display = DisplayStyle.None;
        }
    }
    private void ShowResetPopUp()
    {
        GameObject soundMaster = GameObject.FindGameObjectWithTag("SoundManager");
        SoundManager soundManager = soundMaster.GetComponent<SoundManager>();
        soundManager.ButtonSound();

        var uiDocument = GetComponent<UIDocument>();
        var franchisePopUp = uiDocument.rootVisualElement.Q<VisualElement>("FranchisePopUp");

        franchisePopUp.style.display = DisplayStyle.Flex;

        Label totalGold = franchisePopUp.Q<Label>("totalGold");
        Label bonus = franchisePopUp.Q<Label>("Bonus");
        Button resetButton = franchisePopUp.Q<Button>("ResetButton");

        totalGold.text = gameState.totalGold.ToString();
        GameObject GameMaster = GameObject.FindGameObjectWithTag("GameController");
        
        bonus.text = GameMaster.GetComponent<GameManager>().CalculateBonus().ToString();

        resetButton.clicked += GameMaster.GetComponent<GameManager>().FranchiseGuild;
    }
    #endregion

    private void AchievementsBtn_onClick()
    {
        GameObject soundMaster = GameObject.FindGameObjectWithTag("SoundManager");
        SoundManager soundManager = soundMaster.GetComponent<SoundManager>();
        soundManager.ButtonSound();

        curPage.style.display = DisplayStyle.None;
        ClosePopUps();

        var uiDocument = GetComponent<UIDocument>();
        var AchievementsPage = uiDocument.rootVisualElement.Q<VisualElement>("AchievementsPage");
        curPage = AchievementsPage;
        curPage.style.display = DisplayStyle.Flex;
    }
    private void SettingsBtn_clicked()
    {
        GameObject soundMaster = GameObject.FindGameObjectWithTag("SoundManager");
        SoundManager soundManager = soundMaster.GetComponent<SoundManager>();
        soundManager.ButtonSound();

        curPage.style.display = DisplayStyle.None;
        ClosePopUps();

        var uiDocument = GetComponent<UIDocument>();
        var settingsPage = uiDocument.rootVisualElement.Q<VisualElement>("SettingsPage");
        curPage = settingsPage;
        curPage.style.display = DisplayStyle.Flex;


        if (settingsPageController == null)
        {
            settingsPageController = new SettingsPageController();
            settingsPageController = new SettingsPageController();
            settingsPageController.InitializePage(settingsPage, gameState);
        }
        else
        {
            settingsPageController.InitializePage(settingsPage, gameState);
        }
    }

    void ClosePopUps()
    {
        var uiDocument = GetComponent<UIDocument>();

        var ItemDisplay = uiDocument.rootVisualElement.Q<VisualElement>("ItemEquipPopUp");
        ItemDisplay.style.display = DisplayStyle.None;

        var RecruitPopUp = uiDocument.rootVisualElement.Q<VisualElement>("RecruitPopUp");
        RecruitPopUp.style.display = DisplayStyle.None;

        var StatsDisplay = uiDocument.rootVisualElement.Q<VisualElement>("AdventurerStatsPopUp");
        StatsDisplay.style.display = DisplayStyle.None;

        var RosterPopUp = uiDocument.rootVisualElement.Q<VisualElement>("RosterPopUp");
        RosterPopUp.style.display = DisplayStyle.None;

        var MissionDetails = uiDocument.rootVisualElement.Q<VisualElement>("MissionDetailsPopUp");
        MissionDetails.style.display = DisplayStyle.None;

        var ItemDetails = uiDocument.rootVisualElement.Q<VisualElement>("ItemDetailsPopUp");
        ItemDetails.style.display = DisplayStyle.None;
    }
}
