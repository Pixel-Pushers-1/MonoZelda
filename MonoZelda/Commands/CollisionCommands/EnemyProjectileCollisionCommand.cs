using Microsoft.Xna.Framework;
using MonoZelda.Collision;
using MonoZelda.Controllers;
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

    public void Execute(params object[] metadata)
    {
        Collidable collidableA = (Collidable)metadata[0];
        Collidable collidableB = (Collidable)metadata[1];

        if (collidableA.type == CollidableType.Projectile)
        {
            ProjectileManager projectileManager = collidableA.ProjectileManager;
            if(projectileManager != null)
                projectileManager.destroyProjectile();
        }
        else
        {
            ProjectileManager projectileManager = collidableB.ProjectileManager;
            if (projectileManager != null)
                projectileManager.destroyProjectile();
        }
    }

    public void UnExecute()
    {
        //empty
    }
}
