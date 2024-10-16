using System;
using Microsoft.Xna.Framework;
using MonoZelda.Sprites;

namespace MonoZelda.Enemies.KeeseFolder
{
    public class Keese : IEnemy
    {
        private readonly KeeseStateMachine stateMachine;
        private readonly Random rnd = new();
        private Point pos;
        private SpriteDict keeseSpriteDict;
        private KeeseStateMachine.VertDirection vertDirection = KeeseStateMachine.VertDirection.None;
        private KeeseStateMachine.HorDirection horDirection = KeeseStateMachine.HorDirection.None;
        private readonly GraphicsDeviceManager graphics;
        private readonly int spawnX;
        private readonly int spawnY;
        private bool spawning = true;

        private double startTime = 0;

        public Keese(SpriteDict spriteDict, GraphicsDeviceManager graphics)
        {
            this.graphics = graphics;
            stateMachine = new KeeseStateMachine();
            keeseSpriteDict = spriteDict;
            keeseSpriteDict.SetSprite("keese_blue");
            spawnX = 3 * graphics.PreferredBackBufferWidth / 5;
            spawnY = 3 * graphics.PreferredBackBufferHeight / 5;
            pos = new(spawnX, spawnY);
        }


        public void SetOgPos(GameTime gameTime) //sets to spawn position (eventually could be used for re-entering rooms)
        {
            pos.X = spawnX;
            pos.Y = spawnY;
            keeseSpriteDict.Position = pos;
            keeseSpriteDict.SetSprite("cloud");
            spawning = true;
            startTime = gameTime.TotalGameTime.TotalSeconds;
        }

        public void ChangeDirection()
        {
            stateMachine.ChangeHorDirection(horDirection);
            stateMachine.ChangeVertDirection(vertDirection);
        }

        public void UpdateHorDirection()
        {
            switch (rnd.Next(1, 4))
            {
                case 1:
                    horDirection = KeeseStateMachine.HorDirection.Left;
                    break;
                case 2:
                    horDirection = KeeseStateMachine.HorDirection.Right;
                    break;
                case 3:
                    horDirection = KeeseStateMachine.HorDirection.None;
                    UpdateVertDirection();
                    break;
            }
        }

        public void UpdateVertDirection()
        {
            switch (rnd.Next(1, 4))
            {
                case 1:
                    vertDirection = KeeseStateMachine.VertDirection.Up;
                    break;
                case 2:
                    vertDirection = KeeseStateMachine.VertDirection.Down;
                    break;
                case 3:
                    vertDirection = KeeseStateMachine.VertDirection.None;
                    UpdateHorDirection();
                    break;
            }
        }

        public void Update(GameTime gameTime)
        {
            if (spawning)
            {
                if (gameTime.TotalGameTime.TotalSeconds >= startTime + 0.3)
                {
                    startTime = gameTime.TotalGameTime.TotalSeconds;
                    spawning = false;
                    keeseSpriteDict.SetSprite("keese_blue");
                }
            }
            else if (gameTime.TotalGameTime.TotalSeconds >= startTime + 1)
            {
                UpdateHorDirection();
                UpdateVertDirection();

                ChangeDirection();
                startTime = gameTime.TotalGameTime.TotalSeconds;
            }
            else
            {

                pos = stateMachine.Update(pos, graphics); //gets position updates from state machine
                keeseSpriteDict.Position = pos; //updates sprite position
            }
        }

        public void DisableProjectile()
        {
        }
    }
}
