using Microsoft.Xna.Framework;
using MonoZelda.Collision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoZelda.Dungeons.Parser
{
    internal class BoundaryCollisionCellParser : RoomCollisionCellParser
    {
        public new void Parse(string cell, Point position, DungeonRoom room)
        {
            if (Enum.TryParse(cell, out CollisionTileRect collisionRect))
            {
                var rect = GetCollisionRectangle(collisionRect, position, DungeonConstants.TileWidth, DungeonConstants.TileHeight);
                room.AddStaticBoundaryCollider(rect);
            }
        }
    }

}
