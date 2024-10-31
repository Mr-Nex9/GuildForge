using UnityEngine;
using static Adventurer;


[CreateAssetMenu(fileName = "Collection", menuName = "Scriptable Objects/Mission/Collection")]
public class Collection : Mission
{
    private void OnEnable()
    {
        Type = MissionType.Collection;
    }
}
