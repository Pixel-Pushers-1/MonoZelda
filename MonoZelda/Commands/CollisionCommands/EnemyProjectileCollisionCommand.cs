using Microsoft.Xna.Framework;
using MonoZelda.Collision;
using MonoZelda.Controllers;
using MonoZelda.Enemies;
using MonoZelda.Link;
using MonoZelda.Link.Projectiles;

namespace MonoZelda.Commands.CollisionCommands;

public class EnemyProjectileCollisionCommand : ICommand
{
    private MonoZeldaGame game;

    public EnemyProjectileCollisionCommand()
    {
        //empty
    }

    public EnemyProjectileCollisionCommand(MonoZeldaGame game)
    {
        this.game = game;
    }

    private void handleCollision(ProjectileManager projectileManager,IEnemy enemy, CollisionController collisionController, Collidable enemyCollidable)
    {
        if(projectileManager != null)
        {
            projectileManager.destroyProjectile();
        }
        enemy.KillEnemy();
        collisionController.RemoveCollidable(enemyCollidable);
    }

    public void Execute(params object[] metadata)
    {
        Collidable collidableA = (Collidable)metadata[0];
        Collidable collidableB = (Collidable)metadata[1];
        CollisionController collisionController = (CollisionController)metadata[2];

        if (collidableA.type == CollidableType.Projectile)
        {
            ProjectileManager projectileManager = collidableA.ProjectileManager;
            IEnemy enemy = collidableB.Enemy;
            handleCollision(projectileManager, enemy, collisionController, collidableB);
        }
        else
        {
            ProjectileManager projectileManager = collidableB.ProjectileManager;
            IEnemy enemy = collidableA.Enemy;
            handleCollision(projectileManager, enemy, collisionController, collidableA);
        }
    }

    public void UnExecute()
    {
        //empty
    }
}
