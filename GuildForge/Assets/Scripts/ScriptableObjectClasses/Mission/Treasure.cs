using UnityEngine;
using static Adventurer;


[CreateAssetMenu(fileName = "Treasure", menuName = "Scriptable Objects/Mission/Treasure")]

internal class Treasure : Mission
{
    private void OnEnable()
    {
        Type = MissionType.Treasure;
    }
}

