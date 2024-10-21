using Microsoft.Xna.Framework;
using MonoZelda.Sprites;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using MonoZelda.Collision;
using MonoZelda.Controllers;
using MonoZelda.Enemies.EnemyProjectiles;
using Microsoft.Xna.Framework.Graphics;

namespace MonoZelda.Enemies.EnemyClasses
{
    public class Aquamentus : IEnemy
    {
        public Point Pos { get; set; }
        public Collidable EnemyHitbox { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        private CardinalEnemyStateMachine stateMachine;
        private readonly Random rnd = new();
        private SpriteDict aquamentusSpriteDict;
        private readonly GraphicsDevice graphicsDevice;
        private CardinalEnemyStateMachine.Direction direction = CardinalEnemyStateMachine.Direction.Left;

        private List<IEnemyProjectile> fireballs = new();
        private Dictionary<IEnemyProjectile, EnemyProjectileCollision> projectileDictionary = new();
        private int midAngle = 180;

        private Point spawnPoint;
        private int pixelsMoved;
        private int tileSize = 64;
        private int moveDelay;
        private double attackDelay;
        private bool projectileActiveOrNot;

        public Aquamentus(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            projectileActiveOrNot = true;
            pixelsMoved = 0;
            moveDelay = rnd.Next(1, 4);
            attackDelay = 0;
            Width = 92;
            Height = 92;

        }

        public void EnemySpawn(SpriteDict enemyDict, Point spawnPosition, CollisionController collisionController,
            ContentManager contentManager)
        {
            spawnPoint = spawnPosition;
            EnemyHitbox = new Collidable(new Rectangle(spawnPosition.X, spawnPosition.Y, 60, 60), graphicsDevice, CollidableType.Enemy);
            collisionController.AddCollidable(EnemyHitbox);
            EnemyHitbox.setSpriteDict(enemyDict);
            enemyDict.Position = spawnPosition;
            enemyDict.SetSprite("aquamentus_left");
            aquamentusSpriteDict = enemyDict;
            Pos = spawnPosition;
            pixelsMoved = 0;
            stateMachine = new CardinalEnemyStateMachine();
            stateMachine.ChangeDirection(CardinalEnemyStateMachine.Direction.Left);
            fireballs.Add(new AquamentusFireball(spawnPosition, contentManager, graphicsDevice, collisionController, midAngle + 45));
            fireballs.Add(new AquamentusFireball(spawnPosition, contentManager, graphicsDevice, collisionController, midAngle));
            fireballs.Add(new AquamentusFireball(spawnPosition, contentManager, graphicsDevice, collisionController, midAngle - 45));
            foreach (var projectile in fireballs)
            {
                projectileDictionary.Add(projectile, new EnemyProjectileCollision(projectile, collisionController));
            }
            EnemyHitbox.setEnemy(this);
        }

        public void ChangeDirection()
        {
            switch (direction)
            {
                case CardinalEnemyStateMachine.Direction.Left:
                    direction = CardinalEnemyStateMachine.Direction.Right;
                    stateMachine.ChangeDirection(direction);
                    break;
                case CardinalEnemyStateMachine.Direction.Right:
                    direction = CardinalEnemyStateMachine.Direction.Left;
                    stateMachine.ChangeDirection(direction);
                    break;
            }
        }

        public void Attack(GameTime gameTime)
        {
            fireballs.ForEach(fireball => fireball.ViewProjectile(projectileActiveOrNot));
            fireballs.ForEach(fireball => fireball.Update(gameTime, CardinalEnemyStateMachine.Direction.Left, Pos));
            if (gameTime.TotalGameTime.TotalSeconds >= attackDelay + 4)
            {
                fireballs.ForEach(fireball => fireball.Follow(Pos));
                attackDelay = gameTime.TotalGameTime.TotalSeconds;
            }
        }

        public void Update(GameTime gameTime)
        {
            Pos = stateMachine.Update(Pos);
            aquamentusSpriteDict.Position = Pos;
            pixelsMoved++;
            if (Pos.X > spawnPoint.X || Pos.X < spawnPoint.X - tileSize*5)
            {
                ChangeDirection();
                pixelsMoved = 0;
            }
            else
            {
                if (pixelsMoved >= tileSize*moveDelay)
                {
                    pixelsMoved = 0;
                    ChangeDirection();
                    moveDelay = rnd.Next(1, 5);
                }
            }

            if (gameTime.TotalGameTime.TotalSeconds >= attackDelay)
            {
                if (gameTime.TotalGameTime.TotalSeconds <= attackDelay + 0.1)
                {
                    fireballs.ForEach(fireball => fireball.Follow(Pos));
                    aquamentusSpriteDict.SetSprite("aquamentus_left_mouthopen");
                }
                else if (gameTime.TotalGameTime.TotalSeconds >= attackDelay + 0.5)
                {
                    aquamentusSpriteDict.SetSprite("aquamentus_left");
                }

                Attack(gameTime);
            }

            foreach (KeyValuePair<IEnemyProjectile, EnemyProjectileCollision> entry in projectileDictionary)
            {
                entry.Value.Update();
            }
        }

        public void KillEnemy()
        {
            projectileActiveOrNot = false;
            aquamentusSpriteDict.Enabled = false;
            foreach(IEnemyProjectile projectile in fireballs)
            {
                projectile.ViewProjectile(projectileActiveOrNot);
            }
            EnemyHitbox.UnregisterHitbox();
        }
    }
}
