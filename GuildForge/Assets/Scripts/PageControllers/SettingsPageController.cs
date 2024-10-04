using Unity.Notifications.iOS;
using UnityEngine;
using UnityEngine.UIElements;

public class SettingsPageController
{
    GameState gameState;
    GameManager gameManager;
    VisualElement settingsPage;
    bool setup = false;

    Button tutorialReset;
    SliderInt sound;
    SliderInt music;
    SliderInt effects;
    SliderInt notifications;
    Button GameReset;

    Label soundOn;
    Label musicOn;
    Label effectsOn;
    Label notificationsOn;

    public void InitializePage(VisualElement root, GameState state)
    {
        settingsPage = root;
        gameState = state;

        if (setup == false)
        {
            tutorialReset = settingsPage.Q<Button>("TutorialReset");
            sound = settingsPage.Q<SliderInt>("Sound");
            music = settingsPage.Q<SliderInt>("Music");
            effects = settingsPage.Q<SliderInt>("Effects");
            notifications = settingsPage.Q<SliderInt>("Notifications");
            GameReset = settingsPage.Q<Button>("GameReset");

            soundOn = settingsPage.Q<Label>("SoundOn");
            musicOn = settingsPage.Q<Label>("MusicOn");
            effectsOn = settingsPage.Q<Label>("EffectsOn");
            notificationsOn = settingsPage.Q<Label>("NotificationsOn");

            tutorialReset.clicked += ResetTutorial;
            GameReset.clicked += ResetGame;
            sound.RegisterValueChangedCallback(SoundChanged);
            music.RegisterValueChangedCallback(MusicChanged);
            effects.RegisterValueChangedCallback(EffectsChanged);
            notifications.RegisterValueChangedCallback(NotificationsChanged);

            setup = true;
            Debug.Log("Settings Setup");
        }

        GameObject GameMaster = GameObject.FindGameObjectWithTag("GameController");
        gameManager = GameMaster.GetComponent<GameManager>();

        SetUI();


    }
    void SetUI()
    {
        sound.value = gameManager.GetSettings(0) ? 1 : 0;
        music.value = gameManager.GetSettings(1) ? 1 : 0;
        effects.value = gameManager.GetSettings(2) ? 1 : 0;
        notifications.value = gameManager.GetSettings(3) ? 1 : 0;
    }
    void ResetTutorial()
    {
        Debug.Log("Tutorial reset button clicked");
    }

    void ResetGame()
    {
        Debug.Log("Reset Game button clicked");
    }

    void SoundChanged(ChangeEvent<int> value)
    {
        if (sound.value == 1)
        {
            gameManager.SetSettings(0, true);
            soundOn.text = "On";
        }
        else
        {
            gameManager.SetSettings(0, false);
            soundOn.text = "Off";
        }
    }
    void MusicChanged(ChangeEvent<int> value)
    {
        if (music.value == 1)
        {
            gameManager.SetSettings(1, true);
            musicOn.text = "On";
        }
        else
        {
            gameManager.SetSettings(1, false);
            musicOn.text = "Off";
        }
        
    }
    void EffectsChanged(ChangeEvent<int> value)
    {
        if (effects.value == 1)
        {
            gameManager.SetSettings(2, true);
            effectsOn.text = "On";
        }
        else
        {
            gameManager.SetSettings(2, false);
            effectsOn.text = "Off";
        }
    }
    void NotificationsChanged(ChangeEvent<int> value)
    {
        if (notifications.value == 1)
        {
            gameManager.SetSettings(3, true);
            notificationsOn.text = "On";
        }
        else
        {
            gameManager.SetSettings(3, false);
            notificationsOn.text = "Off";
        }
    }
}
