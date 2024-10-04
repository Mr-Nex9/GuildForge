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
    [SerializeField] public List<Mission> activeMissions = new List<Mission>();
    [SerializeField] public List<Item> inventory = new List<Item>();
    [SerializeField] public List<Item> storeInventory = new List<Item>();
    [ReadOnly]public List<Mission> missionsCompleted;

    [ReadOnly] public int gold;
    [ReadOnly] public int reputation;
    [ReadOnly] public GuildRank guildRank;
    [ReadOnly] public int currentOddJobsRate;
    [ReadOnly] public DateTime storeLastStocked = new DateTime(2024,10,2);
    [SerializeField] public TimeSpan restockTime = new TimeSpan(0, 3, 0);

    [ReadOnly] public int tutorialLevel;
    [ReadOnly] public bool sound;
    [ReadOnly] public bool music;
    [ReadOnly] public bool effects;
    [ReadOnly] public bool notifications;



}
