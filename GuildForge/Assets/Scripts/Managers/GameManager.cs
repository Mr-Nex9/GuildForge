using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using UnityEngine.Android;

public class GameManager : MonoBehaviour
{
    [SerializeField] public GameState gameState;
    public SQLiteSystems sqlSystem;
    public float timePassed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        if (!Permission.HasUserAuthorizedPermission("android.permission.READ_DEVICE_CONFIG"))
        {
            Permission.RequestUserPermission("android.permission.READ_DEVICE_CONFIG");
        }

        Debug.Log("Game Start!");
        sqlSystem = new SQLiteSystems();
        gameState.GameLoaded = false;

        ResetGameState();
        LoadData();
        CalculateOddJobsRate();
        CalculateGuildRank();

        Debug.Log("Game Loaded");
        if(gameState.roster.Count > 0)
        {
            gameState.firstTime = false;
        }
        else
        {
            gameState.firstTime = true;
        }
        Debug.Log(gameState.firstTime);
        CalculateTimeAway();
        gameState.GameLoaded = true;
    }
    void Update()
    {
        if(gameState.currentOddJobsRate > 0)
        {        
            timePassed += Time.deltaTime;
            if (timePassed > (10f / gameState.currentOddJobsRate))
            {
                GainGold((int)(1 * (1 + GameState.bonusPercent)));
                timePassed = 0;
            }
        }
        if(gameState.reputation >= 1000)
        {
            GameObject UIMaster = GameObject.FindGameObjectWithTag("UI Manager");
            UIMaster.GetComponent<UIManager>().ActivateFranchiseButton(true);
        }
    }
    private void OnApplicationQuit()
    {
        gameState.lastOpened = DateTime.Now;

        sqlSystem.UpdateGameState(6, ((DateTimeOffset)gameState.storeLastStocked).ToUnixTimeSeconds());
        Debug.Log(gameState.lastOpened);
    }
    #region Setup/Calculation Functions
    void ResetGameState()
    {
        Debug.Log("Reset Lists");
        gameState.roster.Clear();
        gameState.inventory.Clear();
        gameState.storeInventory.Clear();
        gameState.missionsCompleted.Clear();
        gameState.activeMissions.Clear();
        gameState.offeredMissions.Clear();
        gameState.localAdventurers.Clear();

        gameState.gold = 0;
        gameState.totalGold = 0;
        gameState.reputation = 0;
        GameState.bonusPercent = 0;

    }
    void LoadData()
    {
        Debug.Log("Load Data from DB");
        gameState.allAdventurers = new List<Adventurer>();
        gameState.allAdventurers.AddRange(Resources.LoadAll<Adventurer>("Adventurers"));
        gameState.allItems = new List<Item>();
        gameState.allItems.AddRange(Resources.LoadAll<Item>("Items"));
        gameState.allMissions = new List<Mission>();
        gameState.allMissions.AddRange(Resources.LoadAll<Mission>("Missions"));
        gameState.allQuests = new List<Quest>();
        gameState.allQuests.AddRange(Resources.LoadAll<Quest>("Quests"));

        foreach(Quest q in gameState.allQuests)
        {
            if(q is MissionQuest && !gameState.missionQuests.Contains(q))
            {
                gameState.missionQuests.Add(q as MissionQuest);
            }
            else if (q is AdventurerQuest && !gameState.adventurerQuests.Contains(q))
            {
                gameState.adventurerQuests.Add(q as AdventurerQuest);
            }
            else if(q is GoldQuest && !gameState.goldQuests.Contains(q))
            {
                gameState.goldQuests.Add(q as GoldQuest);
            }
        }

        sqlSystem.LoadAllDataFromDatabase(gameState);
        RestartMissions();
    }
    void RestartMissions()
    {
        Debug.Log("Updating Mission Status");
        foreach (Adventurer adventurer in gameState.roster)
        {
            if(adventurer.currentMissionID > 0)
            {
                adventurer.OnMission = true;
                Mission curmission = gameState.allMissions.Find(x => x.ID == adventurer.currentMissionID);
                curmission.AssignedAdventurers.Add(adventurer);
            }
        }

        foreach(Mission mission in gameState.activeMissions)
        {
            mission.RestartMission();
        }
    }
    public void CalculateOddJobsRate()
    {
        gameState.currentOddJobsRate = 0;
        bool adventurersHome = false;
        foreach (Adventurer x in gameState.roster)
        {
            if (x.OnMission == false)
            {
                gameState.currentOddJobsRate += x.CalculateRate();
                adventurersHome = true;
            }
        }
        if (gameState.currentOddJobsRate < 1 && adventurersHome)
        {
            gameState.currentOddJobsRate = 1;
        }
    }
    void CalculateGuildRank()
    {
        GameState.GuildRank curRank = GameState.guildRank;
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
        if(GameState.guildRank != curRank)
        {
            FindLocalAdventurers(true);
        }
    }
    void CalculateTimeAway()
    {
        int amount = 0;
        Debug.Log(gameState.lastOpened);
        if (gameState.lastOpened < new DateTime(2024, 10, 2))
        {
            TimeSpan elapsed = DateTime.Now - gameState.lastOpened;

            amount = (int)((elapsed.TotalSeconds / (10f / gameState.currentOddJobsRate)) * 1 + GameState.bonusPercent);

            GainGold(amount);
        }
        Debug.Log($"You gained {amount} gold while you were gone!");
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
            case 4:
                {
                    return gameState.password;
                }
        }
        throw new NotImplementedException();
    }
    public int GetTotalGold()
    {
        return gameState.totalGold;
    }

    #endregion

    #region Change GameState Variables
    public void PayGold(int amount)
    {
        if(amount > 0)
        {
            gameState.gold -= amount;
            sqlSystem.UpdateGameState(1, gameState.gold);

            foreach (GoldQuest q in gameState.goldQuests)
            {
                q.CheckforCompletion(amount);
                sqlSystem.UpdateQuest(q);
            }
        }

    }
    public void GainGold(int amount)
    {
        if (amount > 0)
        {
            gameState.gold += amount;
            gameState.totalGold += amount;
            sqlSystem.UpdateGameState(1, gameState.gold);
            sqlSystem.UpdateGameState(4, gameState.totalGold);

            foreach (GoldQuest q in gameState.goldQuests)
            {
                q.CheckforCompletion(amount);
                sqlSystem.UpdateQuest(q);
            }
        }
    }
    public void GainReputation(int amount)
    {
        gameState.reputation += amount;
        sqlSystem.UpdateGameState(2, gameState.reputation);
    }
    public void SetSettings(int setting, bool value, string password = null)
    {
        switch (setting)
        {
            case 0:
                {
                    gameState.sound = value;
                    if(value)
                    {
                        sqlSystem.UpdateSettings(0, 1);
                    }
                    else
                    {
                        sqlSystem.UpdateSettings(0, 0);
                    }
                    
                }
                break;
            case 1:
                {
                    gameState.music = value;
                    if (value)
                    {
                        sqlSystem.UpdateSettings(1, 1);
                    }
                    else
                    {
                        sqlSystem.UpdateSettings(1, 0);
                    }
                }
                break;
            case 2:
                {
                    gameState.effects = value;
                    if (value)
                    {
                        sqlSystem.UpdateSettings(2, 1);
                    }
                    else
                    {
                        sqlSystem.UpdateSettings(2, 0);
                    }
                }
                break;
            case 3:
                {
                    gameState.notifications = value;
                    if (value)
                    {
                        sqlSystem.UpdateSettings(3, 1);
                    }
                    else
                    {
                        sqlSystem.UpdateSettings(3, 0);
                    }
                }
                break;
            case 4:
                {
                    gameState.password = value;
                    if (value)
                    {
                        sqlSystem.UpdateSettings(4, 1, password);
                        gameState.passwordValue = password;
                    }
                    else
                    {
                        sqlSystem.UpdateSettings(4, 0);
                        gameState.passwordValue = null;
                    }
                }
                break;
        }
    }

    public List<Adventurer> FindLocalAdventurers(bool restock)
    {   
        if(gameState.firstTime)
        {
            gameState.localAdventurers = new List<Adventurer>(gameState.firstTimeAdventurers);

            foreach (Adventurer adventurer in gameState.firstTimeAdventurers)
            {
                adventurer.SetBaseStats();
                adventurer.CostToRecruit = 0;
            }
            return gameState.localAdventurers;
        }
        if (restock)
        {
            List<Adventurer> filteredList = new List<Adventurer>();

            foreach (Adventurer adventurer in gameState.allAdventurers)
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
        }

        return gameState.localAdventurers;
    }
    public void Recruit(Adventurer adventurer)
    {
        Debug.Log(adventurer.Name + " recruited");
        PayGold(adventurer.CostToRecruit);
        gameState.localAdventurers.Remove(adventurer);
        gameState.roster.Add(adventurer);
        sqlSystem.UpdateAdventurers(adventurer);

        GainReputation(2 * adventurer.Level);

        if (gameState.firstTime)
        {
            gameState.firstTime = false;
            foreach(Adventurer advenventurer in gameState.localAdventurers)
            {
                adventurer.ResetData();
            }
            gameState.localAdventurers.Clear();
            FindLocalAdventurers(true);
        }
        CalculateOddJobsRate();

        foreach (AdventurerQuest q in gameState.adventurerQuests)
        {
            q.CheckForCompletion(adventurer);
            sqlSystem.UpdateQuest(q);
        }
    }
    public void UpdateAdventurer(Adventurer adventurer)
    {
        sqlSystem.UpdateAdventurers(adventurer);
    }
    public void AddToActiveMissionList(Mission mission)
    {
        gameState.offeredMissions.Remove(mission);
        gameState.activeMissions.Add(mission);
        sqlSystem.UpdateMission(mission, 2);
    }
    public List<Mission> FindNewMissions()
    {
        List<Mission> filteredList = new List<Mission>();

        foreach (Mission mission in gameState.allMissions)
        {
            if (mission.Offered == false && !gameState.offeredMissions.Contains(mission))
            {
                filteredList.Add(mission);
            }
        }

        for (int i = 0; i < filteredList.Count; i++)
        {
            if (gameState.offeredMissions.Count < 5)
            {
                if (UnityEngine.Random.Range(0, filteredList.Count - i) < 4 - gameState.offeredMissions.Count)
                {
                    gameState.offeredMissions.Add(filteredList[i]);
                    filteredList[i].PrepareToOffer();
                    sqlSystem.UpdateMission(filteredList[i], 1);
                }
            }
            else
            {
                break;
            }
        }
        return gameState.offeredMissions;
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
                        if (item.itemRank == Item.ItemRank.D || item.itemRank == Item.ItemRank.C)
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
        sqlSystem.UpdateMission(mission, 3);
        CalculateOddJobsRate();
        CalculateGuildRank();

        foreach (MissionQuest q in gameState.missionQuests)
        {
            q.CheckForCompletion(mission);
            sqlSystem.UpdateQuest(q);
        }
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
        NewGame(false);
        GameState.bonusPercent += CalculateBonus();
        sqlSystem.UpdateGameState(5, (long)(GameState.bonusPercent*100));
        Start();
    }
    public void NewGame(bool isReset)
    {
        foreach (Adventurer adventurer in gameState.allAdventurers)
        {
            adventurer.ResetData();
        }
        foreach (Mission mission in gameState.allMissions)
        {
            mission.ResetData();
        }
        foreach(Quest quest in gameState.allQuests)
        {
            quest.Reset();
        }

        gameState.gold = 0;
        gameState.totalGold = 0;
        gameState.reputation = 0;
        GameState.bonusPercent = 0;

        gameState.activeMissions.Clear();
        gameState.missionsCompleted.Clear();
        gameState.inventory.Clear();
        gameState.storeInventory.Clear();
        gameState.roster.Clear();
        gameState.localAdventurers.Clear();


        if (isReset)
        {
            sqlSystem.ResetGameData();
        }
        else
        {
            sqlSystem.NewGame();
        }
        Start();
    }
    #endregion
}
