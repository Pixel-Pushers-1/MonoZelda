using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using MonoZelda.Collision;
using MonoZelda.Controllers;
using MonoZelda.Enemies.EnemyProjectiles;
using MonoZelda.Items;
using MonoZelda.Link;
using MonoZelda.Sprites;

namespace MonoZelda.Enemies.EnemyClasses
{
    public class Stalfos : Enemy
    {
        private List<IEnemyProjectile> fireballs = new();
        private Dictionary<IEnemyProjectile, EnemyProjectileCollisionManager> projectileDictionary = new();
        private bool projActive = false;
        
        public Stalfos()
        {
            Width = 48;
            Height = 48;
            Health = 2;
            Alive = true;
        }

        public override void EnemySpawn(SpriteDict enemyDict, Point spawnPosition, CollisionController collisionController, ItemFactory itemFactory, bool hasItem)
        {
            EnemyHitbox = new EnemyCollidable(new Rectangle(spawnPosition.X, spawnPosition.Y, Width, Height), EnemyList.Stalfos);
            base.EnemySpawn(enemyDict, spawnPosition, collisionController, itemFactory, hasItem);
            StateMachine.SetSprite("stalfos");
        }

        public override void LevelOneBehavior()
        {
            //default skeleton
        }

        public override void LevelTwoBehavior()
        {
            if(!projActive && Timer >= 3){
                projActive = true;
                Timer = 0;
                var move = PlayerState.Position.ToVector2() - Pos.ToVector2();
                move = Vector2.Divide(move, (float)Math.Sqrt(move.X * move.X + move.Y * move.Y)) * 6;
                fireballs.Add(new Fireball(Pos, CollisionController, move));
                    foreach (var projectile in fireballs)
                    {
                        projectileDictionary.Add(projectile, new EnemyProjectileCollisionManager(projectile));
                    }
            }
            fireballUpdate();
        }

        public override void LevelThreeBehavior()
        {
            if(!projActive && Timer >= 3){
                projActive = true;
                Timer = 0;
                var move = PlayerState.Position.ToVector2() - Pos.ToVector2();
                move = Vector2.Divide(move, (float)Math.Sqrt(move.X * move.X + move.Y * move.Y)) * 6;
                fireballs.Add(new Fireball(Pos, CollisionController, new Vector2(move.X,move.Y - 1)));
                fireballs.Add(new Fireball(Pos, CollisionController, move));
                foreach (var projectile in fireballs)
                {
                    projectileDictionary.Add(projectile, new EnemyProjectileCollisionManager(projectile));
                }
            }
            fireballUpdate();
        }

        public void fireballUpdate(){
            var tempActive = false;
            foreach (var entry in projectileDictionary)
            {
                if (entry.Key.Active)
                {
                    tempActive = true;
                }else{
                    fireballs.Remove(entry.Key);
                    projectileDictionary.Remove(entry.Key);
                }
            }
            projActive = tempActive;
            foreach (KeyValuePair<IEnemyProjectile, EnemyProjectileCollisionManager> fireball in projectileDictionary)
            {
                fireball.Key.Update(EnemyStateMachine.Direction.Left,Pos);
                fireball.Value.Update();
            }
        }
    }
}
