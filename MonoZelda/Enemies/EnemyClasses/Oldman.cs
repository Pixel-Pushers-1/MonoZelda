using Microsoft.Xna.Framework;
using MonoZelda.Controllers;
using MonoZelda.Sprites;
using System;
using MonoZelda.Items;
using MonoZelda.Link;

namespace MonoZelda.Enemies.EnemyClasses
{
    public class Oldman : Enemy
    {
        public Oldman()
        {
            Width = 64;
            Height = 64;
            Alive = true;

        }

        public override void EnemySpawn(SpriteDict enemyDict, Point spawnPosition, CollisionController collisionController, ItemFactory itemFactory, bool hasItem)
        {
            Pos = new Point(spawnPosition.X -32, spawnPosition.Y + 64);
            StateMachine = new EnemyStateMachine(enemyDict);
            StateMachine.SetSprite("oldman");
            PixelsMoved = 0;
        }

        public override void Update()
        {
            StateMachine.Update(this, Pos);
        }

        public override void TakeDamage(float stunTime, Direction collisionDirection, int damage)
        {
            // oldman is immortal
        }
    }
}