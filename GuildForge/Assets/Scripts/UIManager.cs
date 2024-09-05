using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{

    [SerializeField] private GameState gameState;
    [SerializeField] VisualTreeAsset m_ListEntryTemplate;
    private VisualElement curPage;

    private void OnEnable()
    {

        var uiDocument = GetComponent<UIDocument>();
        var QuestPage = uiDocument.rootVisualElement.Q<VisualElement>("QuestPage");
        var Footer = uiDocument.rootVisualElement.Q<ToggleButtonGroup>("Footer");
        curPage = QuestPage;
        curPage.style.display = DisplayStyle.Flex;


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

    private void SettingsBtn_clicked()
    {
        throw new System.NotImplementedException();
    }

    private void RosterBtn_clicked()
    {
        curPage.style.display = DisplayStyle.None;
        var uiDocument = GetComponent<UIDocument>();
        var RosterPage = uiDocument.rootVisualElement.Q<VisualElement>("RosterPage");
        curPage = RosterPage;
        curPage.style.display = DisplayStyle.Flex;

        var adventurerlistcontroller = new AdventurerListController();
        adventurerlistcontroller.InitializeRosterList(RosterPage, m_ListEntryTemplate);

    }

    private void HomeBtn_clicked()
    {
        curPage.style.display = DisplayStyle.None;
        var uiDocument = GetComponent<UIDocument>();
        var QuestPage = uiDocument.rootVisualElement.Q<VisualElement>("QuestPage");
        curPage = QuestPage;
        curPage.style.display = DisplayStyle.Flex;
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
}
