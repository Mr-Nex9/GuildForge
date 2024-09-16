using UnityEngine;


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
    public override int CalculateCompletionBonus()
    {
        return 0;
    }
}
