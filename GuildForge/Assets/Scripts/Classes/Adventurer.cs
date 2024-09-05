using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Adventurer", menuName = "Scriptable Objects/Adventurer")]
public class Adventurer : ScriptableObject
{
    enum AdventurerClasses { Archer, Swordsman, Sorcerer, Thief}
    public enum AdventurerRanks {S, A, B, C, D}
    [SerializeField] public string Name;
    private int ID;
    [SerializeField] private AdventurerClasses Class;
    public AdventurerRanks Rank
    {
        get
        {
            int x = HP + Mana + Attack + Defense + Magic + Speed;
            x = x / 120;

            switch (x)
            {
                case 0:
                    return AdventurerRanks.D;
                case 1:
                    return AdventurerRanks.C;
                case 2:
                    return AdventurerRanks.B;
                case 3:
                    return AdventurerRanks.A;
                case 4:
                    return AdventurerRanks.S;
                case 5:
                    return AdventurerRanks.S;
            }

            return AdventurerRanks.D;
        }
    }
    [SerializeField] private int Level;
    public int EXP
    {
        get { return EXP; }
        set
        {
            EXP = value;
            OnEXPChange();
        }
    }

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



    [SerializeField] private List<Skill> Skills = new List<Skill>();
    [SerializeField] private bool OnMission;

    [Header("Equipment")]
    [SerializeField] public Armor Armor;
    [SerializeField] public Weapon Weapon;
    [SerializeField] public Accessory AccessorySlot1;
    [SerializeField] public Accessory AccessorySlot2;



    void OnEXPChange()
    {
        float x = Mathf.Log(EXP / 100 + 2.5f) / Mathf.Log(2.5f);
        int NewLevel = (int)x;

        if (Level < NewLevel)
        {
            LevelUp(NewLevel);
        }
    }

    void LevelUp(int newLevel)
    {
        Level = newLevel;
    }
}
