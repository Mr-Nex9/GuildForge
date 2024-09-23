using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameState gameState;

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
            gameState.Gold = gameState.Gold + gameState.CurrentOddJobsRate;
            timePassed = 0;
        }
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
        gameState.Gold += gold;
        gameState.Reputation += reputation;
    }
    public void CalculateOddJobsRate()
    {
        gameState.CurrentOddJobsRate = 1;
    }

    public void Recruit(Adventurer adventurer)
    {
        gameState.Roster.Add(adventurer);
    }
    public int getCurGold()
    {
        return gameState.Gold;
    }
    public void PayGold(int amount)
    {
        gameState.Gold -= amount;
    }
    public void GainReputation(int amount)
    {
        gameState.Reputation += amount;
    }
    #endregion
}
