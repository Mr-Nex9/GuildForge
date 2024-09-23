using UnityEngine;
using static Mission;


[CreateAssetMenu(fileName = "Sorcerer", menuName = "Scriptable Objects/Adventurer/Sorcerer")]
public class Sorcerer : Adventurer
{
    private void OnEnable()
    {
        Class = AdventurerClasses.Sorcerer;
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
        HP += 1;
        Mana += 4;
        Attack += 3;
        Defense += 1;
        Magic += 5;
        Speed += 1;
        base.LevelUp();
    }
}
