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
    [SerializeField] public Texture Icon;
    [SerializeField] public string Name;
    [ReadOnly] public AdventurerClasses Class;
    [ReadOnly] public AdventurerRanks Rank;
 
    [SerializeField] public int Level;
    [SerializeField] public bool OnMission;
    public string CurrentMission;
    [SerializeField] private int EXP;
    public bool ReadyToLevel;


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
