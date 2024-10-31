using UnityEngine;
using static Mission;

[CreateAssetMenu(fileName = "Thief", menuName = "Scriptable Objects/Adventurer/Thief")]
public class Thief : Adventurer
{
    private void OnEnable()
    {
        Class = AdventurerClasses.Thief;
        defaultWeapon = "ShortDagger#FFFFFF/0:0:0";
        defaultArmor = "TravelerTunic#FFFFFF/0:-66:-36";
        Helmet = "BanditPatch [ShowEars]#FFFFFF/-52:-100:6";
    }
    public override int CalculateSpeedCompletionBonus(Mission mission)
    {
        switch (mission.Type)
        {
            case MissionType.Collection:
                {
                    return ActualSpeed / 10 + 5;
                }
            case MissionType.Treasure:
                {
                    return ActualSpeed / 10 + 5;
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
                    return ActualAttack / 10 +2;
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
                    return ActualMagic / 6 +3;
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
                    return ActualHP / 6;
                }
            case MissionType.Guard:
                {
                    return ActualDefense / 6;
                }
            case MissionType.Rescue:
                {
                    return ActualMagic / 6 + 1;
                }
        }
        return 0;
    }
    public override int CalculateRate()
    {
        return ActualSpeed / 8;
    }
    public override void LevelUp()
    {
        health += 1;
        mana += 1;
        attack += 1;
        defense += 4;
        magic += 3;
        speed += 5;
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
                    speed = 10;
                    defense = 7;
                    magic = 5;
                    attack = 3;
                    mana = 3;
                    health = 2;
                }
                break;
            case 1:
                {
                    speed = 12;
                    defense = 6;
                    magic = 5;
                    attack = 4;
                    mana = 2;
                    health = 1;
                }
                break;
            case 2:
                {
                    speed = 5;
                    defense = 5;
                    magic = 5;
                    attack = 5;
                    mana = 5;
                    health = 5;
                }
                break;
            case 3:
                {
                    speed = 8;
                    defense = 7;
                    magic = 5;
                    attack = 5;
                    mana = 3;
                    health = 2;
                }
                break;
            case 4:
                {
                    speed = 9;
                    defense = 6;
                    magic = 5;
                    attack = 4;
                    mana = 4;
                    health = 2;
                }
                break;
            case 5:
                {
                    speed = 11;
                    defense = 7;
                    magic = 7;
                    attack = 2;
                    mana = 2;
                    health = 1;
                }
                break;
        }
        base.SetBaseStats();
    }
}
