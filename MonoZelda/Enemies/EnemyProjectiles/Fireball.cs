﻿using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Collision;
using MonoZelda.Controllers;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace MonoZelda.Enemies.EnemyProjectiles
{
    public class Fireball : IEnemyProjectile
    {
        public EnemyProjectileCollidable ProjectileHitbox { get; set; }
        public Point Pos { get; set; }

        private CollisionController collisionController;
        public SpriteDict FireballSpriteDict { get; private set; }

        public bool Active { get; set; }
        private Vector2 move;

        public Fireball(Point pos, CollisionController collisionController, Vector2 C)
        {
            Pos = pos;
            FireballSpriteDict = new(SpriteType.Enemies, 0, new Point(100, 100));
            FireballSpriteDict.SetSprite("fireball");
            ProjectileHitbox = new EnemyProjectileCollidable(new Rectangle(pos.X, pos.Y, 30, 30));
            collisionController.AddCollidable(ProjectileHitbox);
            move = C;
            this.collisionController = collisionController;

            Active = true;
        }

        public void ViewProjectile(bool view, bool aquamentusAlive)
        {
            FireballSpriteDict.Enabled = view;
            if(aquamentusAlive == false)
            {
                collisionController.RemoveCollidable(ProjectileHitbox);
                ProjectileHitbox.UnregisterHitbox();
            }
        }
        public void ProjectileCollide()
        {
            FireballSpriteDict.Enabled = false;
            Active = false;
            collisionController.RemoveCollidable(ProjectileHitbox);
            ProjectileHitbox.UnregisterHitbox();
        }

        public void Update(EnemyStateMachine.Direction attackDirection, Point enemyPos)
        {
            Pos = Pos + move.ToPoint();
            FireballSpriteDict.Position = Pos;
        }
    }
}
