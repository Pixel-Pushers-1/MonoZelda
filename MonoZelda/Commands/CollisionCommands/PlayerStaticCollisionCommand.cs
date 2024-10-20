using MonoZelda.Collision.Collidables;
using MonoZelda.Controllers;
using MonoZelda.Sprites;

namespace MonoZelda.Commands.CollisionCommands;

public class PlayerStaticCollisionCommand : ICommand
{
    private MonoZeldaGame game;

    public PlayerStaticCollisionCommand()
    {
        //empty
    }

    public PlayerStaticCollisionCommand(MonoZeldaGame game)
    {
        this.game = game;
    }

    public void Execute(params object[] metadata)
    {
        ICollidable collidableA = (ICollidable)metadata[0];
        ICollidable collidableB = (ICollidable)metadata[1];
        CollisionController collisionController = (CollisionController)metadata[2];

        if (collidableA.type == CollidableType.Player)
        {
            PlayerCollidable playerCollidable = (PlayerCollidable)collidableA;
            playerCollidable.updatePlayer();
        }
        else
        {
            PlayerCollidable playerCollidable = (PlayerCollidable)collidableB;
            playerCollidable.updatePlayer();
        }
    }

    public void UnExecute()
    {
        //empty
    }
}
