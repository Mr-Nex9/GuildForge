using UnityEngine;
using static Adventurer;


[CreateAssetMenu(fileName = "Hunt", menuName = "Scriptable Objects/Mission/Hunt")]

public class Hunt : Mission
{
    private void OnEnable()
    {
        Type = MissionType.Hunt;
    }
}
