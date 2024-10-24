using MonoZelda.Collision;
using MonoZelda.Link;
using Microsoft.Xna.Framework;
using MonoZelda.Enemies.EnemyProjectiles;

namespace MonoZelda.Commands.CollisionCommands;

public class PlayerEnemyProjectileCollisionCommand : ICommand
{
    private MonoZeldaGame game;
    private Player player;

    public PlayerEnemyProjectileCollisionCommand()
    {
        //empty
    }

    public void Execute(params object[] metadata)
    {
        ICollidable collidableA = (ICollidable)metadata[0];
        ICollidable collidableB = (ICollidable)metadata[1];
        Direction collisionDirection = (Direction)metadata[3];
        Rectangle intersection = (Rectangle)metadata[4];

        EnemyProjectileCollidable enemyProjectileCollidable;
        PlayerCollidable playerCollidable;

        if (collidableA.type == CollidableType.Player)
        {
            enemyProjectileCollidable = (EnemyProjectileCollidable)collidableA;
            playerCollidable = (PlayerCollidable)collidableB;
        }
        else
        {
            enemyProjectileCollidable = (EnemyProjectileCollidable)collidableB;
            playerCollidable = (PlayerCollidable)collidableA;
        }

        EnemyProjectileCollisionManager enemyProjectileCollision = enemyProjectileCollidable.EnemyProjectileCollision;
        PlayerCollisionManager playerCollision = playerCollidable.PlayerCollision;

        playerCollision.HandleEnemyProjectileCollision(collisionDirection);
        enemyProjectileCollision.DestroyProjectile();
    }
    public void UnExecute()
    {
        //empty
    }
}
