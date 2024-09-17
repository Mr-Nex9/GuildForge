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
    [SerializeField] public List<Adventurer> Roster = new List<Adventurer>();
    [SerializeField] public List<Adventurer> AllAdventurers = new List<Adventurer>();
    [SerializeField] public List<Mission> AllMissions = new List<Mission>();
    [SerializeField] public List<Mission> ActiveMissions = new List<Mission>();

    [SerializeField] public int Gold;
    [SerializeField] public int Loot;
    [SerializeField] public int Reputation;


    

    public void AddGold()
    { 
        Gold = Gold + 1;
    }


    void MissionCompleted(int gold, int loot, int reputation)
    {
        Gold += gold;
        Loot += loot;
        Reputation += reputation;
    }
}
