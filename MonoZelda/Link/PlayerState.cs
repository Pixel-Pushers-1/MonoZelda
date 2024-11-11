using Microsoft.Xna.Framework;
using MonoZelda.Link.Projectiles;
using System.Diagnostics;

namespace MonoZelda.Link;

public static class PlayerState
{
    private static readonly int INITIAL_HP = 6;
    private static int _health = INITIAL_HP;

    public static void Initialize()
    {
        Direction = Direction.Down;
        Position = new Point(500, 700);
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

    public static void TakeDamage()
    {
        if (!IsDead)
        {
            Health = Health - 1;
        }
        Debug.WriteLine($"Player Health: {Health}");
    }

    public static int Rupees { get; set; }
    public static int Bombs { get; set; }
    public static int Keys { get; set; } = 1;
    public static bool IsDead { get; private set; }
    public static bool IsKnockedBack { get; set; }
    public static int MaxHealth { get; set; } = INITIAL_HP;
    public static Point Position { get; set; }
    public static Direction Direction { get; set; }
    public static ProjectileType EquippedProjectile { get; set; }

    public static void AddRupees(int amount) => Rupees += amount;
    public static void AddBombs(int amount) => Bombs += amount;
    public static void AddKeys(int amount) => Keys += amount;
}
