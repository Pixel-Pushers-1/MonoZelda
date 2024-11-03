using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MonoZelda.Link
{
    public class PlayerState
    {
        private int health;
        private int rupees;
        private int bombs;
        private int keys;
        private bool isDead;
        private bool isKnockedBack;
        private PlayerSpriteManager player;
        public PlayerState(PlayerSpriteManager player)
        {
            health = 1;
            rupees = 0;
            bombs = 0;
            keys = 0;
            isDead = false;
            isKnockedBack = false;
            this.player = player;
        }

        public int GetHealth() => health;

        public void SetHealth(int newHealth)
        {
            health = newHealth;
            if (health <= 0)
                isDead = true;
                player.PlayerDeath();
            
                
        }

        public int GetRupees() => rupees;

        public void AddRupees(int amount) => rupees += amount;

        public int GetBombs() => bombs;

        public void AddBombs(int amount) => bombs += amount;

        public int GetKeys() => keys;

        public void AddKeys(int amount) => keys += amount;

        public bool IsDead() => isDead;
        
    }

}
