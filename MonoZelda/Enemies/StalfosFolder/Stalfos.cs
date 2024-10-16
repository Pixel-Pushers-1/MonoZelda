using System;
using Microsoft.Xna.Framework;
using MonoZelda.Sprites;

namespace MonoZelda.Enemies.StalfosFolder
{
    public class Stalfos : IEnemy
    {
        private readonly StalfosStateMachine stateMachine;
        private Point pos;
        private readonly Random rnd = new();
        private SpriteDict stalfosSpriteDict;
        private StalfosStateMachine.Direction direction = StalfosStateMachine.Direction.Left;
        private readonly GraphicsDeviceManager graphics;
        private readonly int spawnX;
        private readonly int spawnY;
        private bool spawning;

        private double startTime = 0;

        public Stalfos(SpriteDict spriteDict, GraphicsDeviceManager graphics)
        {
            stalfosSpriteDict = spriteDict;
            stateMachine = new StalfosStateMachine();
            this.graphics = graphics;
            spawnX = 3 * graphics.PreferredBackBufferWidth / 5;
            spawnY = 3 * graphics.PreferredBackBufferHeight / 5;
            pos = new(spawnX, spawnY);
        }

        public void SetOgPos(GameTime gameTime)
        {
            pos.X = spawnX;
            pos.Y = spawnY;
            stalfosSpriteDict.Position = pos;
            stalfosSpriteDict.SetSprite("cloud");
            spawning = true;
            startTime = gameTime.TotalGameTime.TotalSeconds;
        }

        public void DisableProjectile()
        {
        }

        public void ChangeDirection()
        {
            switch (rnd.Next(1, 5))
            {
                case 1:
                    direction = StalfosStateMachine.Direction.Left;
                    break;
                case 2:
                    direction = StalfosStateMachine.Direction.Right;
                    break;
                case 3:
                    direction = StalfosStateMachine.Direction.Up;
                    break;
                case 4:
                    direction = StalfosStateMachine.Direction.Down;
                    break;
            }
            stateMachine.ChangeDirection(direction);
        }

        public void Update(GameTime gameTime)
        {
            if (spawning)
            {
                if (gameTime.TotalGameTime.TotalSeconds >= startTime + 0.3)
                {
                    startTime = gameTime.TotalGameTime.TotalSeconds;
                    spawning = false;
                    stalfosSpriteDict.SetSprite("stalfos");
                }
            }
            else if (gameTime.TotalGameTime.TotalSeconds >= startTime + 1)
            {
                startTime = gameTime.TotalGameTime.TotalSeconds;
                ChangeDirection();
            }
            else
            {
                pos = stateMachine.Update(pos, graphics);
                stalfosSpriteDict.Position = pos;
            }
        }
    }
}
