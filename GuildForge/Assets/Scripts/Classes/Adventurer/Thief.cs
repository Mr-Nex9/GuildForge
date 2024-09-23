using UnityEngine;
using static Mission;

[CreateAssetMenu(fileName = "Thief", menuName = "Scriptable Objects/Adventurer/Thief")]
public class Thief : Adventurer
{
    private void OnEnable()
    {
        Class = AdventurerClasses.Thief;
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

    public override void LevelUp()
    {
        HP += 1;
        Mana += 1;
        Attack += 1;
        Defense += 4;
        Magic += 3;
        Speed += 5;
        base.LevelUp();
    }
}
