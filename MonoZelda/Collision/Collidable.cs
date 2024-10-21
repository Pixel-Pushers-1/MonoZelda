﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Enemies;
using MonoZelda.Link.Projectiles;
using MonoZelda.Sprites;

namespace MonoZelda.Collision
{
    public enum CollidableType {
        Player,
        Item,
        Enemy,
        Projectile,
        EnemyProjectile,
        Static
    }
    public class Collidable : ICollidable
    {
        public CollidableType type { get; set; }
        public Rectangle Bounds { get; set; }
        public SpriteDict CollidableDict { get; private set; }
        public IEnemy Enemy { get; private set; }   
        public ProjectileManager ProjectileManager { get; private set; }

        private readonly CollisionHitboxDraw hitbox;

        public Collidable(Rectangle bounds, GraphicsDevice graphicsDevice, CollidableType type)
        {
            Bounds = bounds;
            hitbox = new CollisionHitboxDraw(this, graphicsDevice);
            this.type = type;
        }

        public void UnregisterHitbox() {
            hitbox.Unregister();
        }

        public bool Intersects(ICollidable other)
        {
            return Bounds.Intersects(other.Bounds);
        }

        public Rectangle GetIntersectionArea(ICollidable other)
        {
            return Rectangle.Intersect(Bounds, other.Bounds);
        }

        public override string ToString()
        {
            return $"{type}";
        }

        public void setSpriteDict(SpriteDict collidableDict)
        {
            CollidableDict = collidableDict;
        }

        public void setEnemy(IEnemy enemy)
        {
            Enemy = enemy;
        }
        public void setProjectileManager(ProjectileManager projectileManager)
        {
            ProjectileManager = projectileManager;
        }
    }
}
