using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameState gameState;
    [SerializeField] VisualTreeAsset m_RosterListEntryTemplate;
    [SerializeField] VisualTreeAsset m_NewMissionListEntryTemplate;
    [SerializeField] VisualTreeAsset m_ActiveMissionListEntryTemplate;

    private VisualElement curPage;
    private VisualElement curPopUp;
    private float timePassed;

    MissionDetailsController missionDetailsController;
    MissionListController missionListController;
    ActiveMissionListController activemissionListController;
    AdventurerListController adventurerlistcontroller;
    private void OnEnable()
    {

        var uiDocument = GetComponent<UIDocument>();
        var QuestPage = uiDocument.rootVisualElement.Q<VisualElement>("QuestPage");
        var Footer = uiDocument.rootVisualElement.Q<ToggleButtonGroup>("Footer");
        curPage = QuestPage;
        HomeBtn_clicked();


        var AchievementsBtn = Footer.Q<Button>("AchievementsButton");
        var InventoryBtn = Footer.Q<Button>("InventoryButton");
        var HomeBtn = Footer.Q<Button>("HomeButton");
        var RosterBtn = Footer.Q<Button>("RosterButton");
        var SettingsBtn = Footer.Q<Button>("SettingsButton");

        AchievementsBtn.clicked += AchievementsBtn_onClick;
        InventoryBtn.clicked += InventoryBtn_clicked;
        HomeBtn.clicked += HomeBtn_clicked;
        RosterBtn.clicked += RosterBtn_clicked;
        SettingsBtn.clicked += SettingsBtn_clicked;
        

    }

    public void HomeBtn_clicked()
    {
        curPage.style.display = DisplayStyle.None;
        var uiDocument = GetComponent<UIDocument>();
        var QuestPage = uiDocument.rootVisualElement.Q<VisualElement>("QuestPage");
        curPage = QuestPage;
        curPage.style.display = DisplayStyle.Flex;


        if (missionListController == null)
        {
            missionListController = new MissionListController();
            missionListController.InitializeMissionList(QuestPage, m_NewMissionListEntryTemplate);
        }
        else
        {
            missionListController.InitializeMissionList(QuestPage, m_NewMissionListEntryTemplate);
        };
        if (activemissionListController == null)
        {
            activemissionListController = new ActiveMissionListController();
            activemissionListController.InitializeMissionList(QuestPage, m_ActiveMissionListEntryTemplate);
        }
        else
        {
            activemissionListController.InitializeMissionList(QuestPage, m_ActiveMissionListEntryTemplate);
        };

    }
    private void RosterBtn_clicked()
    {
        curPage.style.display = DisplayStyle.None;
        var uiDocument = GetComponent<UIDocument>();
        var RosterPage = uiDocument.rootVisualElement.Q<VisualElement>("RosterPage");
        curPage = RosterPage;
        curPage.style.display = DisplayStyle.Flex;

        if (adventurerlistcontroller == null)
        {
            adventurerlistcontroller = new AdventurerListController();
            adventurerlistcontroller.InitializeRosterList(RosterPage, m_RosterListEntryTemplate, gameState.Roster);
        }
        else
        {
            adventurerlistcontroller.InitializeRosterList(RosterPage, m_RosterListEntryTemplate, gameState.Roster);
        };
    }

    private void InventoryBtn_clicked()
    {
        curPage.style.display = DisplayStyle.None;
        var uiDocument = GetComponent<UIDocument>();
        var InventoryPage = uiDocument.rootVisualElement.Q<VisualElement>("InventoryPage");
        curPage = InventoryPage;
        curPage.style.display = DisplayStyle.Flex;
    }

    private void AchievementsBtn_onClick()
    {
        curPage.style.display = DisplayStyle.None;
        var uiDocument = GetComponent<UIDocument>();
        var AchievementsPage = uiDocument.rootVisualElement.Q<VisualElement>("AchievementsPage");
        curPage = AchievementsPage;
        curPage.style.display = DisplayStyle.Flex;
    }
    private void SettingsBtn_clicked()
    {
        throw new System.NotImplementedException();
    }

    public void MissionDetails(int  missionId)
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

        missionDetailsController.InitializeRosterList(RosterPopUp, m_RosterListEntryTemplate, gameState.Roster);
    }

    public void ShowAdventurerStats()
    {
        var uiDocument = GetComponent<UIDocument>();
        var StatsDisplay = uiDocument.rootVisualElement.Q<VisualElement>("AdventurerStatsPopUp");
        StatsDisplay.style.display = DisplayStyle.Flex;
        if (adventurerlistcontroller == null)
        {
            adventurerlistcontroller = new AdventurerListController();
            adventurerlistcontroller.InitializeStatsPage(StatsDisplay);
        }
        else
        {
            adventurerlistcontroller.InitializeStatsPage(StatsDisplay);
        }
    }
}
