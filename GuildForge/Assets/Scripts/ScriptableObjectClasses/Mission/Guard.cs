using UnityEngine;
using static Adventurer;


[CreateAssetMenu(fileName = "Guard", menuName = "Scriptable Objects/Mission/Guard")]
public class Guard : Mission
{
    private void OnEnable()
    {
        Type = MissionType.Guard;
    }
}
