using System;
using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using MonoZelda.Collision;
using MonoZelda.Controllers;
using System.Diagnostics.SymbolStore;

namespace MonoZelda.Enemies.EnemyProjectiles
{
    public class AquamentusFireball : IEnemyProjectile
    {
        public EnemyProjectileCollidable ProjectileHitbox { get; set; }
        public Point Pos { get; set; }

        private CollisionController collisionController;
        public SpriteDict FireballSpriteDict { get; private set; }

        private int speed = 4;
        private double angle;

        public AquamentusFireball(Point pos, ContentManager contentManager, GraphicsDevice graphicsDevice, CollisionController collisionController, int newAngle)
        {
            Pos = pos;
            FireballSpriteDict = new(contentManager.Load<Texture2D>("Sprites/enemies"), SpriteCSVData.Enemies, 0, new Point(100, 100));
            FireballSpriteDict.SetSprite("fireball");
            ProjectileHitbox = new EnemyProjectileCollidable(new Rectangle(pos.X, pos.Y, 30, 30), graphicsDevice);
            collisionController.AddCollidable(ProjectileHitbox);
            angle = newAngle;
            if (angle <= 180)
            {
                if (angle <= 90)
                {
                    angle = angle / 180 - 2;
                }
                else
                {
                    angle = angle / 180 - 1;
                }
            }
            else
            {
                angle /= 180;
            }
            this.collisionController = collisionController; 
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

        public void Follow(Point newPos)
        {
            Point pos = Pos;
            pos.X = newPos.X;
            pos.Y = newPos.Y - 48;
            Pos = pos;
        }

        public void ProjectileCollide()
        {
            FireballSpriteDict.Enabled = false;
            collisionController.RemoveCollidable(ProjectileHitbox);
            ProjectileHitbox.UnregisterHitbox();
        }

        public void Update(GameTime gameTime, EnemyStateMachine.Direction attackDirection, Point enemyPos)
        {
            Point pos = Pos;
            pos.X -= speed;
            if (angle >= 1.5)
            {
                angle = Math.Ceiling(angle);
            }
            else
            {
                angle = Math.Floor(angle);
            }

            pos.Y -= (int)angle;
            Pos = pos;
            FireballSpriteDict.Position = Pos;
        }
    }
}
