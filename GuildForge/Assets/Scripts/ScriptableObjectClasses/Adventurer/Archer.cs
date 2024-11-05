using UnityEngine;
using static Mission;


[CreateAssetMenu(fileName = "Archer", menuName = "Scriptable Objects/Adventurer/Archer")]
public class Archer : Adventurer
{
    
    //Archer Specific Bonuses
    //Speed up completion rate 
    //Best class Success Chance when on Hunts and Target Missions
    //Better Chance when on collect missions
    private void OnEnable()
    {
        Class = AdventurerClasses.Archer;
        defaultWeapon = "ShortBow#FFFFFF/0:0:0";
        Back = "LeatherQuiver#FFFFFF/0:0:0";
        defaultArmor = "Link#FFFFFF/0:0:-27";
        Helmet = "MusketeerHat [ShowEars]#FFFFFF/-52:0:-27";
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
                    return ActualSpeed / 10;
                }
            case MissionType.Hunt:
                {
                    return ActualMana / 10 + 2;
                }
            case MissionType.Target:
                {
                    return ActualAttack / 10 + 5;
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
    public override int CalculateCompletionSuccessBonus(Mission mission)
    {
        switch (mission.Type)
        {
            case MissionType.Collection:
                {
                    return ActualMagic / 6 + 3;
                }
            case MissionType.Treasure:
                {
                    return ActualDefense / 6;
                }
            case MissionType.Hunt:
                {
                    return ActualHP / 6 + 1;
                }
            case MissionType.Target:
                {
                    return ActualHP / 6 + 3;
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
    public override void LevelUp()
    {
        health += 1;
        mana += 4;
        attack += 5;
        defense += 1;
        magic += 1;
        speed += 3;
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
                    attack = 10;
                    mana = 7;
                    speed = 5;
                    health = 3;
                    magic = 3;
                    defense = 2;
                }
                break;
            case 1:
                {
                    attack = 12;
                    mana = 6;
                    speed = 5;
                    health = 4;
                    magic = 2;
                    defense = 1;
                }
                break;
            case 2:
                {
                    attack = 5;
                    mana = 5;
                    speed = 5;
                    health = 5;
                    magic = 5;
                    defense = 5;
                }
                break;
            case 3:
                {
                    attack = 8;
                    mana = 7;
                    speed = 5;
                    health = 5;
                    magic = 3;
                    defense = 2;
                }
                break;
            case 4:
                {
                    attack = 9;
                    mana = 6;
                    speed = 5;
                    health = 4;
                    magic = 4;
                    defense = 2;
                }
                break;
            case 5:
                {
                    attack = 11;
                    mana = 7;
                    speed = 7;
                    health = 2;
                    magic = 2;
                    defense = 1;
                }
                break;
        }
        base.SetBaseStats();
    }
}
