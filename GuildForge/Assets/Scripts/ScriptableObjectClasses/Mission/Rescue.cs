using UnityEngine;
using static Adventurer;


[CreateAssetMenu(fileName = "Rescue", menuName = "Scriptable Objects/Mission/Rescue")]
public class Rescue : Mission
{
    private void OnEnable()
    {
        Type = MissionType.Rescue;
    }
}
