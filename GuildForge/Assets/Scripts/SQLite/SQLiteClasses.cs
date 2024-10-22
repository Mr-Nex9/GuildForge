using SQLite4Unity3d;

public class AdventurerDB
{
    [PrimaryKey]
    public int ID { get; set; }
    public bool Recruited { get; set; }
    public int OnMission { get; set; }
    public int EXP { get; set; }
    public int Health { get; set; }
    public int Mana { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
    public int Magic { get; set; }
    public int Speed { get; set; }
    public int WeaponID { get; set; }
    public int ArmorID { get; set; }
    public int Accessory1ID { get; set; }
    public int Accessory2ID { get; set; }
}
public class ItemDB
{
    [PrimaryKey]
    public int ID { get; set; }
    public int Location { get; set; }
}
public class GameStateDB
{
    [PrimaryKey]
    public int ID { get; set; }
    public long Amount { get; set; }

}
public class SettingDB
{
    [PrimaryKey]
    public int ID { get; set; }
    public int Setting { get; set; }
}
public class MissionDB
{
    [PrimaryKey]
    public int ID { get; set; }
    public int Status { get; set; }
    public long StartTime { get; set; }
    public int Rank { get; set; }
    public int Difficulty { get; set; }
}
