using UnityEngine;


[CreateAssetMenu(fileName = "Sorcerer", menuName = "Scriptable Objects/Adventurer/Sorcerer")]
public class Sorcerer : Adventurer
{
    private void OnEnable()
    {
        Class = AdventurerClasses.Sorcerer;
    }
    public override int CalculateCompletionBonus()
    {
        return 0;
    }
}
