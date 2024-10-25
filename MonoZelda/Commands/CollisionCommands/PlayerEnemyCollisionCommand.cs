using Microsoft.Xna.Framework;
using MonoZelda.Collision;
using MonoZelda.Link;

namespace MonoZelda.Commands.CollisionCommands;

public class PlayerEnemyCollisionCommand : ICommand
{
    private MonoZeldaGame game;
    private PlayerCollisionManager playerCollisionManager;

    public PlayerEnemyCollisionCommand()
    {
        //empty
    }

    public PlayerEnemyCollisionCommand(PlayerCollisionManager playerCollisionManager)
    {
        this.playerCollisionManager = playerCollisionManager;
    }

    public void Execute(params object[] metadata)
    {
        ICollidable collidableA = (ICollidable)metadata[0];
        ICollidable collidableB = (ICollidable)metadata[1];
        Direction collisionDirection = (Direction)metadata[3];
        Rectangle intersection = (Rectangle)metadata[4];

        PlayerCollidable playerCollidable;

        if (collidableA.type == CollidableType.Player)
        {
            playerCollidable = (PlayerCollidable)collidableA;
        }
        else
        {
            playerCollidable = (PlayerCollidable)collidableB;
        }

        PlayerCollisionManager playerCollision = playerCollidable.PlayerCollision;
        playerCollision.HandleEnemyCollision(collisionDirection);
    }

    public void UnExecute()
    {
        //empty
    }
}
