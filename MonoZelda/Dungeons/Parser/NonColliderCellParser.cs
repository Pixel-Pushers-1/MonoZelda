using Microsoft.Xna.Framework;
using MonoZelda.Dungeons.Parser.Data;
using MonoZelda.Items;
using System;

namespace MonoZelda.Dungeons.Parser
{
    internal class NonColliderCellParser : ICellParser
    {
        public void Parse(string cell, Point position, DungeonRoom room)
        {
            var nonColliderSpawn = new NonColliderSpawn(position);
            room.AddNonColliderSpawn(nonColliderSpawn);
        }
    }
}
