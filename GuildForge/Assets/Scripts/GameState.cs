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
    [SerializeField] private List<Adventurer> Roster = new List<Adventurer>();
    [SerializeField] private List<Adventurer> AllAdventurers = new List<Adventurer>();
    [SerializeField] private int Gold;
    [SerializeField] private int Loot;
    [SerializeField] private int Reputation;

}
