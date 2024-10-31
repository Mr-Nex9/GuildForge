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

    [Header("Permanent Info")]
    public int ID;
    public Texture defaultIcon;
    [SerializeField] public Texture Icon;
    [SerializeField] public string Name;
    public AdventurerClasses Class;
    public AdventurerRanks Rank;

    [Header("Details")]
    [SerializeField] public int Level;
    [SerializeField] public bool OnMission;
    public bool offered;
    public bool Recruited;
    public string CurrentMission;
    public int currentMissionID;
    [SerializeField] public int EXP;
    public bool ReadyToLevel;
    public int CostToRecruit;


    [Header("Stats")]
    [Range(0, 100)]
    [SerializeField] public int health;
    [Range(0, 100)]
    [SerializeField] public int mana;
    [Range(0, 100)]
    [SerializeField] public int attack;
    [Range(0, 100)]
    [SerializeField] public int defense;
    [Range(0, 100)]
    [SerializeField] public int magic;
    [Range(0, 100)]
    [SerializeField] public int speed;

    public int ActualHP;
    public int ActualMana;
    public int ActualAttack;
    public int ActualDefense;
    public int ActualMagic;
    public int ActualSpeed;


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

    public string defaultWeapon;
    public string defaultArmor;

    private void OnValidate()
    {
        Loaded();

    }
    public void Loaded()
    {
        CheckForLevelUpStatus();
        CalculateRank();
        RecalulateActualStats();
        if(WeaponItem != null)
        {
            Weapon = WeaponItem.characterBuilderName;
        }
        else
        {
            Weapon = defaultWeapon;
        }
        if (ArmorItem != null)
        {
            Armor = ArmorItem.characterBuilderName;
        }
        else
        {
            Armor = defaultArmor;
        }
    }
    void CheckForLevelUpStatus()
    {
        float x = (float)EXP / (125 + (25*Level)) / 1.1912f;
        int NewLevel = (int)x;

        if (Level < NewLevel)
        {
            ReadyToLevel = true;
        }
    }

    public void AddEXP(int amount)
    {
        EXP += amount;
        CheckForLevelUpStatus();
    }
    void CalculateRank()
    {
        int x = health + mana + attack + defense + magic + speed;
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
        if (Recruited)
        {
            GameObject GameMaster = GameObject.FindGameObjectWithTag("GameController");
            GameManager GameManager = GameMaster.GetComponent<GameManager>();
            GameManager.GainReputation(10);
            OnValidate();
            GameManager.UpdateAdventurer(this);

        }
    }

    public virtual void SetBaseStats()
    {
        EXP = 0;
        Loaded();
    }
    public void PrepareForRecruitment()
    {
        SetBaseStats();
        int x = 0;
        switch(GameState.guildRank)
        {
            case GameState.GuildRank.S:
                {
                    x = UnityEngine.Random.Range(25, 31);
                }
                break;
            case GameState.GuildRank.A:
                {
                    x = UnityEngine.Random.Range(22, 27);
                }
                break;
            case GameState.GuildRank.B:
                {
                    x = UnityEngine.Random.Range(14, 19);
                }
                break;
            case GameState.GuildRank.C:
                {
                    x = UnityEngine.Random.Range(6, 11);
                }
                break;
            case GameState.GuildRank.D:
                {
                    x = UnityEngine.Random.Range(1, 4);
                    
                }
                break;
        }

        CostToRecruit = x * 300 + UnityEngine.Random.Range(11, 100);

        for (int i = 0; i < x; i++)
        {
            LevelUp();
        }
        EXP = (int)(x * (125 + (25 * (x - 1))) * 1.1912f);
        offered = true;
        OnValidate();

        GameObject GameMaster = GameObject.FindGameObjectWithTag("GameController");
        GameManager GameManager = GameMaster.GetComponent<GameManager>();
        GameManager.UpdateAdventurer(this);
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

    public virtual int CalculateRate()
    {
        return ActualSpeed/ 10;
    }
    public void RecalulateActualStats()
    {
        ActualHP = health;
        ActualMana = mana;
        ActualAttack = attack;
        ActualDefense = defense;
        ActualMagic = magic;
        ActualSpeed = speed;

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

    public void ResetData()
    {
        Level = 1;
        EXP = 0;
        CostToRecruit = 0;

        OnMission = false;
        offered = false;
        Recruited = false;
        currentMissionID = 0;

        WeaponItem = null;
        ArmorItem = null;
        AccessorySlot1 = null;
        AccessorySlot2 = null;

    }
}
