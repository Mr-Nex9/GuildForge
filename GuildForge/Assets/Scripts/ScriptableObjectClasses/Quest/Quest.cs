using UnityEngine;


public class Quest : ScriptableObject
{
    public int id;
    public string Name;
    public int currentProgress;
    public int requiredForCompletion;
    public bool completed;

    public void Reset()
    {
        currentProgress = 0;
    }
}
