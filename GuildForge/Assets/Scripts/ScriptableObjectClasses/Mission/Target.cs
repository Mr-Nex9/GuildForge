using UnityEngine;
using static Adventurer;


[CreateAssetMenu(fileName = "Target", menuName = "Scriptable Objects/Mission/Target")]

internal class Target : Mission
{
    private void OnEnable()
    {
        Type = MissionType.Target;
    }
}