using MonoZelda.Collision;
using MonoZelda.Controllers;
using MonoZelda.Enemies;

namespace MonoZelda.Commands.CollisionCommands;

public class PlayerProjectileStaticBoundaryCollisionCommand : ICommand
{
    private MonoZeldaGame game;

    public PlayerProjectileStaticBoundaryCollisionCommand()
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

        if(projectileCollidable.projectileType != Link.Projectiles.ProjectileType.Bomb){
            projectileCollidable.HandleCollision();
            collisionController.RemoveCollidable(projectileCollidable);
        }
    }

    public void UnExecute()
    {
        //empty
    }
}
