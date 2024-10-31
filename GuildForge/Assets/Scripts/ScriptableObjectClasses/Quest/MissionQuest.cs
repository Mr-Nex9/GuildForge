using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MissionQuest", menuName = "Scriptable Objects/Quest/MissionQuest")]
public class MissionQuest : Quest
{
    public Mission.MissionType TypeTracked;
    public List<Mission> MissionsTracked;
    public bool isTotal;

    public void CheckForCompletion(Mission mission)
    {
        if (mission.Type == TypeTracked && !MissionsTracked.Contains(mission) && completed == false || isTotal && !MissionsTracked.Contains(mission))
        {
            MissionsTracked.Add(mission);
        }

        if(MissionsTracked.Count > requiredForCompletion)
        {
            completed = true;
        }

    }
}
