using Microsoft.Xna.Framework;
using MonoZelda.Trigger;
using System.Collections.Generic;

namespace MonoZelda.Dungeons
{
    public interface IDungeonRoom
    {
        string RoomName { get; }
        Dungeon1Sprite RoomSprite { get; }
        List<IDoor> GetDoors();
        List<Rectangle> GetStaticRoomColliders();
        List<Rectangle> GetStaticBoundaryColliders();
        List<EnemySpawn> GetEnemySpawns();
        List<ItemSpawn> GetItemSpawns();
        List<TriggerSpawn> GetTriggers();
    }
}
