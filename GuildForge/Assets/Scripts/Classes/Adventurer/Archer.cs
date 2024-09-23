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
        HP += 1;
        Mana += 4;
        Attack += 5;
        Defense += 1;
        Magic += 1;
        Speed += 3;
        base.LevelUp();
    }
}
