using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.Mathematics;
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
    [SerializeField] public string Name;
    [SerializeField] public string Description;
    [ReadOnly] public MissionType Type;
    [SerializeField][Range(1, 60)] public int Difficulty;
    [SerializeField] public MissionRank Rank;
    [SerializeField] private int BaseEXPValue;
    public int EXPValue;
    [SerializeField] private int BaseGoldValue;
    [SerializeField] private int BaseReputationValue;
    [ReadOnly] public int GoldValue;
    [ReadOnly] public int ReputationValue;



    [Header("Adventurer Details")]
    [SerializeField] public List<Adventurer> AssignedAdventurers = new List<Adventurer>();

    [Header("Completion Time Variables")]
    [ReadOnly] public float CompletionPercent
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
    [SerializeField] int BaseCompletionTimeInSeconds = 300;        //Default time to complete is 5 minutes  
    private float AdventurerReduction = 0;

    [Header("Success Chance Variables")]
    [SerializeField] [Range(10, 50)] private const int BaseReduction = 30;
    [SerializeField][Range(0, 100)] private float BaseSuccessChance = 90;
    private float SuccessChance;
    private int AdventurerBonus = 0;
    int SuccessMulitplier;

    private void OnValidate()
    {
        SetValues();
    }
    void SetValues()
    {
        switch (Rank)
        {
            case MissionRank.S:
                EXPValue = BaseEXPValue + (500 * (Difficulty / 6));
                GoldValue = BaseGoldValue + (50 * (Difficulty / 6));
                ReputationValue = BaseReputationValue + (5 * (Difficulty / 6));
                break;
            case MissionRank.A:
                EXPValue = BaseEXPValue + (400 * (Difficulty / 6));
                GoldValue = BaseGoldValue + (40 * (Difficulty / 6));
                ReputationValue = BaseReputationValue + (5 * (Difficulty / 6));
                break;
            case MissionRank.B:
                EXPValue = BaseEXPValue + (300 * (Difficulty / 6));
                GoldValue = BaseGoldValue + (30 * (Difficulty / 6));
                ReputationValue = BaseReputationValue + (5 * (Difficulty / 6));
                break;
            case MissionRank.C:
                EXPValue = BaseEXPValue + (200 * (Difficulty / 6));
                GoldValue = BaseGoldValue + (20 * (Difficulty / 6));
                ReputationValue = BaseReputationValue + (5 * (Difficulty / 6));
                break;
            case MissionRank.D:
                EXPValue = BaseEXPValue + (100 * (Difficulty / 6));
                GoldValue = BaseGoldValue + (10 * (Difficulty / 6));
                ReputationValue = BaseReputationValue + (5 * (Difficulty / 6));
                break;
        }
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
                TimeToCompleteInSeconds = (int)((BaseCompletionTimeInSeconds * 5 * Difficulty) * (1 - AdventurerReduction));
                break;
            case MissionRank.A:
                TimeToCompleteInSeconds = (int)((BaseCompletionTimeInSeconds * 4 * Difficulty) * (1- AdventurerReduction));
                break;
            case MissionRank.B:
                TimeToCompleteInSeconds = (int)((BaseCompletionTimeInSeconds * 3 * Difficulty) * (1 - AdventurerReduction));
                break;
            case MissionRank.C:
                TimeToCompleteInSeconds = (int)((BaseCompletionTimeInSeconds * 2 * Difficulty) * (1 - AdventurerReduction));
                break;
            case MissionRank.D:
                TimeToCompleteInSeconds = (int)((BaseCompletionTimeInSeconds * 1 * Difficulty) * (1 - AdventurerReduction));
                break;
        }
        return TimeToCompleteInSeconds;
    }

    void CalculateAdventurerReductions(List<Adventurer> list)
    {
        //First adventurer is required to complete mission
        //Any past the first reduces completion time
        int baseReductionPerAdventurer = BaseReduction / 2;
        AdventurerReduction = (AssignedAdventurers.Count -1) * baseReductionPerAdventurer;

        foreach (Adventurer adventurer in list)
        {
            AdventurerReduction += adventurer.CalculateSpeedCompletionBonus(this);
        }
        AdventurerReduction /= 100;
        
    }

    void CalculateSuccessChance()
    {
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
    }

    void CalculateAdventurerBonus()
    {
        foreach (Adventurer adventurer in AssignedAdventurers)
        {
            AdventurerBonus += adventurer.CalculateCompletionSuccessBonus(this);
        }

    }
    public virtual void CompleteMission()
    {
        GameObject GameMaster = GameObject.FindGameObjectWithTag("GameController");
        GameManager GameManager = GameMaster.GetComponent<GameManager>();

        GameManager.MissionCompleted(GoldValue * SuccessMulitplier, ReputationValue * SuccessMulitplier);
    }

    public int CalculateSuccessMultiplier()
    {
        SuccessMulitplier = (int)((SuccessChance - UnityEngine.Random.Range(0, 100)) / 200);
        return SuccessMulitplier;
    }

    public void CheckForCompletion()
    {
        if((DateTime.Now - StartTime).TotalSeconds >= TimeToCompleteInSeconds)
        {

        }
    }
}
