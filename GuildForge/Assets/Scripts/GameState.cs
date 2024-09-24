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
    [SerializeField] public List<Adventurer> Roster = new List<Adventurer>();
    [SerializeField] public List<Mission> ActiveMissions = new List<Mission>();
    [SerializeField] public List<Item> Inventory = new List<Item>();

    [ReadOnly] public int gold;
    [ReadOnly] public int reputation;
    [ReadOnly] public GuildRank guildRank;
    [ReadOnly] public int currentOddJobsRate;
    [ReadOnly] public bool missionsCompleted;





}
