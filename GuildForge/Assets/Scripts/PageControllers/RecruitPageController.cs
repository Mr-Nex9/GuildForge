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
    List<Adventurer> LocalAdventurers;
    bool isOpen = false;
    #endregion

    public void InitializeRecruitPage(VisualElement root)
    {
        RecruitPopUp = root;
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
        CreateList();
        FillAdventurerInfo();

    }

    void CreateList()
    {
        List<Adventurer> temp = new List<Adventurer>();
        temp.AddRange(Resources.LoadAll<Adventurer>("Adventurers"));
        LocalAdventurers = new List<Adventurer>();

        foreach (Adventurer adventurer in temp)
        {
            if (adventurer.Recruited == false)
            {
                LocalAdventurers.Add(adventurer);

            }
        }
        CurSelection = LocalAdventurers[CurIndex];
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
        CurIndex -= 1;
        if (CurIndex < 0)
        {
            CurIndex = LocalAdventurers.Count - 1;
        }
        CurSelection = LocalAdventurers[CurIndex];

        FillAdventurerInfo();
    }

    void NextAdventurer()
    {
        CurIndex += 1;
        if (CurIndex > LocalAdventurers.Count - 1)
        {
            CurIndex = 0;
        }
        CurSelection = LocalAdventurers[CurIndex];

        FillAdventurerInfo();
    }
    void RecruitPressed()
    {
        LocalAdventurers.Remove(CurSelection);
        CurSelection.Recruited = true;

        GameObject GameMaster = GameObject.FindGameObjectWithTag("GameController");
        GameManager GameManager = GameMaster.GetComponent<GameManager>();
        GameManager.Recruit(CurSelection);
        GameManager.PayGold(CurSelection.CostToRecruit);
        switch (CurSelection.Rank)
        {
            case Adventurer.AdventurerRanks.S:
                {
                    GameManager.GainReputation(100);
                }break;
            case Adventurer.AdventurerRanks.A:
                {
                    GameManager.GainReputation(80);
                }
                break;
            case Adventurer.AdventurerRanks.B:
                {
                    GameManager.GainReputation(60);
                }
                break;
            case Adventurer.AdventurerRanks.C:
                {
                    GameManager.GainReputation(40);
                }
                break;
            case Adventurer.AdventurerRanks.D:
                {
                    GameManager.GainReputation(20);
                }
                break;

        }
        NextAdventurer();
    }

    void CloseRecruitPage()
    {
        RecruitPopUp.style.display = DisplayStyle.None;
    }
    #endregion
}
