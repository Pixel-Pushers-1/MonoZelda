using MonoZelda.Collision;
using MonoZelda.Controllers;
using MonoZelda.Link;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace MonoZelda.Commands.CollisionCommands;

public class PlayerEnemyCollisionCommand : ICommand
{
    private MonoZeldaGame game;
    private PlayerCollision playerCollision;
    public PlayerEnemyCollisionCommand()
    {
        //empty
    }

    public PlayerEnemyCollisionCommand(MonoZeldaGame game)
    {
        this.game = game;
    }

    public PlayerEnemyCollisionCommand(PlayerCollision playerCollision)
    {
        this.playerCollision = playerCollision;
    }

    public void Execute(params object[] metadata)
    {
        Debug.WriteLine("Hi");
        Collidable collidableA = (Collidable)metadata[0];
        Collidable collidableB = (Collidable)metadata[1];
        CollisionController collisionController = (CollisionController)metadata[2];
        Direction collisionDirection = (Direction)metadata[3];
        Rectangle intersection = (Rectangle)metadata[4];
        playerCollision.HandleEnemyCollision(collisionDirection);



    }

    public void UnExecute()
    {
        //empty
    }
}
