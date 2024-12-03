using Microsoft.Xna.Framework;
using MonoZelda.Dungeons.Parser.Data;
using MonoZelda.Save;
using MonoZelda.Link.Equippables;
using System.Collections.Generic;
using MonoZelda.Commands.GameCommands;
using MonoZelda.Link.Projectiles;
using MonoZelda.Sound;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MonoZelda.Link;

public static class PlayerState
{
    private static readonly int INITIAL_HP = 6;
    private static readonly int INITIAL_RUPEES = 3;
    private static readonly int INITIAL_BOMBS = 0;
    private static readonly int INITIAL_KEYS = 0;
    private static int _health = INITIAL_HP;

    //RPG
    private static readonly int INITIAL_LEVEL = 1;
    private static readonly int XP_BASE = 100; 
    private static readonly float XP_SCALING = 1.3f;
    private static readonly float INITIAL_DEFENSE = 0f;


    public static List<EquippableType> EquippableInventory;

    // (RoomName, Direction)
    public static HashSet<(string, DoorDirection)> Keyring { get; set; } = new ();
    public static HashSet<Point> DiscoveredRooms { get; set; } = new();

    public static void Initialize()
    {
        Direction = Direction.Down;
        Position = new Point(515, 725);
        IsCandleUsed = false;
        IsDead = false;
        IsKnockedBack = false;
        _health = INITIAL_HP;
        MaxHealth = INITIAL_HP;
        Rupees = INITIAL_RUPEES;
        Bombs = INITIAL_BOMBS;
        Keys = INITIAL_KEYS;
        HasMap = false;
        HasCompass = false;
        Keyring = new();
        DiscoveredRooms = new();
        Level = INITIAL_LEVEL;
        Defense = INITIAL_DEFENSE;
        XP = 0;
    }

    public static void ResetCandle()
    {
        IsCandleUsed = false;
    }

    public static float Health
    {
        get => _health;
        set
        {
            _health = (int)value;
            if (_health <= 0)
                IsDead = true;
        }
    }

    public static bool IsMaxHealth()
    {
        return Health == INITIAL_HP;
    }

    public static void TakeDamage()
    {
        if (!IsDead)
        {
            float effectiveDamage = 1 / (1 + (float)Math.Log(1 + Defense));
            Health -= effectiveDamage;
            Debug.WriteLine($"Effective Damage Taken: {effectiveDamage}, Defense: {Defense}");
        }
        Debug.WriteLine($"Player Health: {Health}");


    }

    public static void GetHealth() {
        Health = MathHelper.Clamp(Health + 2, 0, INITIAL_HP);
    }

    public static int Rupees { get; set; }
    public static int Bombs { get; set; }
    public static int Keys { get; set; }
    public static bool IsDead { get; set; }
    public static bool IsKnockedBack { get; set; }
    public static bool IsCandleUsed { get; set; }   
    public static int MaxHealth { get; set; }
    public static Point Position { get; set; }
    public static Direction Direction { get; set; }
    public static EquippableType EquippedItem { get; set; }
    public static bool HasBoomerang { get; set; }
    public static bool ObtainedTriforce { get; set; }
    public static bool HasCompass;
    public static bool HasMap;

    // RPG 
    public static int Level { get; private set; }
    public static int XP { get; private set; }
    public static float Defense { get; private set; }

    public static int GetXPToLevelUp()
    {
        return (int)(XP_BASE * Math.Pow(XP_SCALING, Level - 1));
    }

    public static bool AddXP(int amount)
    {
        bool leveledUp = false;
        XP += amount;
        // does player have enough xp to level up?
        while (XP >= GetXPToLevelUp()) 
        {
            XP -= GetXPToLevelUp(); 
            Level++;
            leveledUp |= true;
            Health = MathHelper.Clamp(Health + 1, 0, MaxHealth);
            SoundManager.PlaySound("LOZ_LevelUp", false);
            Defense += .50f;

        }
        return leveledUp;
    }
    public static float GetXPProgress()
    {
        int xpToLevelUp = GetXPToLevelUp();
        return (float)XP / xpToLevelUp;
    }
    public static void AddRupees(int amount) => Rupees += amount;
    public static void AddBombs(int amount) => Bombs += amount;
    public static void AddKeys(int amount) => Keys += amount;

    public static void Save(SaveState save)
    {
        save.MaxHealth = MaxHealth;
        save.EquippableInventory = EquippableInventory;
        save.HasBoomerang = HasBoomerang;
        save.ObtainedTriforce = ObtainedTriforce;
        save.Health = (int)Health;
        save.BombCount = Bombs;
        save.RupeeCount = Rupees;
        save.KeyCount = Keys;
        save.HasCompass = HasCompass;
        save.HasMap = HasMap;
        save.Keyring = Keyring;
        save.DiscoveredRooms = DiscoveredRooms;
    }

    public static void Load(SaveState save)
    {
        MaxHealth = save.MaxHealth;
        EquippableInventory = save.EquippableInventory;
        HasBoomerang = save.HasBoomerang;
        ObtainedTriforce = save.ObtainedTriforce;
        Health = save.Health;
        Bombs = save.BombCount;
        Rupees = save.RupeeCount;
        Keys = save.KeyCount;
        HasCompass = save.HasCompass;
        HasMap = save.HasMap;
        Keyring = save.Keyring;
        DiscoveredRooms = save.DiscoveredRooms;
    }
}
