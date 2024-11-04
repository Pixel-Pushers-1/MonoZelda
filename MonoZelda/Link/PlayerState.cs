using MonoZelda.Commands.GameCommands;
using MonoZelda.HUD;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MonoZelda.Link
{
    public class PlayerState
    {
        private PlayerSpriteManager playerSpriteManager;
        private int _health;
        private HUDManager _hudManager;

        public PlayerState(PlayerSpriteManager playerSpriteManager)
        {
            Health = 6;
            Rupees = 0;
            Bombs = 0;
            Keys = 0;
            IsDead = false;
            IsKnockedBack = false;
            this.playerSpriteManager = playerSpriteManager;
        }

        public HUDManager HUDManager
        {
            get => _hudManager;
            set => _hudManager = value;
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
            this.Health = this.Health - 1;
            Debug.WriteLine($"Player Health: {this.Health}");
            HUDManager.UpdateHeartDisplay();  // Using null conditional operator
            playerSpriteManager.TakeDamage();
        }

        public int Rupees { get; set; }
        public int Bombs { get; set; }
        public int Keys { get; set; }
        public bool IsDead { get; private set; }
        public bool IsKnockedBack { get; set; }

        public void AddRupees(int amount) => Rupees += amount;
        public void AddBombs(int amount) => Bombs += amount;
        public void AddKeys(int amount) => Keys += amount;
    }

}
