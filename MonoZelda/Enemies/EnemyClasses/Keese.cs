using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Collision;
using MonoZelda.Controllers;
using MonoZelda.Sprites;

namespace MonoZelda.Enemies.EnemyClasses
{
    public class Keese : IEnemy
    {
        private  DiagonalEnemyStateMachine stateMachine;
        private readonly Random rnd = new();
        public Point Pos { get; set; }
        public EnemyCollidable EnemyHitbox { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        private SpriteDict keeseSpriteDict;
        private DiagonalEnemyStateMachine.VertDirection vertDirection = DiagonalEnemyStateMachine.VertDirection.None;
        private DiagonalEnemyStateMachine.HorDirection horDirection = DiagonalEnemyStateMachine.HorDirection.None;
        private readonly GraphicsDevice graphicsDevice;
        private int pixelsMoved;

        public Keese(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            Height = 64;
            Width = 64;
        }

        public void EnemySpawn(SpriteDict enemyDict, Point spawnPosition, CollisionController collisionController,
            ContentManager contentManager)
        {
            EnemyHitbox = new EnemyCollidable(new Rectangle(spawnPosition.X, spawnPosition.Y, 60, 60), graphicsDevice, EnemyList.Keese);
            collisionController.AddCollidable(EnemyHitbox);
            EnemyHitbox.setSpriteDict(enemyDict);
            enemyDict.Position = spawnPosition;
            enemyDict.SetSprite("cloud");
            keeseSpriteDict = enemyDict;
            Pos = spawnPosition;
            pixelsMoved = 0;
            stateMachine = new DiagonalEnemyStateMachine();
        }

        public void ChangeDirection()
        {
            keeseSpriteDict.SetSprite("keese_blue");
            stateMachine.ChangeHorDirection(horDirection);
            stateMachine.ChangeVertDirection(vertDirection);
        }

        public void UpdateHorDirection()
        {
            switch (rnd.Next(1, 4))
            {
                case 1:
                    horDirection = DiagonalEnemyStateMachine.HorDirection.Left;
                    break;
                case 2:
                    horDirection = DiagonalEnemyStateMachine.HorDirection.Right;
                    break;
                case 3:
                    horDirection = DiagonalEnemyStateMachine.HorDirection.None;
                    UpdateVertDirection();
                    break;
            }
        }

        public void UpdateVertDirection()
        {
            switch (rnd.Next(1, 4))
            {
                case 1:
                    vertDirection = DiagonalEnemyStateMachine.VertDirection.Up;
                    break;
                case 2:
                    vertDirection = DiagonalEnemyStateMachine.VertDirection.Down;
                    break;
                case 3:
                    vertDirection = DiagonalEnemyStateMachine.VertDirection.None;
                    UpdateHorDirection();
                    break;
            }
        }

        public void Update(GameTime gameTime)
        {
            if (pixelsMoved >= 64)
            {
                pixelsMoved = 0;

                UpdateHorDirection();
                UpdateVertDirection();
                ChangeDirection();
            }
            else
            {
                pixelsMoved++;
                keeseSpriteDict.Position = Pos;
            }
            Pos = stateMachine.Update(Pos);
        }

        public void KillEnemy()
        {
            keeseSpriteDict.Enabled = false;
            EnemyHitbox.UnregisterHitbox();
        }
    }
}
