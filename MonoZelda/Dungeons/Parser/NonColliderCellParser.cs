using Microsoft.Xna.Framework;
using MonoZelda.Dungeons.Parser.Data;
using MonoZelda.Items;
using System;

namespace MonoZelda.Dungeons.Parser
{
    internal class NonColliderCellParser : ICellParser
    {
        private static readonly Rectangle NO_SPAWN_ZONE = new Rectangle(new Point(128, 320), new Point(128, 448));

        public void Parse(string cell, Point position, DungeonRoom room)
        {
            if(NO_SPAWN_ZONE.Contains(position) == false)
            {
                var nonColliderSpawn = new NonColliderSpawn(position);
                room.AddNonColliderSpawn(nonColliderSpawn);
            }
        }
    }
}
