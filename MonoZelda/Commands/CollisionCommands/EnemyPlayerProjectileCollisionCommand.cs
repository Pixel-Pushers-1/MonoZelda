using System;
using MonoZelda.Collision;
using MonoZelda.Controllers;
using MonoZelda.Enemies;
using MonoZelda.Link;
using MonoZelda.Link.Projectiles;
using MonoZelda.Sound;

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
        Boolean stun = false;

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

        if (projectileCollidable.ProjectileManager != null)
        {
            projectileCollidable.ProjectileManager.destroyProjectile();
        }

        if (projectileCollidable.projectileType == ProjectileType.Boomerang ||
            projectileCollidable.projectileType == ProjectileType.BoomerangBlue)
        {
            stun = true;
        }
        collisionController.RemoveCollidable(projectileCollidable);
        IEnemy enemy = enemyCollidable.getEnemy();
        enemy.TakeDamage(stun, collisionDirection);
    }

    public void UnExecute()
    {
        //empty
    }
}
