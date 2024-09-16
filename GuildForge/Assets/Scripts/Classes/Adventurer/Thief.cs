using UnityEngine;

[CreateAssetMenu(fileName = "Thief", menuName = "Scriptable Objects/Adventurer/Thief")]
public class Thief : Adventurer
{
    private void OnEnable()
    {
        Class = AdventurerClasses.Thief;
    }
    public override int CalculateCompletionBonus()
    {
        return 0;
    }
}
