using Microsoft.Xna.Framework;
using MonoZelda.Link.Projectiles;
using System.Diagnostics;

namespace MonoZelda.Link
{
    public class PlayerState
    {
        private static readonly int INITIAL_HP = 12;
        private int _health = INITIAL_HP;

        public PlayerState()
        {

        }

        public int Health
        {
            get => _health;
            set
            {
                _health = value;
                if (_health <= 0)
                    IsDead = true;
            }
        }

        public void TakeDamage()
        {
            if (!IsDead)
            {
                this.Health = this.Health - 1;
            }
            Debug.WriteLine($"Player Health: {this.Health}");
        }

        public int Rupees { get; set; }
        public int Bombs { get; set; }
        public int Keys { get; set; }
        public bool IsDead { get; private set; }
        public bool IsKnockedBack { get; set; }
        public int MaxHealth { get; set; } = INITIAL_HP;
        public Point Position { get; set; }
        public ProjectileType EquippedProjectile { get; set; }

        public void AddRupees(int amount) => Rupees += amount;
        public void AddBombs(int amount) => Bombs += amount;
        public void AddKeys(int amount) => Keys += amount;
    }

}
