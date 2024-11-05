using UnityEngine;
using static Mission;


[CreateAssetMenu(fileName = "Sorcerer", menuName = "Scriptable Objects/Adventurer/Sorcerer")]
public class Sorcerer : Adventurer
{
    private void OnEnable()
    {
        Class = AdventurerClasses.Sorcerer;
        defaultWeapon = "Stick#FFFFFF/0:0:0";
        defaultArmor = "BlueWizardTunic#FFFFFF/-160:6:0";
        Helmet = "BlueWizzardHat#FFFFFF/0:0:0";
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
                    return ActualSpeed / 10;
                }
            case MissionType.Hunt:
                {
                    return ActualMana / 10;
                }
            case MissionType.Target:
                {
                    return ActualAttack / 10 + 2;
                }
            case MissionType.Guard:
                {
                    return ActualMana / 10 + 5;
                }
            case MissionType.Rescue:
                {
                    return ActualAttack / 10 + 5;
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
                    return ActualDefense / 6;
                }
            case MissionType.Hunt:
                {
                    return ActualHP / 6;
                }
            case MissionType.Target:
                {
                    return ActualHP / 6 + 1;
                }
            case MissionType.Guard:
                {
                    return ActualDefense / 6 + 3;
                }
            case MissionType.Rescue:
                {
                    return ActualMagic / 6 + 3;
                }
        }
        return 0;
    }
    public override void LevelUp()
    {
        health += 1;
        mana += 4;
        attack += 3;
        defense += 1;
        magic += 5;
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
                    magic= 10;
                    mana = 7;
                    attack = 5;
                    health = 3;
                    speed = 3;
                    defense = 2;
                }
                break;
            case 1:
                {
                    magic = 12;
                    mana = 6;
                    attack = 5;
                    health = 4;
                    speed = 2;
                    defense = 1;
                }
                break;
            case 2:
                {
                    magic = 5;
                    mana = 5;
                    attack = 5;
                    health = 5;
                    speed = 5;
                    defense = 5;
                }
                break;
            case 3:
                {
                    magic = 8;
                    mana = 7;
                    attack = 5;
                    health = 5;
                    speed = 3;
                    defense = 2;
                }
                break;
            case 4:
                {
                    magic = 9;
                    mana = 6;
                    attack = 5;
                    health = 4;
                    speed = 4;
                    defense = 2;
                }
                break;
            case 5:
                {
                    magic = 11;
                    mana = 7;
                    attack = 7;
                    health = 2;
                    speed = 2;
                    defense = 1;
                }
                break;
        }
        base.SetBaseStats();
    }
}
