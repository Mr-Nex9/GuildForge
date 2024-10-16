using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System;
using static UnityEditor.FilePathAttribute;
using static UnityEditor.Progress;

public class SQLiteSystems
{
    GameState gameState;
    public void LoadAllDataFromDatabase(GameState state)
    {
        gameState = state;
        LoadGameSate();
        LoadSettings();
        LoadAdventurers();
        LoadInventory();
        LoadMissions();
    }

    void LoadAdventurers()
    {
        IDbConnection dbConnection = CreateAndOpenAdventurersTable(); // 14
        IDbCommand dbCommandReadValues = dbConnection.CreateCommand(); // 15
        dbCommandReadValues.CommandText = "SELECT * FROM Adventurers"; // 16
        IDataReader dataReader = dbCommandReadValues.ExecuteReader();

        while (dataReader.Read())
        {
            int iD = dataReader.GetInt32(0);
            Adventurer curAdventurer = gameState.allAdventurers.Find(x => x.ID == iD);

            curAdventurer.Recruited = dataReader.GetBoolean(1);
            curAdventurer.currentMissionID = dataReader.GetInt32(2);
            curAdventurer.EXP = dataReader.GetInt32(3);
            curAdventurer.health = dataReader.GetInt32(4);
            curAdventurer.mana = dataReader.GetInt32(5);
            curAdventurer.attack = dataReader.GetInt32(6);
            curAdventurer.defense = dataReader.GetInt32(7);
            curAdventurer.magic = dataReader.GetInt32(8);
            curAdventurer.speed = dataReader.GetInt32(9);

            if (dataReader.GetInt32(10) > 0)
                curAdventurer.WeaponItem = (Weapon)gameState.allItems[dataReader.GetInt32(10) - 1];
            if (dataReader.GetInt32(11) > 0)
                curAdventurer.ArmorItem = (Armor)gameState.allItems[dataReader.GetInt32(11) - 1];
            if (dataReader.GetInt32(12) > 0)
                curAdventurer.AccessorySlot1 = (Accessory)gameState.allItems[dataReader.GetInt32(12) - 1];
            if (dataReader.GetInt32(13) > 0)
                curAdventurer.AccessorySlot2 = (Accessory)gameState.allItems[dataReader.GetInt32(13) - 1];

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
        dbConnection.Close();
    }
    void LoadInventory()
    {
        IDbConnection dbConnection = CreateAndOpenInventoryTable(); // 14
        IDbCommand dbCommandReadValues = dbConnection.CreateCommand(); // 15
        dbCommandReadValues.CommandText = "SELECT * FROM Inventory"; // 16
        IDataReader dataReader = dbCommandReadValues.ExecuteReader();

        while (dataReader.Read())
        {
            int iD = dataReader.GetInt32(0);
            int location = dataReader.GetInt32(1);

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
        dbConnection.Close();
    }
    void LoadGameSate()
    {
        IDbConnection dbConnection = CreateAndOpenGameStateTable(); // 14
        IDbCommand dbCommandReadValues = dbConnection.CreateCommand(); // 15
        dbCommandReadValues.CommandText = "SELECT * FROM GameState"; // 16
        IDataReader dataReader = dbCommandReadValues.ExecuteReader();
        
        while (dataReader.Read())
        {
            switch(dataReader.GetInt32(0))
            {
                case 1:
                    {
                        gameState.gold = dataReader.GetInt32(1);
                        
                    }break;
                case 2:
                    {
                        gameState.reputation = dataReader.GetInt32(1);
                    }break;
                case 3:
                    {
                        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                        dateTime = dateTime.AddSeconds(dataReader.GetInt32(1)).ToLocalTime();
                        gameState.storeLastStocked = dateTime;
                    }
                    break;
                case 4:
                    {
                        gameState.totalGold = dataReader.GetInt32(1);
                    }
                    break;
                case 5:
                    {
                        GameState.bonusPercent = 1 + (dataReader.GetInt32(1) / 100);
                    }
                    break;
            }

        }
        dbConnection.Close();
    }
    void LoadSettings()
    {
        IDbConnection dbConnection = CreateAndOpenSettingsTable(); // 14
        IDbCommand dbCommandReadValues = dbConnection.CreateCommand(); // 15
        dbCommandReadValues.CommandText = "SELECT * FROM Settings"; // 16
        IDataReader dataReader = dbCommandReadValues.ExecuteReader();

        while (dataReader.Read())
        {
            switch (dataReader.GetInt32(0))
            {
                case 1:
                    {
                        gameState.tutorialLevel = dataReader.GetInt32(1);

                    }
                    break;
                case 2:
                    {
                        gameState.sound = dataReader.GetInt32(1) > 0 ? true : false;
                    }
                    break;
                case 3:
                    {
                        gameState.music = dataReader.GetInt32(1) > 0 ? true : false;
                    }
                    break;
                case 4:
                    {
                        gameState.effects = dataReader.GetInt32(1) > 0 ? true : false;
                    }
                    break;
                case 5:
                    {
                        gameState.notifications = dataReader.GetInt32(1) > 0 ? true : false;
                    }
                    break;
            }

        }
        dbConnection.Close();
    }
    void LoadMissions()
    {
        IDbConnection dbConnection = CreateAndOpenMissionTable(); // 14
        IDbCommand dbCommandReadValues = dbConnection.CreateCommand(); // 15
        dbCommandReadValues.CommandText = "SELECT * FROM Missions"; // 16
        IDataReader dataReader = dbCommandReadValues.ExecuteReader();

        while (dataReader.Read())
        {
            int iD = dataReader.GetInt32(0);
            int status = dataReader.GetInt32(1);

            Mission curMission = gameState.allMissions.Find(x => x.ID == iD);

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
                        dateTime = dateTime.AddSeconds(dataReader.GetInt32(2)).ToLocalTime();
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
                
        }
        dbConnection.Close();
    }

    #region Save Methods
    public void UpdateGameState(int iD, long amount)
    {
        // 1 - Gold, 2 - Reputation, 3 - laststockedtime in Unix seconds, 4 - total gold,  5- current Bonus
        IDbConnection dbConnection = CreateAndOpenGameStateTable();
        IDbCommand dbCommandInsertValue = dbConnection.CreateCommand();

        dbCommandInsertValue.CommandText =
            "INSERT OR REPLACE INTO GameState (id, amount) VALUES (" +iD + ", " + amount + ")";
        dbCommandInsertValue.ExecuteNonQuery();

        dbConnection.Close();
    }
    public void UpdateSettings(int iD, int setting)
    {
        // 1 - Tutorial, 2 - Sound, 3 - music, 4 - effects,  5- Notifications
        IDbConnection dbConnection = CreateAndOpenSettingsTable();
        IDbCommand dbCommandInsertValue = dbConnection.CreateCommand();

        dbCommandInsertValue.CommandText =
            "INSERT OR REPLACE INTO Settings (id, setting) VALUES (" + iD + ", " + setting + ")";
        dbCommandInsertValue.ExecuteNonQuery();

        dbConnection.Close();
    }
    public void UpdateAdventurers(Adventurer adventurer)
    {
        int weaponID = (adventurer.WeaponItem != null) ? adventurer.WeaponItem.ID: 0;
        int armorID = (adventurer.ArmorItem != null) ? adventurer.ArmorItem.ID : 0; 
        int accessory1ID = (adventurer.AccessorySlot1 != null) ? adventurer.AccessorySlot1.ID : 0; 
        int accessory2ID = (adventurer.AccessorySlot2 != null) ? adventurer.AccessorySlot2.ID : 0;
        int recruited = (adventurer.Recruited) ? 1 : 0;

        IDbConnection dbConnection = CreateAndOpenAdventurersTable(); 
        IDbCommand dbCommandInsertValue = dbConnection.CreateCommand();
        dbCommandInsertValue.CommandText =
            "INSERT OR REPLACE INTO Adventurers " +
            "(id, recruited, onMision, exp, health, mana, attack, defense, magic, speed, weapon, armor, accessory1, accessory2) " +
            "VALUES (" + adventurer.ID + ", " + recruited + ", " + adventurer.currentMissionID + ", " + adventurer.EXP + "," +
            "" + adventurer.health + ", " + adventurer.mana + ", " + adventurer.attack + ", " + adventurer.defense + ", " + adventurer.magic + ", " + adventurer.speed + "," +
            "" + weaponID + ", " + armorID + ", " + accessory1ID + ", " + accessory2ID + ")";
        dbCommandInsertValue.ExecuteNonQuery();

        dbConnection.Close();
    }
    public void UpdateInventory(Item item, int location)
    {
        IDbConnection dbConnection = CreateAndOpenAdventurersTable();
        IDbCommand dbCommandInsertValue = dbConnection.CreateCommand();
        dbCommandInsertValue.CommandText = "INSERT OR REPLACE INTO Inventory (id, location) VALUES (" + item.ID + ", " + location + ")";
        dbCommandInsertValue.ExecuteNonQuery();

        dbConnection.Close();
    }
    public void UpdateAchievements()
    {

    }
    public void UpdateMission(int id, int status, long startTime = 0)
    {
        Debug.Log("Test");
        IDbConnection dbConnection = CreateAndOpenMissionTable();
        IDbCommand dbCommandInsertValue = dbConnection.CreateCommand();

        if (status == 2)
        {
            dbCommandInsertValue.CommandText = "INSERT OR REPLACE INTO Missions (id, status, startTime) VALUES (" + id + ", " + status + ", " + startTime + ")";
        }
        else
        {
            dbCommandInsertValue.CommandText = "INSERT OR REPLACE INTO Missions (id, status) VALUES (" + id + ", " + status + ")";
        }

        dbCommandInsertValue.ExecuteNonQuery();

        dbConnection.Close();
    }
    #endregion

    #region Create/Open Database Methods
    IDbConnection CreateAndOpenInventoryTable()
    {
        string dbUri = "URI=file:GuildForgeDB.sqlite";
        IDbConnection dbConnection = new SqliteConnection(dbUri);
        dbConnection.Open();

        IDbCommand dbCommandCreateTable = dbConnection.CreateCommand();
        dbCommandCreateTable.CommandText = "CREATE TABLE IF NOT EXISTS Inventory (id INTEGER PRIMARY KEY, location INTEGER )";
        dbCommandCreateTable.ExecuteReader();

        return dbConnection;
    }
    IDbConnection CreateAndOpenMissionTable()
    {
        string dbUri = "URI=file:GuildForgeDB.sqlite";
        IDbConnection dbConnection = new SqliteConnection(dbUri);
        dbConnection.Open();

        IDbCommand dbCommandCreateTable = dbConnection.CreateCommand();
        dbCommandCreateTable.CommandText = "CREATE TABLE IF NOT EXISTS Missions (id INTEGER PRIMARY KEY, status INTEGER, startTime INTEGER )";
        dbCommandCreateTable.ExecuteReader();

        return dbConnection;
    }
    IDbConnection CreateAndOpenGameStateTable()
    {
        string dbUri = "URI=file:GuildForgeDB.sqlite";
        IDbConnection dbConnection = new SqliteConnection(dbUri);
        dbConnection.Open();

        IDbCommand dbCommandCreateTable = dbConnection.CreateCommand();
        dbCommandCreateTable.CommandText = "CREATE TABLE IF NOT EXISTS GameState (id INTEGER PRIMARY KEY, amount INTEGER)";
        dbCommandCreateTable.ExecuteReader();

        return dbConnection;
    }
    IDbConnection CreateAndOpenAchievementTable()
    {
        string dbUri = "URI=file:GuildForgeDB.sqlite";
        IDbConnection dbConnection = new SqliteConnection(dbUri);
        dbConnection.Open();

        IDbCommand dbCommandCreateTable = dbConnection.CreateCommand();
        dbCommandCreateTable.CommandText = "CREATE TABLE IF NOT EXISTS Achievements (id INTEGER PRIMARY KEY, progress INTEGER, collected BOOL )";
        dbCommandCreateTable.ExecuteReader();

        return dbConnection;
    }
    IDbConnection CreateAndOpenAdventurersTable()
    {
        string dbUri = "URI=file:GuildForgeDB.sqlite";
        IDbConnection dbConnection = new SqliteConnection(dbUri);
        dbConnection.Open();

        IDbCommand dbCommandCreateTable = dbConnection.CreateCommand();
        dbCommandCreateTable.CommandText = 
            "CREATE TABLE IF NOT EXISTS Adventurers " +
            "(id INTEGER PRIMARY KEY, recruited BOOL, onMision INTEGER, exp INTEGER, " +
            "health INTEGER, mana INTEGER, attack INTEGER, defense INTEGER, magic INTEGER, speed INTEGER, " +
            "weapon INTEGER, armor INTEGER, accessory1 INTEGER, accessory2 INTEGER )";
        dbCommandCreateTable.ExecuteReader();

        return dbConnection;
    }
    IDbConnection CreateAndOpenSettingsTable()
    {
        string dbUri = "URI=file:GuildForgeDB.sqlite";
        IDbConnection dbConnection = new SqliteConnection(dbUri);
        dbConnection.Open();

        IDbCommand dbCommandCreateTable = dbConnection.CreateCommand();
        dbCommandCreateTable.CommandText =
            "CREATE TABLE IF NOT EXISTS Settings " +
            "(id INTEGER PRIMARY KEY, setting int)";
        dbCommandCreateTable.ExecuteReader();

        return dbConnection;
    }
    #endregion

    public void NewGame(GameState gameState)
    {
        ClearAdventurerTable();
        ClearMissionTable();
        ClearInventoryTable();

        //reset gamestate values except bonus
        for (int i = 1; i < 6; i++)
        {
            IDbConnection dbConnection = CreateAndOpenGameStateTable();
            IDbCommand dbCommandInsertValue = dbConnection.CreateCommand();

            dbCommandInsertValue.CommandText =
                "INSERT OR REPLACE INTO GameState (id, amount) VALUES (" + i + ", " + 0 + ")";
            dbCommandInsertValue.ExecuteNonQuery();

            dbConnection.Close();
        }
    }

    void ClearAdventurerTable()
    {
        IDbConnection dbConnection = CreateAndOpenAdventurersTable();
        IDbCommand dbCommandRemoveTable = dbConnection.CreateCommand();
        dbCommandRemoveTable.CommandText = "DROP TABLE Adventurers";
        dbCommandRemoveTable.ExecuteNonQuery();

        dbConnection.Close();
    }
    void ClearMissionTable()
    {
        IDbConnection dbConnection = CreateAndOpenAdventurersTable();
        IDbCommand dbCommandRemoveTable = dbConnection.CreateCommand();
        dbCommandRemoveTable.CommandText = "DROP TABLE Missions";
        dbCommandRemoveTable.ExecuteNonQuery();

        dbConnection.Close();
    }
    void ClearInventoryTable()
    {
        IDbConnection dbConnection = CreateAndOpenAdventurersTable();
        IDbCommand dbCommandRemoveTable = dbConnection.CreateCommand();
        dbCommandRemoveTable.CommandText = "DROP TABLE Inventory";
        dbCommandRemoveTable.ExecuteNonQuery();

        dbConnection.Close();
    }
}
