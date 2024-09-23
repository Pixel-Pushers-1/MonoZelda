﻿using System;
using Microsoft.Xna.Framework;
using PixelPushers.MonoZelda;
using PixelPushers.MonoZelda.Sprites;

namespace MonoZelda.Enemies.KeeseFolder
{
    public class Keese : IEnemy
    {
        private readonly KeeseStateMachine stateMachine;
        private readonly Random rnd = new();
        private Point pos = new(250, 250); //will change
        private SpriteDict keeseSpriteDict;
        private KeeseStateMachine.VertDirection vertDirection = KeeseStateMachine.VertDirection.None;
        private KeeseStateMachine.HorDirection horDirection = KeeseStateMachine.HorDirection.None;
        private double startTime = 0;
        private readonly GraphicsDeviceManager graphics;

        public Keese(SpriteDict spriteDict, GraphicsDeviceManager graphics)
        {
            this.graphics = graphics;
            stateMachine = new KeeseStateMachine();
            keeseSpriteDict = spriteDict;
            keeseSpriteDict.SetSprite("keese_blue");
        }


        public void SetOgPos() //sets to spawn position (eventually could be used for re-entering rooms)
        {
            keeseSpriteDict.SetSprite("keese_blue");
            pos.X = 250;
            pos.Y = 250;
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
            if (gameTime.TotalGameTime.TotalSeconds >= startTime + 1)
            {
                UpdateHorDirection();
                UpdateVertDirection();

                ChangeDirection();
                startTime = gameTime.TotalGameTime.TotalSeconds;
            }

            pos = stateMachine.Update(pos,graphics); //gets position updates from state machine
            keeseSpriteDict.Position = pos; //updates sprite position
        }
    }
}
