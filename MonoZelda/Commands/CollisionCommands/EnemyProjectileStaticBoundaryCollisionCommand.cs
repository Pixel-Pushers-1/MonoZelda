using MonoZelda.Collision;
using MonoZelda.Controllers;
using MonoZelda.Enemies;
using MonoZelda.Enemies.EnemyProjectiles;
using MonoZelda.Link;

namespace MonoZelda.Commands.CollisionCommands;

public class EnemyProjectileStaticBoundaryCollisionCommand : ICommand
{
    private MonoZeldaGame game;

    public EnemyProjectileStaticBoundaryCollisionCommand()
    {
        //empty
    }
    public void Execute(params object[] metadata)
    {
        ICollidable collidableA = (ICollidable)metadata[0];
        ICollidable collidableB = (ICollidable)metadata[1];
        CollisionController collisionController = (CollisionController)metadata[2];

        EnemyProjectileCollidable enemyProjectileCollidable;

        if (collidableA.type == CollidableType.PlayerProjectile)
        {
            enemyProjectileCollidable = (EnemyProjectileCollidable)collidableA;
        }
        else
        {
            enemyProjectileCollidable = (EnemyProjectileCollidable)collidableB;
        }
        EnemyProjectileCollisionManager enemyProjectileCollision = enemyProjectileCollidable.EnemyProjectileCollision;

        enemyProjectileCollision.HandleCollision();
    }

    public void UnExecute()
    {
        //empty
    }
}