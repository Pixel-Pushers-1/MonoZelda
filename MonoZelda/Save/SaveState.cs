using MonoZelda.Dungeons;
using MonoZelda.Dungeons.Parser.Data;
using MonoZelda.Link;
using MonoZelda.Link.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoZelda.Save
{
    [Serializable]
    public class SaveState
    {
        // Link state

        public int Health { get; set; }
        public int BombCount { get; set; }
        public int RupeeCount { get; set; }
        public int KeyCount { get; set; }
        public int MaxHealth { get; set; }
        public bool HasBoomerang { get; set; }
        public bool ObtainedTriforce { get; set; }
        public HashSet<(string, DoorDirection)> Keyring { get; set; }

        public ProjectileType EquipedProjectile { get; set; }

        // Dungeon state

        public string RoomName { get; set; }
        public Dictionary<string, DungeonRoom> Rooms { get; set; }
    }
}
