using Microsoft.Xna.Framework;
using MonoZelda.Collision;
using MonoZelda.Link;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoZelda.Commands.CollisionCommands;

public class PlayerStaticCollisionCommand : ICommand
{
    private MonoZeldaGame game;

    public PlayerStaticCollisionCommand()
    {
        //empty
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
        playerCollision.HandleStaticCollision(collisionDirection, intersection);
    }

    public void UnExecute()
    {
        //empty
    }
}
