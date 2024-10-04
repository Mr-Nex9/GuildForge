using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameState gameState;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameState.gold = 0;
        LoadData();
        ReloadRoster();
        CalculateOddJobsRate();
        CalculateGuildRank();
    }

    public float timePassed;
    void Update()
    {
        timePassed += Time.deltaTime;
        if (timePassed > (1f / gameState.currentOddJobsRate))
        {
            gameState.gold += 1;
            timePassed = 0;
        }
    }
    #region Setup/Calculation Functions
    void LoadData()
    {
        //create a separate script for loading data from saves
        //needs to load adventurers & missions data first
        //then pass gold and reputation
        //then call  load lists
    }
    void ReloadRoster()
    {
        List<Adventurer> temp = new List<Adventurer>();
        temp.AddRange(Resources.LoadAll<Adventurer>("Adventurers"));

        foreach (Adventurer adventurer in temp)
        {
            if (adventurer.Recruited && !gameState.roster.Contains(adventurer))
            {
                Recruit(adventurer);
            }
        }
    }
    public void CalculateOddJobsRate()
    {
        gameState.currentOddJobsRate = 1;
        foreach (Adventurer x in gameState.roster)
        {
            if (x.OnMission == false)
            {
                gameState.currentOddJobsRate += x.CalculateRate();
            }
        }

        if (gameState.currentOddJobsRate < 0)
        {
            gameState.currentOddJobsRate = 1;
        }
    }
    void CalculateGuildRank()
    {
        switch (gameState.reputation / 200)
        {
            case 0:
                {
                    gameState.guildRank = GameState.GuildRank.D;
                }
                break;
            case 1:
                {
                    gameState.guildRank = GameState.GuildRank.C;
                }
                break;
            case 2:
                {
                    gameState.guildRank = GameState.GuildRank.B;
                }
                break;
            case 3:
                {
                    gameState.guildRank = GameState.GuildRank.A;
                }
                break;
            case 4 or 5:
                {
                    gameState.guildRank = GameState.GuildRank.S;
                }
                break;
        }
    }
    #endregion

    #region Get GameState Variables
    public GameState.GuildRank GetGuildRank()
    {
        return gameState.guildRank;
    }
    public int getCurGold()
    {
        return gameState.gold;
    }
    public bool GetSettings(int setting)
    {
        switch(setting)
        {
            case 0:
                {
                    return gameState.sound;
                }
            case 1:
                {
                    return gameState.music;
                }
            case 2:
                {
                    return gameState.effects;
                }
            case 3:
                {
                    return gameState.notifications;
                }
        }
        throw new NotImplementedException();
    }
 
    #endregion

    #region Change GameState Variables
    public void Recruit(Adventurer adventurer)
    {
        gameState.roster.Add(adventurer);
    }
    public void PayGold(int amount)
    {
        gameState.gold -= amount;
    }
    public void GainReputation(int amount)
    {
        gameState.reputation += amount;
    }
    public void StockStore(List<Item> items)
    {
        foreach (Item item in items)
        {
            gameState.storeInventory.Add(item);
        }
    }
    public void AddToInventory(Item item)
    {
        gameState.inventory.Add(item);
    }
    public void RemoveFromInventory(Item item)
    {
        gameState.inventory.Remove(item);
    }
    public void AddToActiveMissionList(Mission mission)
    {
        gameState.activeMissions.Add(mission);
    }
    public void SetSettings(int setting, bool value)
    {
        switch (setting)
        {
            case 0:
                {
                    gameState.sound = value;
                }
                break;
            case 1:
                {
                    gameState.music = value;
                }
                break;
            case 2:
                {
                    gameState.effects = value;
                }
                break;
            case 3:
                {
                    gameState.notifications = value;
                }break;
        }
    }
    #endregion

    #region Called From Other Classes
    public void MissionCompleted(int gold, int reputation, Mission mission)
    {
        gameState.gold += gold;
        gameState.reputation += reputation;
        gameState.activeMissions.Remove(mission);
        gameState.missionsCompleted.Add(mission);
        CalculateOddJobsRate();
        CalculateGuildRank();

    }
    public bool IsRestockTime()
    {
        if (DateTime.Now - gameState.storeLastStocked > gameState.restockTime)
        {
            gameState.storeLastStocked = DateTime.Now;
            return true;

        }
        else
        {
            return false;
        }
    }
    #endregion
}
