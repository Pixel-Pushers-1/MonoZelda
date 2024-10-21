using MonoZelda.Collision;
using MonoZelda.Controllers;
using MonoZelda.Link;
using Microsoft.Xna.Framework;
using MonoZelda.Enemies;
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

    public PlayerEnemyProjectileCollisionCommand(MonoZeldaGame game)
    {
        this.game = game;
    }

    public PlayerEnemyProjectileCollisionCommand(Player player)
    {
        this.player = player;
    }

    private void handleCollision(Direction collisionDirection, IEnemyProjectile enemyProjectile)
    {
        Direction playerDirection = player.PlayerDirection;
        if(playerDirection == collisionDirection)
        {
            // destroy or return projectile
        }
        else
        {
            player.TakeDamage();
        }
    }

    public void Execute(params object[] metadata)
    {
        Collidable collidableA = (Collidable)metadata[0];
        Collidable collidableB = (Collidable)metadata[1];
        CollisionController collisionController = (CollisionController)metadata[2];
        Direction collisionDirection = (Direction)metadata[3];
        Rectangle intersection = (Rectangle)metadata[4];

        if(collidableA.type == CollidableType.Player)
        {
            IEnemyProjectile enemyProjectile = collidableB.EnemyProjectile;
            handleCollision(collisionDirection, enemyProjectile);
        }
        else
        {
            IEnemyProjectile enemyProjectile = collidableA.EnemyProjectile;
            handleCollision(collisionDirection, enemyProjectile);
        }
    }

    public void UnExecute()
    {
        //empty
    }
}
