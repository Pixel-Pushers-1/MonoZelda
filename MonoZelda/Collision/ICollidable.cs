﻿using Microsoft.Xna.Framework;

namespace MonoZelda.Collision
{
    public interface ICollidable
    {
        CollidableType type { get; set; }
        Rectangle Bounds { get; set; }
        bool Intersects(ICollidable other);
        Rectangle GetIntersectionArea(ICollidable other);
    }
}
