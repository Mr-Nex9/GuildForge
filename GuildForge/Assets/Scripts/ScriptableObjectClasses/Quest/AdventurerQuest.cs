using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AdventurerQuest", menuName = "Scriptable Objects/Quest/AdventurerQuest")]
public class AdventurerQuest : Quest
{
    public Adventurer.AdventurerClasses TypeTracked;
    public List<Adventurer> AdventurersTracked;
    public bool isTotal;

    public void CheckForCompletion(Adventurer adventurer)
    {
        if (adventurer.Class == TypeTracked && !AdventurersTracked.Contains(adventurer) && completed == false|| isTotal && !AdventurersTracked.Contains(adventurer))
        {
            AdventurersTracked.Add(adventurer);
        }

        if (AdventurersTracked.Count > requiredForCompletion)
        {
            completed = true;
        }

    }
}
