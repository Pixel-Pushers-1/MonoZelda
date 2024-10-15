using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using PixelPushers.MonoZelda;
using PixelPushers.MonoZelda.Sprites;

namespace MonoZelda.Enemies.GoriyaFolder
{
    public class Goriya : IEnemy
    {
        private readonly GoriyaStateMachine stateMachine;
        private Point pos;
        private readonly Random rnd = new();
        private SpriteDict goriyaSpriteDict;
        private GoriyaStateMachine.Direction direction = GoriyaStateMachine.Direction.Left;
        private readonly GraphicsDeviceManager graphics;
        private readonly int spawnX;
        private readonly int spawnY;
        private bool spawning;

        private GoriyaBoomerang boomerang;

        private double startTime;
        private double attackDelay;

        public Goriya(SpriteDict spriteDict, GraphicsDeviceManager graphics, ContentManager contentManager)
        {
            this.graphics = graphics;
            this.goriyaSpriteDict = spriteDict;
            stateMachine = new GoriyaStateMachine();
            spawnX = 3 * graphics.PreferredBackBufferWidth / 5;
            spawnY = 3 * graphics.PreferredBackBufferHeight / 5;
            pos = new(spawnX, spawnY);
            boomerang = new GoriyaBoomerang(pos, contentManager);
            boomerang.BoomerangSpriteDict.Enabled = false;
        }


        public void SetOgPos(GameTime gameTime) //sets to spawn position (eventually could be used for re-entering rooms)
        {
            pos.X = spawnX;
            pos.Y = spawnY;
            goriyaSpriteDict.Position = pos;
            goriyaSpriteDict.SetSprite("cloud");
            spawning = true;
            attackDelay = gameTime.TotalGameTime.TotalSeconds;
            startTime = gameTime.TotalGameTime.TotalSeconds;
        }

        public void DisableProjectile()
        {
            boomerang.BoomerangSpriteDict.Enabled = false;
        }

        public void ChangeDirection()
        {
            switch (rnd.Next(1, 5))
            {
                case 1:
                    direction = GoriyaStateMachine.Direction.Left;
                    break;
                case 2:
                    direction = GoriyaStateMachine.Direction.Right;
                    break;
                case 3:
                    direction = GoriyaStateMachine.Direction.Up;
                    break;
                case 4:
                    direction = GoriyaStateMachine.Direction.Down;
                    break;
            }
            stateMachine.ChangeDirection(direction);
        }

        public void Attack(GameTime gameTime)
        {
            boomerang.BoomerangSpriteDict.Enabled = true;
            boomerang.Update(gameTime,direction,attackDelay);
            if (gameTime.TotalGameTime.TotalSeconds >= attackDelay + 5)
            {
                attackDelay = gameTime.TotalGameTime.TotalSeconds;
                boomerang.BoomerangSpriteDict.Enabled = false;
            }
        }

        public void Update(GameTime gameTime) //might eventually split this into multiple methods, controlling random movement is more extensive than I thought.
        {
            if (spawning)
            {
                if (gameTime.TotalGameTime.TotalSeconds >= startTime + 0.3)
                {
                    startTime = gameTime.TotalGameTime.TotalSeconds;
                    spawning = false;
                    stateMachine.UpdateSprite(goriyaSpriteDict);
                }
            }
            else if (gameTime.TotalGameTime.TotalSeconds < attackDelay + 3)
            {
                if (gameTime.TotalGameTime.TotalSeconds >= startTime + 1)
                {
                    ChangeDirection();
                    startTime = gameTime.TotalGameTime.TotalSeconds;
                    stateMachine.UpdateSprite(goriyaSpriteDict);
                }
                boomerang.Follow(pos);
                pos = stateMachine.Update(pos, goriyaSpriteDict, graphics);
                goriyaSpriteDict.Position = pos;
            }
            else
            {
                Attack(gameTime);
            }
        }
    }
}
