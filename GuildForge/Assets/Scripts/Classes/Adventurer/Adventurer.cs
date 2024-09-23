using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEditor;
using UnityEngine;
using static Mission;

public class Adventurer : ScriptableObject
{
    public enum AdventurerRanks {S, A, B, C, D}
    public enum AdventurerClasses { Archer, Swordsman, Sorcerer, Thief }


    private int ID;
    [SerializeField] public Texture Icon;
    [SerializeField] public string Name;
    [ReadOnly] public AdventurerClasses Class;
    [ReadOnly] public AdventurerRanks Rank;
 
    [SerializeField] public int Level;
    [SerializeField] public bool OnMission;
    public bool Recruited;
    public string CurrentMission;
    [SerializeField] private int EXP;
    public bool ReadyToLevel;
    public int CostToRecruit;


    [Header("Stats")]
    [Range(0, 100)]
    [SerializeField] public int HP;
    [Range(0, 100)]
    [SerializeField] public int Mana;
    [Range(0, 100)]
    [SerializeField] public int Attack;
    [Range(0, 100)]
    [SerializeField] public int Defense;
    [Range(0, 100)]
    [SerializeField] public int Magic;
    [Range(0, 100)]
    [SerializeField] public int Speed;

    [ReadOnly] public int ActualHP;
    [ReadOnly] public int ActualMana;
    [ReadOnly] public int ActualAttack;
    [ReadOnly] public int ActualDefense;
    [ReadOnly] public int ActualMagic;
    [ReadOnly] public int ActualSpeed;


    [Header("Equipment")]
    [SerializeField] public Armor ArmorItem;
    [SerializeField] public Weapon WeaponItem;
    [SerializeField] public Accessory AccessorySlot1;
    [SerializeField] public Accessory AccessorySlot2;
    [SerializeField] private List<Skill> Skills = new List<Skill>();

    [Header("Character Builder Info")]
    public string Head = "Human";
    public string Ears = "Human";
    public string Eyes = "Human";
    public string Body = "Human";
    public string Hair;
    public string Armor;
    public string Helmet;
    public string Weapon;
    public string Firearm;
    public string Shield;
    public string Cape;
    public string Back;
    public string Mask;
    public string Horns;

    private void OnValidate()
    {
        OnEXPChange();
        CalculateRank();
        RecalulateActualStats();
    }
    void OnEXPChange()
    {
        float x = Mathf.Log(EXP / 100 + 2.5f) / Mathf.Log(2.5f);
        int NewLevel = (int)x;

        if (Level < NewLevel)
        {
            ReadyToLevel = true;
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

    public virtual void LevelUp()
    {

        Level += 1;
        ReadyToLevel = false;
        OnValidate();
    }

    public virtual int CalculateSpeedCompletionBonus(Mission mission)
    {
        switch(mission.Type)
        {
            case MissionType.Collection:
                {
                    return ActualSpeed / 10;
                }
            case MissionType.Treasure:
                {
                    return ActualSpeed / 10;
                }
            case MissionType.Hunt:
                {
                    return ActualMana / 10;
                }
            case MissionType.Target:
                {
                    return ActualAttack / 10;
                }
            case MissionType.Guard:
                {
                    return ActualMana / 10;
                }
            case MissionType.Rescue:
                {
                    return ActualAttack / 10;
                }
        }
        return 0;

    }
    public virtual int CalculateCompletionSuccessBonus(Mission mission)
    {
        switch (mission.Type)
        {
            case MissionType.Collection:
                {
                    return ActualMagic / 6;
                }
            case MissionType.Treasure:
                {
                    return ActualDefense / 6;
                }
            case MissionType.Hunt:
                {
                    return ActualHP / 6;
                }
            case MissionType.Target:
                {
                    return ActualHP / 6;
                }
            case MissionType.Guard:
                {
                    return ActualDefense / 6;
                }
            case MissionType.Rescue:
                {
                    return ActualMagic / 6;
                }
        }
        return 0;
    }

    public void RecalulateActualStats()
    {
        ActualHP = HP;
        ActualMana = Mana;
        ActualAttack = Attack;
        ActualDefense = Defense;
        ActualMagic = Magic;
        ActualSpeed = Speed;

        if (ArmorItem != null)
        {
            ActualHP += ArmorItem.HPBonus;
            ActualMana += ArmorItem.ManaBonus;
            ActualAttack += ArmorItem.AtkBonus;
            ActualDefense += ArmorItem.DefBonus;
            ActualMagic += ArmorItem.MagBonus;
            ActualSpeed += ArmorItem.SpdBonus;
        }
        if (WeaponItem != null)
        {
            ActualHP += WeaponItem.HPBonus;
            ActualMana += WeaponItem.ManaBonus;
            ActualAttack += WeaponItem.AtkBonus;
            ActualDefense += WeaponItem.DefBonus;
            ActualMagic += WeaponItem.MagBonus;
            ActualSpeed += WeaponItem.SpdBonus;
        }
        if (AccessorySlot1 != null)
        {
            ActualHP += AccessorySlot1.HPBonus;
            ActualMana += AccessorySlot1.ManaBonus;
            ActualAttack += AccessorySlot1.AtkBonus;
            ActualDefense += AccessorySlot1.DefBonus;
            ActualMagic += AccessorySlot1.MagBonus;
            ActualSpeed += AccessorySlot1.SpdBonus;
        }
        if (AccessorySlot2 != null)
        {
            ActualHP += AccessorySlot2.HPBonus;
            ActualMana += AccessorySlot2.ManaBonus;
            ActualAttack += AccessorySlot2.AtkBonus;
            ActualDefense += AccessorySlot2.DefBonus;
            ActualMagic += AccessorySlot2.MagBonus;
            ActualSpeed += AccessorySlot2.SpdBonus;
        }
    }
}
