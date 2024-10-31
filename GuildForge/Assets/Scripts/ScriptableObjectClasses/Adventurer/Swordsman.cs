using UnityEngine;
using static Mission;

[CreateAssetMenu(fileName = "Swordsman", menuName = "Scriptable Objects/Adventurer/Swordsman")]
public class Swordsman : Adventurer
{
    private void OnEnable()
    {
        Class = AdventurerClasses.Swordsman;
        defaultWeapon = "RustedShortSword#FFFFFF/0:0:0";
        defaultArmor = "MusketeerTunic#FFFFFF/0:0:0";
        Helmet = "CaptainHelmet#FFFFFF/0:0:0";
    }
    public override int CalculateSpeedCompletionBonus(Mission mission)
    {
        switch (mission.Type)
        {
            case MissionType.Collection:
                {
                    return ActualSpeed / 10;
                }
            case MissionType.Treasure:
                {
                    return ActualSpeed / 10 + 2;
                }
            case MissionType.Hunt:
                {
                    return ActualMana / 10;
                }
            case MissionType.Target:
                {
                    return ActualAttack / 10 + 5;
                }
            case MissionType.Guard:
                {
                    return ActualMana / 10 + 5;
                }
            case MissionType.Rescue:
                {
                    return ActualAttack / 10;
                }
        }
        return 0;
    }
    public override int CalculateCompletionSuccessBonus(Mission mission)
    {
        switch (mission.Type)
        {
            case MissionType.Collection:
                {
                    return ActualMagic / 6;
                }
            case MissionType.Treasure:
                {
                    return ActualDefense / 6 + 3;
                }
            case MissionType.Hunt:
                {
                    return ActualHP / 6;
                }
            case MissionType.Target:
                {
                    return ActualHP / 6 + 3;
                }
            case MissionType.Guard:
                {
                    return ActualDefense / 6 + 1;
                }
            case MissionType.Rescue:
                {
                    return ActualMagic / 6;
                }
        }
        return 0;
    }
    public override void LevelUp()
    {
        health += 5;
        mana += 1;
        attack += 3;
        defense += 4;
        magic += 1;
        speed += 1;
        base.LevelUp();
    }
    public override void SetBaseStats()
    {
        Level = 1;
        int y = UnityEngine.Random.Range(0, 6);
        switch (y)
        {
            case 0:
                {
                    health = 10;
                    defense = 7;
                    attack = 5;
                    mana = 3;
                    speed = 3;
                    magic = 2;
                }
                break;
            case 1:
                {
                    health = 12;
                    defense = 6;
                    attack = 5;
                    mana = 4;
                    speed = 2;
                    magic = 1;
                }
                break;
            case 2:
                {
                    health = 5;
                    defense = 5;
                    attack = 5;
                    mana = 5;
                    speed = 5;
                    magic = 5;
                }
                break;
            case 3:
                {
                    health = 8;
                    defense = 7;
                    attack = 5;
                    mana = 5;
                    speed = 3;
                    magic = 2;
                }
                break;
            case 4:
                {
                    health = 9;
                    defense = 6;
                    attack = 5;
                    mana = 4;
                    speed = 4;
                    magic = 2;
                }
                break;
            case 5:
                {
                    health = 11;
                    defense = 7;
                    attack = 7;
                    mana = 2;
                    speed = 2;
                    magic = 1;
                }
                break;
        }
        base.SetBaseStats();
    }
}
