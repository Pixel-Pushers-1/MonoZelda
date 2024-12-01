using Microsoft.Xna.Framework;
using MonoZelda.Dungeons.Loader;
using MonoZelda.Dungeons.Parser.Data;
using MonoZelda.Items;
using System;

namespace MonoZelda.Dungeons.Parser
{
    internal class ItemCellParser : ICellParser
    {
        public void Parse(string cell, Point position, DungeonRoom room)
        {
            // Load item
            if (Enum.TryParse(cell, out ItemList item))
            {
                var itemSpawn = new ItemSpawn(position, item);
                var nonColliderSpawn = new NonColliderSpawn(position);
                room.AddItemSpawn(itemSpawn);
                room.AddNonColliderSpawn(nonColliderSpawn);
            }
        }
    }
}
