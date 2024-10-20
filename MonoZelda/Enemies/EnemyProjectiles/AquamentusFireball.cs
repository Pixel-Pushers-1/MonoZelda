using System;
using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using MonoZelda.Collision;
using MonoZelda.Controllers;

namespace MonoZelda.Enemies.EnemyProjectiles
{
    public class AquamentusFireball : IEnemyProjectile
    {
        public Collidable ProjectileHitbox { get; set; }
        public Point Pos { get; set; }

        public SpriteDict FireballSpriteDict { get; private set; }

        private int speed = 4;
        private double angle;

        public AquamentusFireball(Point pos, ContentManager contentManager, GraphicsDevice graphicsDevice, CollisionController collisionController, int newAngle)
        {
            Pos = pos;
            FireballSpriteDict = new(contentManager.Load<Texture2D>("Sprites/enemies"), SpriteCSVData.Enemies, 0, new Point(100, 100));
            FireballSpriteDict.SetSprite("fireball");
            ProjectileHitbox = new Collidable(new Rectangle(pos.X, pos.Y, 30, 30), graphicsDevice, CollidableType.Projectile);
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
        }

        public void ViewProjectile(bool view)
        {
            FireballSpriteDict.Enabled = view;
        }

        public void Follow(Point newPos)
        {
            Point pos = Pos;
            pos.X = newPos.X;
            pos.Y = newPos.Y - 48;
            Pos = pos;
        }

        public void Update(GameTime gameTime, CardinalEnemyStateMachine.Direction attackDirection, Point enemyPos)
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
