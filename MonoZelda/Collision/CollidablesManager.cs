using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using PixelPushers.MonoZelda.Sprites;
using System;

namespace MonoZelda.Collision
{
    public class CollidablesManager
    {
        private readonly List<Collidable> collidables;

        public CollidablesManager()
        {
            collidables = new List<Collidable>();
        }

        // Add a new hitbox
        public void AddHitbox(Collidable collidable)
        {
            collidables.Add(collidable);
        }

        // Remove a hitbox
        public void RemoveHitbox(Collidable collidable)
        {
            collidables.Remove(collidable);
        }

        // Get all hitboxes
        public List<Collidable> GetHitboxes()
        {
            return collidables;
        }

        internal void Clear()
        {
            collidables.Clear();
        }
    }
}
