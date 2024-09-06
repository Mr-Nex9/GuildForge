using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Scriptable Objects/Quest")]
public class Mission : ScriptableObject
{
    public enum MissionType {Collection, Treasure, Hunt, Target, Guard, Rescue};

    [SerializeField] static int DefaultCompletionTime;

    [SerializeField] public string Name;
    [SerializeField] public string Description;
    [SerializeField] public MissionType missionType;
    [SerializeField] public int Difficulty;

    [SerializeField] public int MaxAssigned;
    [SerializeField] public List<Adventurer> AssignedAdventurers;


    [SerializeField] public int CompletionPercent;
    private int TimeToCompleteInSeconds;



    void CalculateTimeToComplete()
    {

    }

    
}
