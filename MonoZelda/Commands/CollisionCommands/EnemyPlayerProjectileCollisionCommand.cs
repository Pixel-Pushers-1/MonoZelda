using System;
using MonoZelda.Collision;
using MonoZelda.Controllers;
using MonoZelda.Enemies;
using MonoZelda.Enemies.EnemyClasses;
using MonoZelda.Link;
using MonoZelda.Link.Projectiles;

namespace MonoZelda.Commands.CollisionCommands;

public class EnemyPlayerProjectileCollisionCommand : ICommand
{
    private MonoZeldaGame game;

    public EnemyPlayerProjectileCollisionCommand()
    {
        //empty
    }
    public void Execute(params object[] metadata)
    {
        int damage = 1;

        if (PlayerState.Level >= 10)
        {
            damage = 3;
        }
        else if (PlayerState.Level >= 5)
        {
            damage = 2;
        }
        ICollidable collidableA = (ICollidable)metadata[0];
        ICollidable collidableB = (ICollidable)metadata[1];
        CollisionController collisionController = (CollisionController)metadata[2];
        Direction collisionDirection = (Direction)metadata[3];

        PlayerProjectileCollidable projectileCollidable;
        EnemyCollidable enemyCollidable;
        float stunTime = 0;
        if (collidableA.type == CollidableType.PlayerProjectile)
        {
            projectileCollidable = (PlayerProjectileCollidable)collidableA;
            enemyCollidable = (EnemyCollidable)collidableB;
        }
        else
        {
            projectileCollidable = (PlayerProjectileCollidable)collidableB;
            enemyCollidable = (EnemyCollidable)collidableA;
        }

        projectileCollidable.HandleCollision();

        Enemy enemy = enemyCollidable.getEnemy();

        if (projectileCollidable.projectileType == ProjectileType.Boomerang ||
            projectileCollidable.projectileType == ProjectileType.BoomerangBlue)
        {
            stunTime = 2;
            damage = 0;
        }


        if (projectileCollidable.projectileType == ProjectileType.BombExplosion)
        {
            damage = 2;
        }

        if(enemyCollidable.enemyType == EnemyList.DodongoMouth && projectileCollidable.projectileType == ProjectileType.Bomb){
            enemy.TakeDamage(2, collisionDirection, 2);
            projectileCollidable.Projectile.FinishProjectile();
            collisionController.RemoveCollidable(projectileCollidable);
        }

        if(enemyCollidable.enemyType != EnemyList.DodongoMouth){
            enemy.TakeDamage(stunTime, collisionDirection, damage);
            collisionController.RemoveCollidable(projectileCollidable);
        }
    }

    public void UnExecute()
    {
        //empty
    }
}
