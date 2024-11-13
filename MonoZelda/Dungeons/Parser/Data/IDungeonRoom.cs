using Microsoft.Xna.Framework;
using MonoZelda.Trigger;
using System.Collections.Generic;

namespace MonoZelda.Dungeons
{
    public interface IDungeonRoom
    {
        string RoomName { get; }
        public Point SpawnPoint { get; set; }
        Dungeon1Sprite RoomSprite { get; }
        List<DoorSpawn> GetDoors();
        List<Rectangle> GetStaticRoomColliders();
        List<Rectangle> GetStaticBoundaryColliders();
        List<EnemySpawn> GetEnemySpawns();
        void Remove(EnemySpawn spawn);
        List<ItemSpawn> GetItemSpawns();
        List<TriggerSpawn> GetTriggers();
    }
}
