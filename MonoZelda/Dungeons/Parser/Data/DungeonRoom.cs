using Microsoft.Xna.Framework;
using MonoZelda.Dungeons.Parser.Data;
using MonoZelda.Trigger;
using System;
using System.Collections.Generic;

namespace MonoZelda.Dungeons
{
    [Serializable]
    public class DungeonRoom : IDungeonRoom
    {
        public List<DoorSpawn> doors { get; set; }
        public List<Rectangle> roomColliders { get; set; }
        public List<Rectangle> boundaryColliders { get; set; }
        public List<NonColliderSpawn> nonColliderSpawns { get; set; }
        public List<ItemSpawn> itemSpawns { get; set; }
        public List<EnemySpawn> enemySpawns { get; set; }
        public List<TriggerSpawn> triggers { get; set; }

        public string RoomName { get; private set; }
        public Dungeon1Sprite RoomSprite { get; private set; }
        public Point SpawnPoint { get; set; }

        public DungeonRoom(string roomName, Dungeon1Sprite roomSprite)
        {
            RoomName = roomName;
            RoomSprite = roomSprite;

            doors = new List<DoorSpawn>();
            roomColliders = new List<Rectangle>();
            boundaryColliders = new List<Rectangle>();
            nonColliderSpawns = new List<NonColliderSpawn>();
            itemSpawns = new List<ItemSpawn>();
            enemySpawns = new List<EnemySpawn>();
            triggers = new List<TriggerSpawn>();
        }

        public List<DoorSpawn> GetDoors()
        {
            return doors;
        }

        public void AddDoor(DoorSpawn door)
        {
            doors.Add(door);
        }

        public void AddTrigger(TriggerSpawn trigger)
        {
            triggers.Add(trigger);
        }

        public void AddNonColliderSpawn(NonColliderSpawn nonColliderSpawn)
        {
            nonColliderSpawns.Add(nonColliderSpawn);    
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

        public void Remove(EnemySpawn spawn)
        {
            enemySpawns.Remove(spawn);
        }

        public List<ItemSpawn> GetItemSpawns()
        {
            return itemSpawns;
        }

        public List<NonColliderSpawn> GetNonColliderSpawns()
        {
            return nonColliderSpawns;
        }

        public List<TriggerSpawn> GetTriggers()
        {
            return triggers;
        }
    }
}
