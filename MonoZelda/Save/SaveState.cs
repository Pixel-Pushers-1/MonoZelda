using Microsoft.Xna.Framework;
using MonoZelda.Dungeons;
using MonoZelda.Dungeons.Parser.Data;
using MonoZelda.Link.Equippables;
using MonoZelda.Link.Projectiles;
using System;
using System.Collections.Generic;

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
        public bool HasCompass { get; set; }
        public bool HasMap { get; set; }
        public bool ObtainedTriforce { get; set; }
        public List<EquippableType> EquippableInventory { get; set; }

        public HashSet<(string, DoorDirection)> Keyring { get; set; }
        public HashSet<Point> DiscoveredRooms = new();

        // Dungeon state

        public string RoomName { get; set; }
        public Dictionary<string, DungeonRoom> Rooms { get; set; }
    }
}
