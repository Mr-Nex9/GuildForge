using JetBrains.Annotations;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "GameState", menuName = "Scriptable Objects/GameState")]
public class GameState : ScriptableObject
{
    public enum GuildRank { S, A, B, C, D };

    [Header("Adventurer Lists")]
    public List<Adventurer> roster = new List<Adventurer>();
    public List<Adventurer> localAdventurers = new List<Adventurer>();
    public List<Adventurer> firstTimeAdventurers = new List<Adventurer>();

    [Header("Mission Lists")]
    public List<Mission> offeredMissions = new List<Mission>();
    public List<Mission> activeMissions = new List<Mission>();
    public List<Mission> missionsCompleted = new List<Mission>();

    [Header("Item Lists")]
    public List<Item> inventory = new List<Item>();
    public List<Item> storeInventory = new List<Item>();

    [Header("Quest Lists")]
    public List<GoldQuest> goldQuests = new List<GoldQuest>();
    public List<MissionQuest> missionQuests = new List<MissionQuest>();
    public List<AdventurerQuest> adventurerQuests = new List<AdventurerQuest>();

    [Header("Lists of All")]
    public List<Adventurer> allAdventurers = new List<Adventurer>();
    public List<Mission> allMissions = new List<Mission>();
    public List<Item> allItems = new List<Item>();
    public List<Quest> allQuests = new List<Quest>();

    [Header("Details")]
    public int gold;
    public int totalGold;
    public int reputation;
    public static float bonusPercent;
    public static GuildRank guildRank;
    public int currentOddJobsRate;
    public DateTime storeLastStocked = new DateTime(2024,10,2);
    public TimeSpan restockTime = new TimeSpan(24, 0, 0);
    public DateTime lastOpened = new DateTime(2000, 10, 2,0, 0, 0);
    public bool GameLoaded = false;

    [Header ("Settings")]
    [ReadOnly] public int tutorialLevel;
    [ReadOnly] public bool sound;
    [ReadOnly] public bool music;
    [ReadOnly] public bool effects;
    [ReadOnly] public bool notifications;
    [ReadOnly] public bool password = false;
    [ReadOnly] public bool firstTime;
    [ReadOnly] public string passwordValue;


}
