using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Events;

public class Mission : ScriptableObject
{
    public enum MissionType { Collection, Treasure, Hunt, Target, Guard, Rescue };
    public enum MissionRank { S, A, B, C, D };
    public UnityEvent<int, int> MissionComplete;


    [Header("Mission Details")]
    public int ID;
    [SerializeField] public bool Active;
    [SerializeField] public bool Offered;
    [SerializeField] public bool Completed;
    [SerializeField] public string Name;
    [SerializeField] public string Description;
    public MissionType Type;
    [SerializeField][Range(6, 60)] public int Difficulty;
    [SerializeField] public MissionRank Rank;
    public int EXPValue;
    public int GoldValue;
    public int ReputationValue;



    [Header("Adventurer Details")]
    [SerializeField] public List<Adventurer> AssignedAdventurers = new List<Adventurer>();

    [Header("Completion Time Variables")]
    public float CompletionPercent
    {
        get
        {
            TimeSpan elapsed = DateTime.Now - StartTime;
            float x = (float)((elapsed.TotalSeconds / TimeToCompleteInSeconds) * 100);
            if( x < 100)
            {
                return x;
            }
            else
            {
                return 100;
            }

        }
    }
    public System.DateTime StartTime;
    public System.DateTime EndTime;
    public int TimeToCompleteInSeconds;
    private float AdventurerReduction = 0;

    [Header("Success Chance Variables")]
    [SerializeField] [Range(10, 50)] private const int BaseReduction = 30;
    [SerializeField][Range(0, 100)] private float BaseSuccessChance = 90;
    private float SuccessChance;
    private int AdventurerBonus = 0;
    float SuccessMulitplier;

    void OnValidate()
    {
        SetValues();
    }

    public int CalculateTimeToComplete(List<Adventurer> list)
    {
        if(list.Count >0)
        { 
            CalculateAdventurerReductions(list); 
        }

        switch (Rank)
        {
            case MissionRank.S:
                TimeToCompleteInSeconds = (int)((300 * 5 * Difficulty) * (1 - AdventurerReduction));
                break;
            case MissionRank.A:
                TimeToCompleteInSeconds = (int)((300 * 4 * Difficulty) * (1- AdventurerReduction));
                break;
            case MissionRank.B:
                TimeToCompleteInSeconds = (int)((300 * 3 * Difficulty) * (1 - AdventurerReduction));
                break;
            case MissionRank.C:
                TimeToCompleteInSeconds = (int)((300 * 2 * Difficulty) * (1 - AdventurerReduction));
                break;
            case MissionRank.D:
                TimeToCompleteInSeconds = (int)((50 * 1 * Difficulty) * (1 - AdventurerReduction));
                break;
        }
        return TimeToCompleteInSeconds;
    }
    public void MissionStart(List<Adventurer> adventurers)
    {
        Active = true;
        StartTime = DateTime.Now;
        EndTime = StartTime + TimeSpan.FromSeconds(TimeToCompleteInSeconds);
        AssignedAdventurers = new List<Adventurer>(adventurers);
        CalculateSuccessChance();
        foreach (Adventurer hero in adventurers)
        {
            hero.OnMission = true;
        }
    }
    public virtual void CompleteMission()
    {
        Completed = true;
        Active = false;

        int amount = EXPValue / AssignedAdventurers.Count;
        foreach(Adventurer adventurer in AssignedAdventurers)
        {
            adventurer.AddEXP(amount);
            adventurer.OnMission = false;
            adventurer.CurrentMission = null;
        }

        GameObject GameMaster = GameObject.FindGameObjectWithTag("GameController");
        GameManager GameManager = GameMaster.GetComponent<GameManager>();
        GameManager.MissionCompleted(GoldValue, ReputationValue, this);
    }
    public void PrepareToOffer()
    {
        AssignedAdventurers.Clear();
        Offered = true;
        switch (GameState.guildRank)
        {
            case GameState.GuildRank.S:
                {
                    Rank = MissionRank.S;
                    Difficulty = UnityEngine.Random.Range(36, 60);
                }
                break;
            case GameState.GuildRank.A:
                {
                    Rank = MissionRank.A;
                    Difficulty = UnityEngine.Random.Range(24, 48);
                }
                break;
            case GameState.GuildRank.B:
                {
                    Rank = MissionRank.B;
                    Difficulty = UnityEngine.Random.Range(12, 36);
                }
                break;
            case GameState.GuildRank.C:
                {
                    Rank = MissionRank.C;
                    Difficulty = UnityEngine.Random.Range(1, 24);
                }
                break;
            case GameState.GuildRank.D:
                {
                    Rank = MissionRank.D;
                    Difficulty = UnityEngine.Random.Range(1, 12);
                }
                break;
        }
        SetValues();

    }
    public void RestartMission()
    {
        CalculateTimeToComplete(AssignedAdventurers);
        EndTime = StartTime + TimeSpan.FromSeconds(TimeToCompleteInSeconds);
        CalculateSuccessChance();
    } 
    void SetValues()
    {
        switch (Rank)
        {
            case MissionRank.S:
                EXPValue = 100 + (500 * (Difficulty / 6));
                GoldValue = 100 + (50 * (Difficulty / 6));
                ReputationValue = 50 + (5 * (Difficulty / 6));
                break;
            case MissionRank.A:
                EXPValue = 80 + (400 * (Difficulty / 6));
                GoldValue = 80 + (40 * (Difficulty / 6));
                ReputationValue = 40 + (4 * (Difficulty / 6));
                break;
            case MissionRank.B:
                EXPValue = 60 + (300 * (Difficulty / 6));
                GoldValue = 60 + (30 * (Difficulty / 6));
                ReputationValue = 30 + (3 * (Difficulty / 6));
                break;
            case MissionRank.C:
                EXPValue = 40 + (200 * (Difficulty / 6));
                GoldValue = 40 + (20 * (Difficulty / 6));
                ReputationValue = 20 + (2 * (Difficulty / 6));
                break;
            case MissionRank.D:
                EXPValue = 20 + (100 * (Difficulty / 6));
                GoldValue = 20 + (10 * (Difficulty / 6));
                ReputationValue = 10 + (1 * (Difficulty / 6));
                break;
        }
        EXPValue += UnityEngine.Random.Range(0, 30);
        GoldValue += UnityEngine.Random.Range(0, 30);
        ReputationValue += UnityEngine.Random.Range(0, 30);
    }
    void CalculateAdventurerReductions(List<Adventurer> list)
    {
        //First adventurer is required to complete mission
        //Any past the first reduces completion time
        int baseReductionPerAdventurer = BaseReduction / 2;
        AdventurerReduction = (AssignedAdventurers.Count - 1) * baseReductionPerAdventurer;

        foreach (Adventurer adventurer in list)
        {
            AdventurerReduction += adventurer.CalculateSpeedCompletionBonus(this);
        }
        AdventurerReduction /= 100;

    }
    void CalculateSuccessChance()
    {
        AdventurerBonus = 0;
        CalculateAdventurerBonus();

        SuccessChance = BaseSuccessChance;
        switch (Rank)
        {
            case MissionRank.S:
                SuccessChance = (SuccessChance * .5f) - (Difficulty / 6) + AdventurerBonus;
                break;
            case MissionRank.A:
                SuccessChance = (SuccessChance * .6f) - (Difficulty / 6) + AdventurerBonus;
                break;
            case MissionRank.B:
                SuccessChance = (SuccessChance * .75f) - (Difficulty / 6) + AdventurerBonus;
                break;
            case MissionRank.C:
                SuccessChance = (SuccessChance * .9f) - (Difficulty / 6) + AdventurerBonus;
                break;
            case MissionRank.D:
                SuccessChance = SuccessChance - (Difficulty / 6) + AdventurerBonus;
                break;
        }
        CalculateSuccessMultiplier();

        GoldValue = (int)(SuccessMulitplier * GoldValue * (1 + GameState.bonusPercent));
        ReputationValue = (int)(SuccessMulitplier * ReputationValue);
    }
    void CalculateAdventurerBonus()
    {
        foreach (Adventurer adventurer in AssignedAdventurers)
        {
            AdventurerBonus += adventurer.CalculateCompletionSuccessBonus(this);
        }

    }
    void CalculateSuccessMultiplier()
    {
        //how function was designed to work:
        //Roll d100, subtract number needed to succeed,(Better your success chance increases rewards from missions)
        //divide by 200 to keep the result between -.5 and +.5,
        //add it to one to return a functional percentage(.5(abject failure) to 1.5(absolute Success) to multiply gains against
        SuccessMulitplier = ((UnityEngine.Random.Range(0, 100) - (100 - SuccessChance)) / 200) + 1;
    }
    public void ResetData()
    {
        Active = false;
        Offered = false;
        Completed = false;
    }
}
