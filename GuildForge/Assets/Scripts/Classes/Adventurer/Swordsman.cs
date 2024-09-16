using UnityEngine;

[CreateAssetMenu(fileName = "Swordsman", menuName = "Scriptable Objects/Adventurer/Swordsman")]
public class Swordsman : Adventurer
{
    private void OnEnable()
    {
        Class = AdventurerClasses.Swordsman;
    }
    public override int CalculateCompletionBonus()
    {
        return 0;
    }
}
