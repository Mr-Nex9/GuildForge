using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using SQLite4Unity3d;
using System;
using System.IO;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using static UnityEditor.FilePathAttribute;
using static UnityEditor.Progress;

public class SQLiteSystems
{
    GameState gameState;
    private SQLiteConnection connection;

    public void LoadAllDataFromDatabase(GameState state)
    {
        gameState = state;
        CreateAndOpenDatabase();

        LoadGameSate();
        LoadSettings();
        LoadAdventurers();
        LoadInventory();
        LoadMissions();
    }

    void LoadAdventurers()
    {
        Debug.Log("Loading Adventurer Data");
        List<AdventurerDB>  reader = new List<AdventurerDB>(CreateAndOpenAdventurersTable());

        foreach (AdventurerDB dbRead in reader)
        {
            int iD = dbRead.ID;
            Adventurer curAdventurer = gameState.allAdventurers.Find(x => x.ID == iD);

            curAdventurer.Recruited = dbRead.Recruited;
            curAdventurer.currentMissionID = dbRead.OnMission;
            curAdventurer.EXP = dbRead.EXP;
            curAdventurer.health = dbRead.Health;
            curAdventurer.mana = dbRead.Mana;
            curAdventurer.attack = dbRead.Attack;
            curAdventurer.defense = dbRead.Defense;
            curAdventurer.magic = dbRead.Magic;
            curAdventurer.speed = dbRead.Speed;

            if (dbRead.WeaponID > 0)
                curAdventurer.WeaponItem = (Weapon)gameState.allItems.Find(x => x.ID == dbRead.WeaponID);
            if (dbRead.ArmorID > 0)
                curAdventurer.ArmorItem = (Armor)gameState.allItems.Find(x => x.ID == dbRead.ArmorID);
            if (dbRead.Accessory1ID > 0)
                curAdventurer.AccessorySlot1 = (Accessory)gameState.allItems.Find(x => x.ID == dbRead.Accessory1ID);
            if (dbRead.Accessory2ID > 0)
                curAdventurer.AccessorySlot2 = (Accessory)gameState.allItems.Find(x => x.ID == dbRead.Accessory2ID);

            curAdventurer.Loaded();
            if(curAdventurer.Recruited)
            {
                gameState.roster.Add(curAdventurer);
            }
            else if(curAdventurer.offered)
            {
                gameState.localAdventurers.Add(curAdventurer);
            }
        }
    }
    void LoadInventory()
    {
        Debug.Log("Loading Inventory Data");
        List<ItemDB> reader = new List<ItemDB>(CreateAndOpenInventoryTable());

        foreach (ItemDB dbRead in reader)
        {
            int iD = dbRead.ID; ;
            int location = dbRead.Location;

            Item curItem = gameState.allItems.Find(x => x.ID == iD);

            if( location == 1)
            {
                gameState.storeInventory.Add(curItem);
            }
            else if ( location == 2)
            {
                gameState.inventory.Add(curItem);
            }
        }
    }
    void LoadGameSate()
    {
        Debug.Log("Loading GameState");
        List<GameStateDB> reader = new List<GameStateDB>(CreateAndOpenGameStateTable());

        foreach (GameStateDB dbRead in reader)
        {
            switch(dbRead.ID)
            {
                case 1:
                    {
                        gameState.gold = (int)dbRead.Amount;
                        
                    }break;
                case 2:
                    {
                        gameState.reputation = (int)dbRead.Amount;
                    }break;
                case 3:
                    {
                        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                        dateTime = dateTime.AddSeconds(dbRead.Amount).ToLocalTime();
                        gameState.storeLastStocked = dateTime;
                    }
                    break;
                case 4:
                    {
                        gameState.totalGold = (int)dbRead.Amount;
                    }
                    break;
                case 5:
                    {
                        GameState.bonusPercent = 1 + (dbRead.Amount / 100);
                    }
                    break;
                case 6:
                    {
                        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                        dateTime = dateTime.AddSeconds(dbRead.Amount).ToLocalTime();
                        gameState.lastOpened = dateTime;
                    }
                    break;
            }

        }
    }
    void LoadSettings()
    {
        Debug.Log("Loading Settings");
        List<SettingDB> reader = new List<SettingDB>(CreateAndOpenSettingsTable());

        foreach (SettingDB dbRead in reader)
        {
            switch (dbRead.ID)
            {
                case 1:
                    {
                        gameState.tutorialLevel = dbRead.Setting;

                    }
                    break;
                case 2:
                    {
                        gameState.sound = dbRead.Setting > 0 ? true : false;
                    }
                    break;
                case 3:
                    {
                        gameState.music = dbRead.Setting > 0 ? true : false;
                    }
                    break;
                case 4:
                    {
                        gameState.effects = dbRead.Setting > 0 ? true : false;
                    }
                    break;
                case 5:
                    {
                        gameState.notifications = dbRead.Setting > 0 ? true : false;
                    }
                    break;
            }

        }
    }
    void LoadMissions()
    {
        Debug.Log("Loading Mission Data");
        List<MissionDB> reader = new List<MissionDB>(CreateAndOpenMissionTable());

        foreach (MissionDB dbRead in reader)
            {
            int iD = dbRead.ID;
            int status = dbRead.Status;

            Mission curMission = gameState.allMissions.Find(x => x.ID == iD);
            curMission.Difficulty = dbRead.Difficulty;
            switch(status)
            {
                case 1:
                    {
                        curMission.Offered = true;

                        gameState.offeredMissions.Add(curMission);
                    }break;
                case 2:
                    {
                        curMission.Offered = true;
                        curMission.Active  = true;
                        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                        dateTime = dateTime.AddSeconds(dbRead.StartTime).ToLocalTime();
                        curMission.StartTime = dateTime;

                        gameState.activeMissions.Add(curMission);
                    }
                    break;
                case 3:
                    {
                        curMission.Offered = true;
                        curMission.Active = true;
                        curMission.Completed = true;

                        gameState.missionsCompleted.Add(curMission);
                    }
                    break;
            }
            switch(dbRead.Rank)
            {
                case 1:
                    {
                        curMission.Rank = Mission.MissionRank.S;
                    }
                    break;
                case 2:
                    {
                        curMission.Rank = Mission.MissionRank.A;
                    }
                    break;
                case 3:
                    {
                        curMission.Rank = Mission.MissionRank.B;
                    }
                    break;
                case 4:
                    {
                        curMission.Rank = Mission.MissionRank.C;
                    }
                    break;
                case 5:
                    {
                        curMission.Rank = Mission.MissionRank.D;
                    }
                    break;
            }
                
        }
    }

    #region Save Methods
    public void UpdateGameState(int iD, long amount)
    {
        // 1 - Gold, 2 - Reputation, 3 - laststockedtime in Unix seconds, 4 - total gold,  5- current Bonus, 6 - lastopened
        var updatedValue = new GameStateDB
        {
            ID = iD,
            Amount = amount
        };

        connection.InsertOrReplace(updatedValue);
    }
    public void UpdateSettings(int iD, int setting)
    {
        // 1 - Tutorial, 2 - Sound, 3 - music, 4 - effects,  5- Notifications
        var updatedValue = new SettingDB
        {
            ID = iD,
            Setting = setting
        };

        connection.InsertOrReplace(updatedValue);
    }
    public void UpdateAdventurers(Adventurer adventurer)
    {
        int weaponID = (adventurer.WeaponItem != null) ? adventurer.WeaponItem.ID: 0;
        int armorID = (adventurer.ArmorItem != null) ? adventurer.ArmorItem.ID : 0; 
        int accessory1ID = (adventurer.AccessorySlot1 != null) ? adventurer.AccessorySlot1.ID : 0; 
        int accessory2ID = (adventurer.AccessorySlot2 != null) ? adventurer.AccessorySlot2.ID : 0;
        int recruited = (adventurer.Recruited) ? 1 : 0;

        var updatedValue = new AdventurerDB
        {
            ID = adventurer.ID,
            Recruited = adventurer.Recruited,
            OnMission = adventurer.currentMissionID,
            EXP = adventurer.EXP,
            Health = adventurer.health,
            Mana = adventurer.mana,
            Attack = adventurer.attack,
            Defense = adventurer.defense,
            Magic = adventurer.magic,
            Speed = adventurer.speed,
            WeaponID = weaponID,
            ArmorID = armorID,
            Accessory1ID = accessory1ID,
            Accessory2ID = accessory2ID,
        };

        connection.InsertOrReplace(updatedValue);
    }
    public void UpdateInventory(Item item, int location)
    {
        var updatedValue = new ItemDB
        {
            ID = item.ID,
            Location = location
        };

        connection.InsertOrReplace(updatedValue);
    }
    public void UpdateAchievements()
    {

    }
    public void UpdateMission(Mission mission, int status)
    {
        MissionDB updatedValue;
        int rank = 0;
        switch (mission.Rank)
        {
            case Mission.MissionRank.S:
                {
                    rank = 1;
                }break;
            case Mission.MissionRank.A:
                {
                    rank = 2;
                }
                break;
            case Mission.MissionRank.B:
                {
                    rank = 3;
                }
                break;
            case Mission.MissionRank.C:
                {
                    rank = 4;
                }
                break;
            case Mission.MissionRank.D:
                {
                    rank = 5;
                }
                break;
        }
        if (status == 2)
        {
            updatedValue = new MissionDB
            {
                ID = mission.ID,
                Status = status,
                StartTime = ((DateTimeOffset)mission.StartTime).ToUnixTimeSeconds(),
                Difficulty = mission.Difficulty,
                Rank = rank

            };
        }
        else
        {
            updatedValue = new MissionDB
            {
                ID = mission.ID,
                Status = status,
                Difficulty = mission.Difficulty,
                Rank = rank
            };
        }

        connection.InsertOrReplace(updatedValue);
    }
    #endregion

    #region Create/Open Database Methods

    void CreateAndOpenDatabase()
    {
#if UNITY_EDITOR
        var dbPath = string.Format(@"Assets/StreamingAssets/GuildForge");
#else
        // check if file exists in Application.persistentDataPath
        var filepath = string.Format("{0}/{1}", Application.persistentDataPath, DatabaseName);

        if (!File.Exists(filepath))
        {
            Debug.Log("Database not in Persistent path");
            // if it doesn't ->
            // open StreamingAssets directory and load the db ->

#if UNITY_ANDROID
            var loadDb = new WWW("jar:file://" + Application.dataPath + "!/assets/GuildForge");  // this is the path to your StreamingAssets in android
            while (!loadDb.isDone) { }  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
            // then save to Application.persistentDataPath
            File.WriteAllBytes(filepath, loadDb.bytes);
#elif UNITY_IOS
                 var loadDb = Application.dataPath + "/Raw/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);
#elif UNITY_WP8
                var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);

#elif UNITY_WINRT
		var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
		// then save to Application.persistentDataPath
		File.Copy(loadDb, filepath);
		
#elif UNITY_STANDALONE_OSX
		var loadDb = Application.dataPath + "/Resources/Data/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
		// then save to Application.persistentDataPath
		File.Copy(loadDb, filepath);
#else
	var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
	// then save to Application.persistentDataPath
	File.Copy(loadDb, filepath);

#endif

            Debug.Log("Database written");
        }

        var dbPath = filepath;
#endif
        connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
        Debug.Log("Final PATH: " + dbPath);
    }
    IEnumerable<ItemDB> CreateAndOpenInventoryTable()
    {
        connection.CreateTable<ItemDB>();    

        return connection.Table<ItemDB>();
    }
    IEnumerable<MissionDB> CreateAndOpenMissionTable()
    {
        connection.CreateTable<MissionDB>();

        return connection.Table<MissionDB>();
    }
    IEnumerable<GameStateDB> CreateAndOpenGameStateTable()
    {
        connection.CreateTable<GameStateDB>();

        return connection.Table<GameStateDB>();
    }
    /*IDbConnection CreateAndOpenAchievementTable()
    {
        string dbUri = "URI=file:GuildForgeDB.sqlite";
        IDbConnection dbConnection = new SqliteConnection(dbUri);
        dbConnection.Open();

        IDbCommand dbCommandCreateTable = dbConnection.CreateCommand();
        dbCommandCreateTable.CommandText = "CREATE TABLE IF NOT EXISTS Achievements (id INTEGER PRIMARY KEY, progress INTEGER, collected BOOL )";
        dbCommandCreateTable.ExecuteReader();

        return dbConnection;
    }*/
    IEnumerable<AdventurerDB> CreateAndOpenAdventurersTable()
    {
        connection.CreateTable<AdventurerDB>();

        return connection.Table<AdventurerDB>();
    }
    IEnumerable<SettingDB> CreateAndOpenSettingsTable()
    {
        connection.CreateTable<SettingDB>();

        return connection.Table<SettingDB>();
    }
    #endregion

    public void NewGame()
    {
        ClearAdventurerTable();
        ClearMissionTable();
        ClearInventoryTable();

        //reset gamestate values except bonus
        for (int i = 1; i < 6; i++)
        {
            UpdateGameState(i, 0);
        }
    }

    public void ResetGameData()
    {
        ClearAdventurerTable();
        ClearMissionTable();
        ClearInventoryTable();
        ClearGameStateTable();
    }
    void ClearAdventurerTable()
    {
        connection.DropTable<AdventurerDB>();
    }
    void ClearMissionTable()
    {
        connection.DropTable<MissionDB>();
    }
    void ClearInventoryTable()
    {
        connection.DropTable<ItemDB>();
    }
    void ClearGameStateTable()
    {
        connection.DropTable<GameStateDB>();
    }


}


