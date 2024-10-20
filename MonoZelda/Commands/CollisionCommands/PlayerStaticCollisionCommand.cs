using Microsoft.Xna.Framework;
using MonoZelda.Collision;
using MonoZelda.Controllers;
using MonoZelda.Link;
using System.Diagnostics;

namespace MonoZelda.Commands.CollisionCommands;

public class PlayerStaticCollisionCommand : ICommand
{
    private MonoZeldaGame game;
    private Player player;
    public PlayerStaticCollisionCommand()
    {
        //empty
    }

    public PlayerStaticCollisionCommand(Player player)
    {
        this.player = player;
    }

    public void Execute(params object[] metadata)
    {
        Collidable collidableA = (Collidable)metadata[0];
        Collidable collidableB = (Collidable)metadata[1];
        CollisionController collisionController = (CollisionController)metadata[2];
        Direction collisionDirection = (Direction)metadata[3];
        Rectangle intersection = (Rectangle)metadata[4];

        Vector2 currentPos = player.GetPlayerPosition();

        switch (collisionDirection)
        {
            case Direction.Left:
                currentPos.X -= intersection.Width;
                break;
            case Direction.Right:
                currentPos.X += intersection.Width;
                break;
            case Direction.Up:
                currentPos.Y -= intersection.Height;
                break;
            case Direction.Down:
                currentPos.Y += intersection.Height;
                break;
        }

        player.SetPosition(currentPos);
    }

    public void UnExecute()
    {
        //empty
    }
}
