using UnityEngine;
using static Mission;

[CreateAssetMenu(fileName = "Swordsman", menuName = "Scriptable Objects/Adventurer/Swordsman")]
public class Swordsman : Adventurer
{
    private void OnEnable()
    {
        Class = AdventurerClasses.Swordsman;
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
        HP += 5;
        Mana += 1;
        Attack += 3;
        Defense += 4;
        Magic += 1;
        Speed += 1;
        base.LevelUp();
    }
}
