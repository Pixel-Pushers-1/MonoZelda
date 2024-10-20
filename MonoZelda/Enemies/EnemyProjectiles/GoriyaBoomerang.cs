using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Collision;
using MonoZelda.Controllers;
using MonoZelda.Enemies.EnemyProjectiles;
using MonoZelda.Sprites;

namespace MonoZelda.Enemies.GoriyaFolder
{
    public class GoriyaBoomerang : IEnemyProjectile
    {
        public Point Pos { get; set; }
        public SpriteDict BoomerangSpriteDict { get; private set; }
        private int speed = 4;
        private int pixelsMoved;
        public Collidable ProjectileHitbox { get; set; }

        public GoriyaBoomerang(Point pos, ContentManager contentManager, GraphicsDevice graphicsDevice, CollisionController collisionController)
        {
            this.Pos = pos;
            BoomerangSpriteDict = new(contentManager.Load<Texture2D>(TextureData.Enemies), SpriteCSVData.Enemies, 0, new Point(0, 0));
            BoomerangSpriteDict.SetSprite("boomerang");
            ViewProjectile(false);
            ProjectileHitbox = new Collidable(new Rectangle(pos.X, pos.Y, 30, 30), graphicsDevice, CollidableType.Projectile);
            collisionController.AddCollidable(ProjectileHitbox);
        }
        public void ViewProjectile(bool view)
        {
            BoomerangSpriteDict.Enabled = view;
        }

        public void Follow(Point newPos)
        {
            Pos = newPos;
            speed = Math.Abs(speed);
        }

        public void Update(GameTime gameTime, CardinalEnemyStateMachine.Direction attackDirection, Point enemyPos)
        {
            var pos = new Point();
            pos = Pos;
            if ((Math.Abs(enemyPos.X - pos.X) <= 192 && Math.Abs(enemyPos.Y - pos.Y) <= 192 )|| speed < 0)
            {
                switch (attackDirection)
                {
                    case CardinalEnemyStateMachine.Direction.Left:
                        pos.X -= speed;
                        break;
                    case CardinalEnemyStateMachine.Direction.Right:
                        pos.X += speed;
                        break;
                    case CardinalEnemyStateMachine.Direction.Up:
                        pos.Y -= speed;
                        break;
                    case CardinalEnemyStateMachine.Direction.Down:
                        pos.Y += speed;
                        break;
                }
            }
            else
            {
                speed *= -1;
            }

            Pos = pos;
            BoomerangSpriteDict.Position = Pos;
        }
    }
}