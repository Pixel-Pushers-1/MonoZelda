﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using MonoZelda.Collision;
using MonoZelda.Controllers;
using MonoZelda.Sprites;

namespace MonoZelda.Enemies
{
    public interface IEnemy
    {
        public Point Pos { get; set; }

        public Collidable EnemyHitbox { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public void EnemySpawn(SpriteDict enemyDict, Point spawnPosition, CollisionController collisionController, ContentManager contentManager);

        public void ChangeDirection();

        public void Update(GameTime gameTime);

        public void KillEnemy();
        
    }
}
