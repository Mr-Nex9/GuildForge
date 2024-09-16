using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Adventurer", menuName = "Scriptable Objects/Adventurer")]
public class Adventurer : ScriptableObject
{
    public enum AdventurerRanks {S, A, B, C, D}
    public enum AdventurerClasses { Archer, Swordsman, Sorcerer, Thief }


    private int ID;
    [SerializeField] public Sprite Icon;
    [SerializeField] public string Name;
    [ReadOnly] public AdventurerClasses Class;
    [ReadOnly] public AdventurerRanks Rank;
 
    [SerializeField] public int Level;
    [SerializeField] private bool OnMission;
    [SerializeField] private int EXP;


    [Header("Stats")]
    [Range(0, 100)]
    [SerializeField] private int HP;
    [Range(0, 100)]
    [SerializeField] private int Mana;
    [Range(0, 100)]
    [SerializeField] private int Attack;
    [Range(0, 100)]
    [SerializeField] private int Defense;
    [Range(0, 100)]
    [SerializeField] private int Magic;
    [Range(0, 100)]
    [SerializeField] private int Speed;


    [Header("Equipment")]
    [SerializeField] public Armor Armor;
    [SerializeField] public Weapon Weapon;
    [SerializeField] public Accessory AccessorySlot1;
    [SerializeField] public Accessory AccessorySlot2;
    [SerializeField] private List<Skill> Skills = new List<Skill>();


    private void OnValidate()
    {
        OnEXPChange();
        CalculateRank();
    }
    void OnEXPChange()
    {
        float x = Mathf.Log(EXP / 100 + 2.5f) / Mathf.Log(2.5f);
        int NewLevel = (int)x;

        if (Level < NewLevel)
        {
            LevelUp(NewLevel);
        }
    }

    void CalculateRank()
    {
        int x = HP + Mana + Attack + Defense + Magic + Speed;
        x = x / 120;
        switch (x)
        {
            case 0:
                Rank = AdventurerRanks.D;
                break;
            case 1:
                Rank = AdventurerRanks.C;
                break;
            case 2:
                Rank = AdventurerRanks.B;
                break;
            case 3:
                Rank = AdventurerRanks.A;
                break;
            case 4:
                Rank = AdventurerRanks.S;
                break;
            case 5:
                Rank = AdventurerRanks.S;
                break;
        }

    }

    protected virtual void LevelUp(int newLevel)
    {
        Level = newLevel;
    }

    public virtual int CalculateCompletionBonus()
    {
        return 0;
    }
    public virtual int CalculateCompletionSuccessBonus()
    {
        return 10;
    }

    public virtual int CalculateLootBonus()
    {
        return 0; 
    }

}
