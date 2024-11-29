using Microsoft.Xna.Framework;
using MonoZelda.Commands.GameCommands;
using MonoZelda.Dungeons.Parser.Data;
using MonoZelda.Link.Projectiles;
using MonoZelda.Save;
using MonoZelda.Link.Equippables;
using System.Collections.Generic;
using MonoZelda.Dungeons.Parser.Data;
using System.Diagnostics;

namespace MonoZelda.Link;

public static class PlayerState
{
    private static readonly int INITIAL_HP = 6;
    private static readonly int INITIAL_RUPEES = 3;
    private static readonly int INITIAL_BOMBS = 0;
    private static readonly int INITIAL_KEYS = 0;

    private static int _health = INITIAL_HP;
    

    // (RoomName, Direction)
    public static HashSet<(string, DoorDirection)> Keyring { get; set; } = new ();

    public static HashSet<Point> DiscoveredRooms { get; set; } = new();



    public static List<EquippableType> EquippableInventory;

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
        EquippedProjectile = ProjectileType.None;
        HasMap = false;
        HasCompass = false;
        Keyring = new();
        DiscoveredRooms = new();
        EquippedItem = EquippableType.None;
        EquippableInventory = new List<EquippableType>();
    }

    public static void ResetCandle()
    {
        IsCandleUsed = false;
    }

    public static int Health
    {
        get => _health;
        set
        {
            _health = value;
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
            Health = Health - 1;
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

    public static void AddRupees(int amount) => Rupees += amount;
    public static void AddBombs(int amount) => Bombs += amount;
    public static void AddKeys(int amount) => Keys += amount;

    public static void Save(SaveState save)
    {
        save.MaxHealth = MaxHealth;
        save.EquipedProjectile = EquippedProjectile;
        save.HasBoomerang = HasBoomerang;
        save.ObtainedTriforce = ObtainedTriforce;
        save.Health = Health;
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
        EquippedProjectile = save.EquipedProjectile;
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
