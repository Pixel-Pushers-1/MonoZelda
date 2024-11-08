﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Collision;
using MonoZelda.Controllers;
using MonoZelda.Sprites;
using System;
using MonoZelda.Link;

namespace MonoZelda.Enemies.EnemyClasses
{
    public class Oldman : IEnemy
    {
        public Point Pos { get; set; }
        private SpriteDict oldmanSpriteDict;
        public EnemyCollidable EnemyHitbox { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool Alive { get; set; }

        private int spawnTimer;

        public Oldman()
        {
            Width = 64;
            Height = 64;
            Alive = true;

        }

        public void EnemySpawn(SpriteDict enemyDict, Point spawnPosition, CollisionController collisionController, PlayerState player)
        {
            EnemyHitbox = new EnemyCollidable(new Rectangle(spawnPosition.X, spawnPosition.Y, Width, Height), EnemyList.Oldman);
            collisionController.AddCollidable(EnemyHitbox);
            EnemyHitbox.setSpriteDict(enemyDict);
            enemyDict.Position = spawnPosition;
            enemyDict.SetSprite("cloud");
            oldmanSpriteDict = enemyDict;
            Pos = spawnPosition;
            spawnTimer = 0;
        }

        public void ChangeDirection()
        {
        }

        public void Update(GameTime gameTime)
        {
            if (spawnTimer >= 64)
            {
                oldmanSpriteDict.SetSprite("oldman");
            }
            else
            {
                spawnTimer++;
            }
        }

        public void TakeDamage(Boolean stun, Direction collisionDirection)
        {
            // oldman is immortal
        }
    }
}
