﻿using MonoZelda.Collision;
using MonoZelda.Controllers;
using MonoZelda.Link;
using MonoZelda.Sprites;

namespace MonoZelda.Commands.CollisionCommands;

public class PlayerItemCollisionCommand : ICommand
{
    private MonoZeldaGame game;

    public PlayerItemCollisionCommand()
    {
        //empty
    }

    public PlayerItemCollisionCommand(MonoZeldaGame game)
    {
        this.game = game;
    }

    public void Execute(params object[] metadata)
    {
        ICollidable collidableA = (ICollidable) metadata[0];
        ICollidable collidableB = (ICollidable) metadata[1];
        CollisionController collisionController = (CollisionController) metadata[2];
        //it's possible that checking A and B is not necessary if CollisionController is forcing an order 
        ItemCollidable itemCollidable;

        if (collidableA.type == CollidableType.Item)
        {
            itemCollidable = (ItemCollidable)collidableA;
        }
        else
        {
            itemCollidable = (ItemCollidable)collidableB;
        }

        SpriteDict collidableDict = itemCollidable.CollidableDict;
        collidableDict.Unregister();
        itemCollidable.PlaySound();
        itemCollidable.UnregisterHitbox();
        collisionController.RemoveCollidable(itemCollidable);
        
    }

    public void UnExecute()
    {
        //empty
    }
}
