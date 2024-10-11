using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameState gameState;
    SQLiteSystems sqlSystem;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sqlSystem = new SQLiteSystems();

        ResetGameState();
        LoadData();
        CalculateOddJobsRate();
        CalculateGuildRank();

        if (gameState.reputation >= 1000)
        {
            GameObject UIMaster = GameObject.FindGameObjectWithTag("UI Manager");
            UIMaster.GetComponent<UIManager>().ActivateFranchiseButton(true);
        }
        else
        {
            GameObject UIMaster = GameObject.FindGameObjectWithTag("UI Manager");
            UIMaster.GetComponent<UIManager>().ActivateFranchiseButton(false);
        }
    }

    public float timePassed;
    void Update()
    {
        timePassed += Time.deltaTime;
        if (timePassed > (10f / gameState.currentOddJobsRate))
        {
            GainGold((int)(1 * GameState.bonusPercent));
            timePassed = 0;
        }
        if(gameState.reputation >= 1000)
        {
            GameObject UIMaster = GameObject.FindGameObjectWithTag("UI Manager");
            UIMaster.GetComponent<UIManager>().ActivateFranchiseButton(true);
        }
    }
    #region Setup/Calculation Functions
    void ResetGameState()
    {
        gameState.roster.Clear();
        gameState.inventory.Clear();
        gameState.storeInventory.Clear();
        gameState.missionsCompleted.Clear();
        gameState.activeMissions.Clear();
        gameState.localAdventurers.Clear();
    }
    void LoadData()
    {
        gameState.allAdventurers = new List<Adventurer>();
        gameState.allAdventurers.AddRange(Resources.LoadAll<Adventurer>("Adventurers"));
        gameState.allItems = new List<Item>();
        gameState.allItems.AddRange(Resources.LoadAll<Item>("Items"));
        gameState.allMissions = new List<Mission>();
        gameState.allMissions.AddRange(Resources.LoadAll<Mission>("Missions"));

        sqlSystem.LoadAllDataFromDatabase(gameState);
    }

    public void CalculateOddJobsRate()
    {
        gameState.currentOddJobsRate = 0;
        foreach (Adventurer x in gameState.roster)
        {
            if (x.OnMission == false)
            {
                gameState.currentOddJobsRate += x.CalculateRate();
            }
        }
        if(gameState.currentOddJobsRate < 1)
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
                    GameState.guildRank = GameState.GuildRank.D;
                }
                break;
            case 1:
                {
                    GameState.guildRank = GameState.GuildRank.C;
                }
                break;
            case 2:
                {
                    GameState.guildRank = GameState.GuildRank.B;
                }
                break;
            case 3:
                {
                    GameState.guildRank = GameState.GuildRank.A;
                }
                break;
            case 4 or 5:
                {
                    GameState.guildRank = GameState.GuildRank.S;
                }
                break;
        }
    }
    #endregion

    #region Get GameState Variables
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
    public List<Adventurer> FindLocalAdventurers()
    {   
        List<Adventurer> filteredList = new List<Adventurer>();

        foreach(Adventurer adventurer in gameState.allAdventurers)
        {
            if (adventurer.Recruited == false)
            {
                filteredList.Add(adventurer);
            }
        }

        for (int i = 0; i < filteredList.Count; i++)
        {
            if (gameState.localAdventurers.Count < 4)
            {
                if (UnityEngine.Random.Range(0, filteredList.Count - i) < 4 - gameState.localAdventurers.Count)
                {
                    gameState.localAdventurers.Add(filteredList[i]);
                    filteredList[i].PrepareForRecruitment();
                    sqlSystem.UpdateAdventurers(filteredList[i]);
                }
            }
            else
            {
                break;
            }
        }
        return gameState.localAdventurers;
    }
    public void Recruit(Adventurer adventurer)
    {
        gameState.localAdventurers.Remove(adventurer);
        gameState.roster.Add(adventurer);
        sqlSystem.UpdateAdventurers(adventurer);

        GainReputation(2 * adventurer.Level);
    }
    public void PayGold(int amount)
    {
        gameState.gold -= amount;
        sqlSystem.UpdateGameState(1, gameState.gold);
    }
    public void GainGold(int amount)
    {
        gameState.gold += amount;
        sqlSystem.UpdateGameState(1, gameState.gold);
        gameState.totalGold += amount;
        sqlSystem.UpdateGameState(4, gameState.totalGold);
    }
    public void GainReputation(int amount)
    {
        gameState.reputation += amount;
        sqlSystem.UpdateGameState(2, gameState.reputation);
    }
    public void AddToInventory(Item item)
    {
        gameState.inventory.Add(item);
        sqlSystem.UpdateInventory(item, 2);
    }
    public void RemoveFromInventory(Item item)
    {
        gameState.inventory.Remove(item);
        sqlSystem.UpdateInventory(item, 0);
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
    private void FillStoreInventory()
    {
        foreach(Item item in gameState.storeInventory)
        {
            sqlSystem.UpdateInventory(item, 0);
        }
        //get all items
        List<Item> allItems = new List<Item>();
        allItems.AddRange(Resources.LoadAll<Item>("Items"));

        //filter items based on rank
        List<Item> filteredItems = new List<Item>();
        foreach (Item item in allItems)
        {
            switch (GameState.guildRank)
            {
                case GameState.GuildRank.S:
                    {
                        if (item.itemRank == Item.ItemRank.S || item.itemRank == Item.ItemRank.A)
                        {
                            filteredItems.Add(item);
                        }
                    }
                    break;
                case GameState.GuildRank.A:
                    {
                        if (item.itemRank == Item.ItemRank.S || item.itemRank == Item.ItemRank.S || item.itemRank == Item.ItemRank.B)
                        {
                            filteredItems.Add(item);
                        }
                    }
                    break;
                case GameState.GuildRank.B:
                    {
                        if (item.itemRank == Item.ItemRank.A || item.itemRank == Item.ItemRank.B || item.itemRank == Item.ItemRank.C)
                        {
                            filteredItems.Add(item);
                        }
                    }
                    break;
                case GameState.GuildRank.C or GameState.GuildRank.D:
                    {
                        if (item.itemRank == Item.ItemRank.B || item.itemRank == Item.ItemRank.D || item.itemRank == Item.ItemRank.C)
                        {
                            filteredItems.Add(item);
                        }
                    }
                    break;
            }
        }

        //if there are more than 15 items random selection sample down to 15 if not just add them all to store inventory
        if (filteredItems.Count > 15)
        {
            gameState.storeInventory = new List<Item>();
            for (int i = 0; i < filteredItems.Count; i++)
            {
                if (gameState.storeInventory.Count < 15)
                {
                    if (UnityEngine.Random.Range(0, filteredItems.Count - i) < 15 - gameState.storeInventory.Count)
                    {
                        gameState.storeInventory.Add(filteredItems[i]);
                    }
                }
                else
                {
                    break;
                }
            }
        }
        else
        {
            gameState.storeInventory = new List<Item>(filteredItems);
        }

        foreach (Item item in gameState.storeInventory)
        {
            sqlSystem.UpdateInventory(item, 1 );
        }

    }
    public void BuyItem(Item item)
    {
        AddToInventory(item);
        PayGold(item.Cost);
        gameState.storeInventory.Remove(item);
    }

    #endregion

    #region Called From Other Classes
    public void MissionCompleted(int gold, int reputation, Mission mission)
    {
        GainGold(gold);
        GainReputation(reputation);
        gameState.activeMissions.Remove(mission);
        gameState.missionsCompleted.Add(mission);
        CalculateOddJobsRate();
        CalculateGuildRank();
    }
    public List<Item> IsRestockTime()
    {
        if (DateTime.UtcNow - gameState.storeLastStocked > gameState.restockTime)
        {
            gameState.storeLastStocked = DateTime.UtcNow;
            FillStoreInventory();
            
            sqlSystem.UpdateGameState(3, ((DateTimeOffset)gameState.storeLastStocked).ToUnixTimeSeconds());
            return gameState.storeInventory;
        }
        else
        {
            return gameState.storeInventory;
        }
    }
    public void UpdateAdventurer(Adventurer adventurer)
    {
        sqlSystem.UpdateAdventurers(adventurer);
    }
    public float CalculateBonus()
    {
        int count = 0;
        int total = gameState.totalGold;
        while (total > 10)
        {
            total /= 10;
            count++;
        }

        return (float)((count + 5) * 1.912);
    }
    public void FranchiseGuild()
    {
        GameState.bonusPercent = CalculateBonus();
        sqlSystem.NewGame(gameState);

        gameState.activeMissions.Clear();
        gameState.missionsCompleted.Clear();
        gameState.inventory.Clear();
        gameState.storeInventory.Clear();
        gameState.roster.Clear();

        Start();
    }
    #endregion
}
