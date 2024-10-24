using MonoZelda.Collision;
using MonoZelda.Controllers;
using MonoZelda.Enemies;

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

        PlayerProjectileCollidable projectileCollidable;
        EnemyCollidable enemyCollidable;

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
        IEnemy enemy = enemyCollidable.getEnemy();
        enemy.KillEnemy();
        collisionController.RemoveCollidable(enemyCollidable);
    }

    public void UnExecute()
    {
        //empty
    }
}
