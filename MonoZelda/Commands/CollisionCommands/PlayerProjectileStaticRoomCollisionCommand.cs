using MonoZelda.Collision;
using MonoZelda.Controllers;

namespace MonoZelda.Commands.CollisionCommands;

public class PlayerProjectileStaticRoomCollisionCommand : ICommand
{
    private MonoZeldaGame game;

    public PlayerProjectileStaticRoomCollisionCommand()
    {
        //empty
    }
    public void Execute(params object[] metadata)
    {
        ICollidable collidableA = (ICollidable)metadata[0];
        ICollidable collidableB = (ICollidable)metadata[1];
        CollisionController collisionController = (CollisionController)metadata[2];

        PlayerProjectileCollidable projectileCollidable;

        if (collidableA.type == CollidableType.PlayerProjectile)
        {
            projectileCollidable = (PlayerProjectileCollidable)collidableA;
        }
        else
        {
            projectileCollidable = (PlayerProjectileCollidable)collidableB;
        }

        if (projectileCollidable.projectileType == Link.Projectiles.ProjectileType.Fire)
        {
            projectileCollidable.HandleCollision();
        }
    }

    public void UnExecute()
    {
        //empty
    }
}
