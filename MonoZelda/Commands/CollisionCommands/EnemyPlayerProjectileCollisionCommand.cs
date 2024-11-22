using System;
using MonoZelda.Collision;
using MonoZelda.Controllers;
using MonoZelda.Enemies;
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
        ICollidable collidableA = (ICollidable)metadata[0];
        ICollidable collidableB = (ICollidable)metadata[1];
        CollisionController collisionController = (CollisionController)metadata[2];
        Direction collisionDirection = (Direction)metadata[3];

        PlayerProjectileCollidable projectileCollidable;
        EnemyCollidable enemyCollidable;
        float stunTime = 0;
        int damage = 1;

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

        if (projectileCollidable.projectileType == ProjectileType.Boomerang ||
            projectileCollidable.projectileType == ProjectileType.BoomerangBlue)
        {
            stunTime = 2;
            damage = 0;
        }

        if (projectileCollidable.projectileType == ProjectileType.Bomb)
        {
            damage = 2;
        }

        collisionController.RemoveCollidable(projectileCollidable);
        Enemy enemy = enemyCollidable.getEnemy();
        enemy.TakeDamage(stunTime, collisionDirection, damage);
    }

    public void UnExecute()
    {
        //empty
    }
}
