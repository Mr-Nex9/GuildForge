using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameState gameState;
    public UnityEvent UIUpdate;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CalculateOddJobsRate();
    }

    public float timePassed;
    void Update()
    {
        timePassed += Time.deltaTime;
        if (timePassed > 5)
        {
            gameState.gold = gameState.gold + gameState.currentOddJobsRate;
            timePassed = 0;
        }
        foreach(Mission mission in gameState.ActiveMissions)
        {
            mission.CheckForCompletion();
        }
        UIUpdate.Invoke();
    }
    #region Setup/ Loading Functions
    void ReloadRoster()
    {
        List<Adventurer> temp = new List<Adventurer>();
        temp.AddRange(Resources.LoadAll<Adventurer>("Adventurers"));

        foreach (Adventurer adventurer in temp)
        {
            if (adventurer.Recruited)
            {
                Recruit(adventurer);
            }
        }
    }
    #endregion
    #region Calls From Other Classes
    public void AddToInventory(Item item)
    {
        gameState.Inventory.Add(item);
    }
    public void RemoveFromInventory(Item item)
    {
        gameState.Inventory.Remove(item);
    }
    public void MissionCompleted(int gold, int reputation)
    {
        gameState.gold += gold;
        gameState.reputation += reputation;
    }
    public void CalculateOddJobsRate()
    {
        gameState.currentOddJobsRate = 1;
    }

    public void Recruit(Adventurer adventurer)
    {
        gameState.Roster.Add(adventurer);
    }
    public int getCurGold()
    {
        return gameState.gold;
    }
    public void PayGold(int amount)
    {
        gameState.gold -= amount;
    }
    public void GainReputation(int amount)
    {
        gameState.reputation += amount;
    }
    public void AddToActiveMissionList(Mission mission)
    {
        gameState.ActiveMissions.Add(mission);
    }
    #endregion
}
