using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class SettingsPageController
{
    GameState gameState;
    GameManager gameManager;
    VisualElement settingsPage;
    bool setup = false;

    SliderInt sound;
    SliderInt music;
    SliderInt effects;
    SliderInt notifications;
    SliderInt password;
    Button gameReset;
    Button savePassword;
    VisualElement passwordBox;
    TextField passwordField;

    Label soundOn;
    Label musicOn;
    Label effectsOn;
    Label notificationsOn;
    Label passwordOn;

    public void InitializePage(VisualElement root, GameState state)
    {
        settingsPage = root;
        gameState = state;

        if (setup == false)
        {
            passwordBox = settingsPage.Q<VisualElement>("PasswordBox");
            sound = settingsPage.Q<SliderInt>("Sound");
            music = settingsPage.Q<SliderInt>("Music");
            effects = settingsPage.Q<SliderInt>("Effects");
            notifications = settingsPage.Q<SliderInt>("Notification");
            password = settingsPage.Q<SliderInt>("Password");
            gameReset = settingsPage.Q<Button>("GameReset");
            savePassword = settingsPage.Q<Button>("SavePassword");
            passwordField = settingsPage.Q<TextField>("PasswordField");

            soundOn = settingsPage.Q<Label>("SoundOn");
            musicOn = settingsPage.Q<Label>("MusicOn");
            effectsOn = settingsPage.Q<Label>("EffectsOn");
            notificationsOn = settingsPage.Q<Label>("NotificationOn");
            passwordOn = settingsPage.Q<Label>("PasswordOn");

            gameReset.clicked += ResetGame;
            savePassword.clicked += SavePassword;
            sound.RegisterValueChangedCallback(SoundChanged);
            music.RegisterValueChangedCallback(MusicChanged);
            effects.RegisterValueChangedCallback(EffectsChanged);
            notifications.RegisterValueChangedCallback(NotificationsChanged);
            password.RegisterValueChangedCallback(PasswordChanged);

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
        soundOn.text = sound.value == 1 ? "On" : "Off";

        music.value = gameManager.GetSettings(1) ? 1 : 0;
        musicOn.text = music.value == 1 ? "On" : "Off";

        effects.value = gameManager.GetSettings(2) ? 1 : 0;
        effectsOn.text = effects.value == 1 ? "On" : "Off";

        notifications.value = gameManager.GetSettings(3) ? 1 : 0;
        notificationsOn.text = notifications.value == 1 ? "On" : "Off";

        password.value = gameManager.GetSettings(4) ? 1 : 0;
        passwordOn.text = password.value == 1 ? "On" : "Off";
        passwordBox.style.display = password.value == 1 ? DisplayStyle.Flex : DisplayStyle.None;

    }
    void ResetTutorial()
    {
        GameObject soundMaster = GameObject.FindGameObjectWithTag("SoundManager");
        SoundManager soundManager = soundMaster.GetComponent<SoundManager>();
        soundManager.ButtonSound();

        Debug.Log("Tutorial reset button clicked");
    }

    void ResetGame()
    {
        GameObject soundMaster = GameObject.FindGameObjectWithTag("SoundManager");
        SoundManager soundManager = soundMaster.GetComponent<SoundManager>();
        soundManager.ButtonSound();

        Debug.Log("Reset Game");
        gameManager.NewGame(true);
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
            gameManager.SetSettings(1, false);
            musicOn.text = "Off";
            gameManager.SetSettings(2, false);
            effectsOn.text = "Off";

            music.value = gameManager.GetSettings(1) ? 1 : 0;
            effects.value = gameManager.GetSettings(2) ? 1 : 0;
        }
        GameObject soundMaster = GameObject.FindGameObjectWithTag("SoundManager");
        SoundManager soundManager = soundMaster.GetComponent<SoundManager>();
        soundManager.ButtonSound();
    }
    void MusicChanged(ChangeEvent<int> value)
    {
        GameObject soundMaster = GameObject.FindGameObjectWithTag("SoundManager");
        SoundManager soundManager = soundMaster.GetComponent<SoundManager>();
        soundManager.ButtonSound();

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
        GameObject soundMaster = GameObject.FindGameObjectWithTag("SoundManager");
        SoundManager soundManager = soundMaster.GetComponent<SoundManager>();
        soundManager.ButtonSound();
    }
    void NotificationsChanged(ChangeEvent<int> value)
    {
        GameObject soundMaster = GameObject.FindGameObjectWithTag("SoundManager");
        SoundManager soundManager = soundMaster.GetComponent<SoundManager>();
        soundManager.ButtonSound();

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
    void PasswordChanged(ChangeEvent<int> value)
    {
        GameObject soundMaster = GameObject.FindGameObjectWithTag("SoundManager");
        SoundManager soundManager = soundMaster.GetComponent<SoundManager>();
        soundManager.ButtonSound();

        if (password.value == 1)
        {
            passwordOn.text = "On";
            passwordBox.style.display = DisplayStyle.Flex;
        }
        else
        {
            gameManager.SetSettings(4, false);
            passwordOn.text = "Off";
            passwordBox.style.display = DisplayStyle.None;
        }
    }

    void SavePassword()
    {
        GameObject soundMaster = GameObject.FindGameObjectWithTag("SoundManager");
        SoundManager soundManager = soundMaster.GetComponent<SoundManager>();
        GameObject UIMaster = GameObject.FindGameObjectWithTag("UI Manager");
        if (passwordField.value.Length > 0 && passwordField.value.All(char.IsDigit))
        {

            soundManager.ButtonSound();
            
            gameManager.SetSettings(4, true, passwordField.value);
            UIMaster.GetComponent<UIManager>().ErrorMessage("Pasword Set!");
        }
        else
        {
            soundManager.ErrorSound();

            UIMaster.GetComponent<UIManager>().ErrorMessage("Password must be numbers 0-9 only!");
        }
    }
}
