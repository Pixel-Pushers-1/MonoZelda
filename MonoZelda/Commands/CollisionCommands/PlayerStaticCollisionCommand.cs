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
    private PlayerCollision playerCollision;
    public PlayerStaticCollisionCommand()
    {
        //empty
    }

    public PlayerStaticCollisionCommand(PlayerCollision playerCollision)
    {
        this.playerCollision = playerCollision;
    }

    public void Execute(params object[] metadata)
    {
        Collidable collidableA = (Collidable)metadata[0];
        Collidable collidableB = (Collidable)metadata[1];
        CollisionController collisionController = (CollisionController)metadata[2];
        Direction collisionDirection = (Direction)metadata[3];
        Rectangle intersection = (Rectangle)metadata[4];
        playerCollision.HandleStaticCollision(collisionDirection, intersection);
    }

    public void UnExecute()
    {
        //empty
    }
}
