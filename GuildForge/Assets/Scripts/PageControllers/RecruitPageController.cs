using Assets.PixelFantasy.PixelHeroes.Common.Scripts.CharacterScripts;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class RecruitPageController
{
    #region
    VisualElement RecruitPopUp;

    Label NameLabel;
    Label ClassLabel;
    Label LvLabel;
    Label RankLabel;
    Label HpLabel;
    Label ManaLabel;
    Label AttackLabel;
    Label DefenseLabel;
    Label MagicLabel;
    Label SpeedLabel;
    Label CostLabel;

    Button RecruitButton;
    Button ExitButton;
    Button BackButton;
    Button ForwardButton;

    Adventurer CurSelection;
    int CurIndex;
    List<Adventurer> localAdventurers;
    bool isOpen = false;
    bool recruiting = false;
    #endregion

    public void InitializeRecruitPage(VisualElement root)
    {
        RecruitPopUp = root;

        if (CreateList())
        {
            if (isOpen == false)
            {
                NameLabel = root.Q<Label>("Name");
                ClassLabel = root.Q<Label>("Class");
                LvLabel = root.Q<Label>("Level");
                RankLabel = root.Q<Label>("RankLabel");
                HpLabel = root.Q<Label>("HPLabel");
                ManaLabel = root.Q<Label>("ManaLabel");
                AttackLabel = root.Q<Label>("AttackLabel");
                DefenseLabel = root.Q<Label>("DefenseLabel");
                MagicLabel = root.Q<Label>("MagicLabel");
                SpeedLabel = root.Q<Label>("SpeedLabel");
                CostLabel = root.Q<Label>("CostLabel");

                RecruitButton = root.Q<Button>("RecruitBtnFinal");
                ExitButton = root.Q<Button>("ExitButton");
                BackButton = root.Q<Button>("BackButton");
                ForwardButton = root.Q<Button>("ForwardButton");

                RecruitButton.clicked += RecruitPressed;
                ExitButton.clicked += CloseRecruitPage;
                BackButton.clicked += PreviousAdventurer;
                ForwardButton.clicked += NextAdventurer;
            }

            CurIndex = 0;
            FillAdventurerInfo();
        }
    }

    bool CreateList()
    {
        GameObject GameMaster = GameObject.FindGameObjectWithTag("GameController");
        localAdventurers = new List<Adventurer>(GameMaster.GetComponent<GameManager>().FindLocalAdventurers(false));

        if (localAdventurers.Count > 0)
        {
            CurSelection = localAdventurers[CurIndex];
            return true;
        }
        else
        {
            VisualElement noAdventurers = RecruitPopUp.Q<VisualElement>("NoAdventurers");
            noAdventurers.style.display = DisplayStyle.Flex;

            Button okayButton = noAdventurers.Q<Button>("OkayButton");
            okayButton.clicked += CloseRecruitPage;
            return false;
        }
        
    }
    void FillAdventurerInfo()
    {
        NameLabel.text = CurSelection.Name;
        ClassLabel.text = CurSelection.Class.ToString();
        LvLabel.text = CurSelection.Level.ToString();
        RankLabel.text = CurSelection.Rank.ToString();
        HpLabel.text = CurSelection.ActualHP.ToString();
        ManaLabel.text = CurSelection.ActualMana.ToString();
        AttackLabel.text = CurSelection.ActualAttack.ToString();
        DefenseLabel.text = CurSelection.ActualDefense.ToString();
        MagicLabel.text = CurSelection.ActualMagic.ToString();
        SpeedLabel.text = CurSelection.ActualSpeed.ToString();
        CostLabel.text = CurSelection.CostToRecruit.ToString();

        GameObject GameMaster = GameObject.FindGameObjectWithTag("GameController");
        GameManager GameManager = GameMaster.GetComponent<GameManager>();

        if (CurSelection.CostToRecruit > GameManager.getCurGold())
        {
            RecruitButton.SetEnabled(false);
        }
        else
        {
            RecruitButton.SetEnabled(true);
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
   
    #region Buttons
    void PreviousAdventurer()
    {
        GameObject soundMaster = GameObject.FindGameObjectWithTag("SoundManager");
        SoundManager soundManager = soundMaster.GetComponent<SoundManager>();
        soundManager.ButtonSound();

        CurIndex -= 1;
        if (CurIndex < 0)
        {
            CurIndex = localAdventurers.Count - 1;
        }
        CurSelection = localAdventurers[CurIndex];

        FillAdventurerInfo();
    }

    void NextAdventurer()
    {
        GameObject soundMaster = GameObject.FindGameObjectWithTag("SoundManager");
        SoundManager soundManager = soundMaster.GetComponent<SoundManager>();
        soundManager.ButtonSound();

        if (localAdventurers.Count > 0)
        {
            CurIndex += 1;
            Debug.Log(CurIndex);
            if (CurIndex > localAdventurers.Count - 1)
            {
                CurIndex = 0;
            }
            CurSelection = localAdventurers[CurIndex];

            FillAdventurerInfo();
        }
        else
        {
            CloseRecruitPage();
        }

    }
    void RecruitPressed()   
    {
        Debug.Log(CurSelection.Name + " Recruiting");
        GameObject GameMaster = GameObject.FindGameObjectWithTag("GameController");
        GameManager GameManager = GameMaster.GetComponent<GameManager>();

        if (recruiting == false && GameManager.getCurGold() >= CurSelection.CostToRecruit)
        {
            GameObject soundMaster = GameObject.FindGameObjectWithTag("SoundManager");
            SoundManager soundManager = soundMaster.GetComponent<SoundManager>();
            soundManager.MoneySound();

            recruiting = true;
            CurSelection.Recruited = true;

            GameManager.Recruit(CurSelection);

            localAdventurers.Remove(CurSelection);

            CreateList();
            NextAdventurer();
            recruiting = false;
        }
    }

    void CloseRecruitPage()
    {
        GameObject soundMaster = GameObject.FindGameObjectWithTag("SoundManager");
        SoundManager soundManager = soundMaster.GetComponent<SoundManager>();
        soundManager.ButtonSound();

        VisualElement noAdventurers = RecruitPopUp.Q<VisualElement>("NoAdventurers");
        noAdventurers.style.display = DisplayStyle.None;
        RecruitPopUp.style.display = DisplayStyle.None; 

        GameObject UIMaster = GameObject.FindGameObjectWithTag("UI Manager");
        UIMaster.GetComponent<UIManager>().RosterBtn_clicked();
    }
    #endregion
}
