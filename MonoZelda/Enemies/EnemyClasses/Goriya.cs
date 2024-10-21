using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Collision;
using MonoZelda.Controllers;
using MonoZelda.Enemies.EnemyProjectiles;
using MonoZelda.Enemies.GoriyaFolder;
using MonoZelda.Sprites;

namespace MonoZelda.Enemies.EnemyClasses
{
    public class Goriya : IEnemy
    {
        public Point Pos { get; set; }
        public Collidable EnemyHitbox { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        private CardinalEnemyStateMachine stateMachine;
        private readonly Random rnd = new();
        private SpriteDict goriyaSpriteDict;
        private CardinalEnemyStateMachine.Direction direction;
        private readonly GraphicsDevice graphicsDevice;
        private IEnemyProjectile projectile;
        private EnemyProjectileCollision projectileCollision;
        private int pixelsMoved;
        private int tilesMoved;
        private int tileSize = 64;
        private bool projectileActiveOrNot;

        public Goriya(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            Width = 64;
            Height = 64;
            projectileActiveOrNot = true;
        }

        public void EnemySpawn(SpriteDict enemyDict, Point spawnPosition, CollisionController collisionController, ContentManager contentManager)
        {
            EnemyHitbox = new Collidable(new Rectangle(spawnPosition.X, spawnPosition.Y, 60, 60), graphicsDevice, CollidableType.Enemy);
            collisionController.AddCollidable(EnemyHitbox);
            EnemyHitbox.setSpriteDict(enemyDict);
            enemyDict.Position = spawnPosition;
            enemyDict.SetSprite("cloud");
            goriyaSpriteDict = enemyDict;
            Pos = spawnPosition;
            pixelsMoved = 0;
            tilesMoved = 0;
            stateMachine = new CardinalEnemyStateMachine();
            projectile = new GoriyaBoomerang(spawnPosition, contentManager, graphicsDevice, collisionController);
            projectileCollision = new EnemyProjectileCollision(projectile, collisionController);
            EnemyHitbox.setEnemy(this);
        }

        public void ChangeDirection()
        {
            switch (rnd.Next(1, 5))
            {
                case 1:
                    direction = CardinalEnemyStateMachine.Direction.Left;
                    goriyaSpriteDict.SetSprite("goriya_red_left");
                    break;
                case 2:
                    direction = CardinalEnemyStateMachine.Direction.Right;
                    goriyaSpriteDict.SetSprite("goriya_red_right");
                    break;
                case 3:
                    direction = CardinalEnemyStateMachine.Direction.Up;
                    goriyaSpriteDict.SetSprite("goriya_red_up");
                    break;
                case 4:
                    direction = CardinalEnemyStateMachine.Direction.Down;
                    goriyaSpriteDict.SetSprite("goriya_red_down");
                    break;
            }
            stateMachine.ChangeDirection(direction);
        }

        public void Attack(GameTime gameTime)
        {
            projectile.ViewProjectile(projectileActiveOrNot);
            projectile.Update(gameTime, direction, Pos);
            pixelsMoved += 4;
            if (pixelsMoved >= tileSize*6)
            {
                projectile.ViewProjectile(false);
                pixelsMoved = 0;
                tilesMoved = 0;
            }
        }

        public void Update(GameTime gameTime)
        {
            if (tilesMoved < 3)
            {
                if (pixelsMoved >= tileSize)
                {
                    pixelsMoved = 0;
                    tilesMoved++;
                    ChangeDirection();
                }

                pixelsMoved++;
                projectile.Follow(Pos);
                Pos = stateMachine.Update(Pos);
                goriyaSpriteDict.Position = Pos;
            }
            else
            {
                Attack(gameTime);
            }
            projectileCollision.Update();

        }
        public void KillEnemy()
        {
            projectileActiveOrNot = false;
            goriyaSpriteDict.Enabled = false;
            projectile.ViewProjectile(projectileActiveOrNot);
            EnemyHitbox.UnregisterHitbox();
        }
    }
}
