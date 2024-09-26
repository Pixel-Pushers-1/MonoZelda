﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using PixelPushers.MonoZelda;
using PixelPushers.MonoZelda.Sprites;

namespace MonoZelda.Enemies.TrapFolder
{
    public class Trap : IEnemy
    {
        private readonly TrapStateMachine stateMachine;
        private Point pos; // will change later
        private readonly SpriteDict trapSpriteDict;
        private TrapStateMachine.Direction direction;
        private readonly GraphicsDeviceManager graphics;
        private readonly int spawnX;
        private readonly int spawnY;

        private readonly TrapStateMachine.Direction attackDirection;

        public Trap(SpriteDict spriteDict, GraphicsDeviceManager graphics, TrapStateMachine.Direction attackDirection)
        {
            trapSpriteDict = spriteDict;
            stateMachine = new TrapStateMachine();
            this.graphics = graphics;
            this.attackDirection = attackDirection;
            direction = attackDirection;
            spawnX = 3 * graphics.PreferredBackBufferWidth / 5;
            spawnY = 3 * graphics.PreferredBackBufferHeight / 5;
            pos = new(spawnX,spawnY);
        }

        public void SetOgPos()
        {
            pos.X = spawnX;
            pos.Y = spawnY;
            trapSpriteDict.SetSprite("bladetrap");
        }

        public void ChangeDirection()
        {
            stateMachine.ChangeDirection(direction);
        }

        public void LeftRight()
        {
            if (pos.X <= 0 + 32)
            {
                direction = TrapStateMachine.Direction.Right;
                stateMachine.ChangeSpeed(1);
            }else if (pos.X >= spawnX)
            {
                direction = TrapStateMachine.Direction.Left;
                stateMachine.ChangeSpeed(3);
            }

        }

        public void RightLeft()
        {
            if (pos.X >= graphics.PreferredBackBufferWidth - 32)
            {
                direction = TrapStateMachine.Direction.Left;
                stateMachine.ChangeSpeed(1);
            }
            else if (pos.X <= spawnX)
            {
                direction = TrapStateMachine.Direction.Right;
                stateMachine.ChangeSpeed(3);
            }

        }

        public void UpDown()
        {
            if (pos.Y <= 0 + 32)
            {
                direction = TrapStateMachine.Direction.Down;
                stateMachine.ChangeSpeed(1);
            }
            else if (pos.Y >= spawnY)
            {
                direction = TrapStateMachine.Direction.Up;
                stateMachine.ChangeSpeed(3);
            }
        }

        public void DownUp()
        {
            if (pos.Y >= graphics.PreferredBackBufferHeight - 32)
            {
                direction = TrapStateMachine.Direction.Up;
                stateMachine.ChangeSpeed(1);
            }
            else if (pos.Y <= spawnY)
            {
                direction = TrapStateMachine.Direction.Down;
                stateMachine.ChangeSpeed(3);
            }
        }

        public void Update(GameTime gameTime)
        {
            switch (attackDirection)
            {
                case TrapStateMachine.Direction.Left:
                    LeftRight();
                    break;
                case TrapStateMachine.Direction.Right:
                    RightLeft();
                    break;
                case TrapStateMachine.Direction.Up:
                    UpDown();
                    break;
                case TrapStateMachine.Direction.Down:
                    DownUp();
                    break;
            }

            ChangeDirection();
            pos = stateMachine.Update(pos, graphics);
            trapSpriteDict.Position = pos;
        }
    }
}