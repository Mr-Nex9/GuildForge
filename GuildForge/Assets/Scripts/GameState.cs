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
    [SerializeField] public List<Adventurer> roster = new List<Adventurer>();
    [SerializeField] public List<Adventurer> localAdventurers = new List<Adventurer>();
    [SerializeField] public List<Mission> activeMissions = new List<Mission>();
    [SerializeField] public List<Item> inventory = new List<Item>();
    [SerializeField] public List<Item> storeInventory = new List<Item>();
    [ReadOnly]public List<Mission> missionsCompleted;

    public List<Adventurer> allAdventurers = new List<Adventurer>();
    public List<Mission> allMissions = new List<Mission>();
    public List<Item> allItems = new List<Item>();

    [ReadOnly] public int gold;
    [ReadOnly] public int totalGold;
    [ReadOnly] public int reputation;
    [ReadOnly] public static float bonusPercent;
    [ReadOnly] public static GuildRank guildRank;
    [ReadOnly] public int currentOddJobsRate;
    [ReadOnly] public DateTime storeLastStocked = new DateTime(2024,10,2);
    [SerializeField] public TimeSpan restockTime = new TimeSpan(0, 3, 0);

    [Header ("Settings")]
    [ReadOnly] public int tutorialLevel;
    [ReadOnly] public bool sound;
    [ReadOnly] public bool music;
    [ReadOnly] public bool effects;
    [ReadOnly] public bool notifications;



}
