﻿using Microsoft.Xna.Framework;
using MonoZelda.Collision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoZelda.Dungeons.Parser
{
    internal class BoundaryCollisionCellParser : ICellParser
    {
        public void Parse(string cell, Point position, DungeonRoom room)
        {
            if (Enum.TryParse(cell, out CollisionTileRect collisionRect))
            {
                var rect = GetCollisionRectangle(collisionRect, position, DungeonConstants.TileWidth, DungeonConstants.TileHeight);
                room.AddStaticBoundaryCollider(rect);
            }
        }

        private Rectangle GetCollisionRectangle(CollisionTileRect collisionRect, Point position, int tileWidth, int tileHeight)
        {
            return collisionRect switch
            {
                CollisionTileRect.top => new Rectangle(position, new Point(tileWidth, tileHeight / 2)),
                CollisionTileRect.bottom => new Rectangle(new Point(position.X, position.Y + tileHeight / 2), new Point(tileWidth, tileHeight / 2)),
                CollisionTileRect.left => new Rectangle(position, new Point(tileWidth / 2, tileHeight)),
                CollisionTileRect.right => new Rectangle(new Point(position.X + tileWidth / 2, position.Y), new Point(tileWidth / 2, tileHeight)),
                CollisionTileRect.topleft => new Rectangle(position, new Point(tileWidth / 2, tileHeight / 2)),
                CollisionTileRect.topright => new Rectangle(new Point(position.X + tileWidth / 2, position.Y), new Point(tileWidth / 2, tileHeight / 2)),
                CollisionTileRect.bottomleft => new Rectangle(new Point(position.X, position.Y + tileHeight / 2), new Point(tileWidth / 2, tileHeight / 2)),
                CollisionTileRect.bottomright => new Rectangle(new Point(position.X + tileWidth / 2, position.Y + tileHeight / 2), new Point(tileWidth / 2, tileHeight / 2)),
                CollisionTileRect.full or _ => new Rectangle(position, new Point(tileWidth, tileHeight)),
            };
        }
    }



}
