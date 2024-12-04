using Microsoft.Xna.Framework;
using MonoZelda.Dungeons.Parser.Data;
using MonoZelda.Trigger;
using System.Collections.Generic;

namespace MonoZelda.Dungeons
{
    public interface IDungeonRoom
    {
        string RoomName { get; }
        public Point SpawnPoint { get; set; }
        public bool IsLit { get; set; }
        Dungeon1Sprite RoomSprite { get; }
        List<DoorSpawn> GetDoors();
        List<Rectangle> GetStaticRoomColliders();
        List<Rectangle> GetStaticBoundaryColliders();
        List<EnemySpawn> GetEnemySpawns();
        void Remove(EnemySpawn spawn);
        List<NonColliderSpawn> GetNonColliderSpawns();
        void Remove(NonColliderSpawn spawn);
        List<ItemSpawn> GetItemSpawns();
        List<TriggerSpawn> GetTriggers();
    }
}
