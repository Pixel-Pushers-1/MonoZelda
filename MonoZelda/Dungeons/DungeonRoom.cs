using Microsoft.Xna.Framework;
using MonoZelda.Trigger;
using System;
using System.Collections.Generic;

namespace MonoZelda.Dungeons
{
    internal class DungeonRoom : IDungeonRoom
    {
        private List<IDoor> doors;
        private List<Rectangle> roomColliders;
        private List<Rectangle> boundaryColliders;        
        private List<ItemSpawn> itemSpawns;
        private List<EnemySpawn> enemySpawns;
        private List<TriggerSpawn> triggers;

        public string RoomName { get; private set; }
        public Dungeon1Sprite RoomSprite { get; private set; }

        public DungeonRoom(string name, Dungeon1Sprite roomSprite, List<IDoor> doors)
        {
            this.doors = doors;
            RoomName = name;
            RoomSprite = roomSprite;

            roomColliders = new List<Rectangle>();
            boundaryColliders = new List<Rectangle>();
            itemSpawns = new List<ItemSpawn>();
            enemySpawns = new List<EnemySpawn>();
            triggers = new List<TriggerSpawn>();
        }

        public List<IDoor> GetDoors()
        {
            return doors;
        }

        public void AddTrigger(TriggerSpawn trigger)
        {
            triggers.Add(trigger);
        }

        public void AddItemSpawn(ItemSpawn itemSpawn)
        {
            itemSpawns.Add(itemSpawn);
        }

        public void AddEnemySpawn(EnemySpawn enemySpawn)
        {
            enemySpawns.Add(enemySpawn);
        }

        public void AddStaticRoomCollider(Rectangle roomCollider)
        {
            roomColliders.Add(roomCollider);
        }

        public void AddStaticBoundaryCollider(Rectangle boundaryCollider)
        {
            boundaryColliders.Add(boundaryCollider);
        }

        public List<Rectangle> GetStaticRoomColliders()
        {
            return roomColliders;
        }

        public List<Rectangle> GetStaticBoundaryColliders()
        {
            return boundaryColliders;
        }

        public List<EnemySpawn> GetEnemySpawns()
        {
            return enemySpawns;
        }

        public List<ItemSpawn> GetItemSpawns()
        {
            return itemSpawns;
        }

        public List<TriggerSpawn> GetTriggers()
        {
            return triggers;
        }
    }
}
