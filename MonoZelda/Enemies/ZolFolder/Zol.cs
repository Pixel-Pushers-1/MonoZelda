using System;
using Microsoft.Xna.Framework;
using MonoZelda.Sprites;

namespace MonoZelda.Enemies.ZolFolder
{
    public class Zol : IEnemy
    {
        private readonly ZolStateMachine stateMachine;
        private Point pos;
        private readonly Random rnd = new();
        private readonly SpriteDict zolSpriteDict;
        private ZolStateMachine.Direction direction = ZolStateMachine.Direction.Left;
        private readonly GraphicsDeviceManager graphics;
        private readonly int spawnX;
        private readonly int spawnY;
        private bool spawning;

        private double startTime = 0;
        private bool readyToJump = true;

        public Zol(SpriteDict spriteDict, GraphicsDeviceManager graphics)
        {
            this.graphics = graphics;
            this.zolSpriteDict = spriteDict;
            stateMachine = new ZolStateMachine();
            spawnX = 3 * graphics.PreferredBackBufferWidth / 5;
            spawnY = 3 * graphics.PreferredBackBufferHeight / 5;
            pos = new(spawnX, spawnY);
        }


        public void SetOgPos(GameTime gameTime) //sets to spawn position (eventually could be used for re-entering rooms)
        {
            pos.X = spawnX;
            pos.Y = spawnY;
            zolSpriteDict.Position = pos;
            zolSpriteDict.SetSprite("cloud");
            spawning = true;
            startTime = gameTime.TotalGameTime.TotalSeconds;
        }

        public void DisableProjectile()
        {
        }

        public void ChangeDirection()
        {
            stateMachine.ChangeDirection(direction);
        }


        public void Update(GameTime gameTime) //too long
        {
            if (spawning)
            {
                if (gameTime.TotalGameTime.TotalSeconds >= startTime + 0.3)
                {
                    startTime = gameTime.TotalGameTime.TotalSeconds;
                    spawning = false;
                    readyToJump = true;
                    zolSpriteDict.SetSprite("zol_green");
                }
            }
            else if (readyToJump)
            {
                switch (rnd.Next(1, 5))
                {
                    case 1:
                        direction = ZolStateMachine.Direction.Left;
                        break;
                    case 2:
                        direction = ZolStateMachine.Direction.Right;
                        break;
                    case 3:
                        direction = ZolStateMachine.Direction.Up;
                        break;
                    case 4:
                        direction = ZolStateMachine.Direction.Down;
                        break;
                }

                startTime = gameTime.TotalGameTime.TotalSeconds;
                readyToJump = false;
                ChangeDirection();
            }
            else if (gameTime.TotalGameTime.TotalSeconds >= startTime + 1)
            {
                direction = ZolStateMachine.Direction.None;
                ChangeDirection();
                if (gameTime.TotalGameTime.TotalSeconds >= startTime + 2)
                {
                    readyToJump = true;
                }
            }
            else
            {
                pos = stateMachine.Update(pos, graphics);
                zolSpriteDict.Position = pos;
            }
        }
    }
}
