using Microsoft.Xna.Framework;
using MonoZelda.Sprites;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using MonoZelda.Collision;
using MonoZelda.Controllers;
using MonoZelda.Enemies.AquamentusFolder;

namespace MonoZelda.Enemies.EnemyClasses
{
    public class Aquamentus : IEnemy
    {
        private readonly AquamentusStateMachine stateMachine;
        private Point pos;
        private readonly Random rnd = new();
        private readonly SpriteDict aquamentusSpriteDict;
        private readonly GraphicsDeviceManager graphics;
        private readonly int spawnX;
        private readonly int spawnY;
        private bool spawning;

        private List<AquamentusFireball> fireballs = new();
        private int midAngle = 180;

        private double startTime;
        private double attackDelay;
        private int moveDelay;

        public Aquamentus(SpriteDict spriteDict, GraphicsDeviceManager graphics, ContentManager contentManager)
        {
            aquamentusSpriteDict = spriteDict;
            stateMachine = new AquamentusStateMachine();
            this.graphics = graphics;
            spawnX = 3 * graphics.PreferredBackBufferWidth / 5;
            spawnY = 3 * graphics.PreferredBackBufferHeight / 5;
            pos = new(spawnX, spawnY);
            moveDelay = rnd.Next(1, 4);
            fireballs.Add(new AquamentusFireball(pos, contentManager, midAngle + 45));
            fireballs.Add(new AquamentusFireball(pos, contentManager, midAngle));
            fireballs.Add(new AquamentusFireball(pos, contentManager, midAngle - 45));
            fireballs.ForEach(fireball => fireball.FireballSpriteDict.Enabled = false);

        }

        public Point Pos { get; set; }
        public Collidable EnemyHitbox { get; set; }

        public void SetOgPos(GameTime gameTime)
        {
            pos.X = spawnX;
            pos.Y = spawnY;
            aquamentusSpriteDict.Position = pos;
            aquamentusSpriteDict.SetSprite("cloud");
            spawning = true;
            startTime = gameTime.TotalGameTime.TotalSeconds;
        }

        public void EnemySpawn(SpriteDict enemyDict, Point spawnPosition, CollisionController collisionController)
        {
            throw new NotImplementedException();
        }

        public void DisableProjectile()
        {
            fireballs.ForEach(fireball => fireball.FireballSpriteDict.Enabled = false);
            fireballs.ForEach(fireball => fireball.Follow(pos));
        }

        public void ChangeDirection()
        {
            stateMachine.ChangeDirection();
        }

        public void Attack(GameTime gameTime)
        {
            fireballs.ForEach(fireball => fireball.FireballSpriteDict.Enabled = true);
            fireballs.ForEach(fireball => fireball.Update());
            if (gameTime.TotalGameTime.TotalSeconds >= attackDelay + 2)
            {
                fireballs.ForEach(fireball => fireball.Follow(pos));
                attackDelay = gameTime.TotalGameTime.TotalSeconds;
            }
        }

        public void Update(GameTime gameTime)
        {
            if (spawning)
            {
                if (gameTime.TotalGameTime.TotalSeconds >= startTime + 0.3)
                {
                    startTime = gameTime.TotalGameTime.TotalSeconds;
                    attackDelay = gameTime.TotalGameTime.TotalSeconds + 2;
                    spawning = false;
                    aquamentusSpriteDict.SetSprite("aquamentus_left");
                    fireballs.ForEach(fireball => fireball.Follow(pos));
                }
            }
            else
            {
                if (pos.X > spawnX || pos.X < spawnX - 150)
                {
                    ChangeDirection();
                }
                else
                {
                    if (gameTime.TotalGameTime.TotalSeconds >= startTime + moveDelay)
                    {
                        ChangeDirection();
                        startTime = gameTime.TotalGameTime.TotalSeconds;
                        moveDelay = rnd.Next(1, 5);
                    }
                }

                pos = stateMachine.Update(pos);
                aquamentusSpriteDict.Position = pos;

                if (gameTime.TotalGameTime.TotalSeconds >= attackDelay)
                {
                    if (gameTime.TotalGameTime.TotalSeconds <= attackDelay + 0.1)
                    {
                        fireballs.ForEach(fireball => fireball.Follow(pos));
                        aquamentusSpriteDict.SetSprite("aquamentus_left_mouthopen");
                    }
                    else if (gameTime.TotalGameTime.TotalSeconds >= attackDelay + 0.5)
                    {
                        aquamentusSpriteDict.SetSprite("aquamentus_left");
                    }

                    Attack(gameTime);
                }
            }
        }
    }
}
