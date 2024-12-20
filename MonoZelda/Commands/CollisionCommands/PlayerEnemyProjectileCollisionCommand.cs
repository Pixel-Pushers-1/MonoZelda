﻿using MonoZelda.Collision;
using MonoZelda.Link;
using Microsoft.Xna.Framework;
using MonoZelda.Enemies.EnemyProjectiles;

namespace MonoZelda.Commands.CollisionCommands;

public class PlayerEnemyProjectileCollisionCommand : ICommand
{
    private MonoZeldaGame game;
    private PlayerSpriteManager player;

    public PlayerEnemyProjectileCollisionCommand()
    {
        //empty
    }

    public void Execute(params object[] metadata)
    {
        ICollidable collidableA = (ICollidable)metadata[0];
        ICollidable collidableB = (ICollidable)metadata[1];
        Direction collisionDirection = (Direction)metadata[3];
        Rectangle intersection = (Rectangle)metadata[4];

        EnemyProjectileCollidable enemyProjectileCollidable;
        PlayerCollidable playerCollidable;

        if (collidableA.type == CollidableType.Player)
        {
            enemyProjectileCollidable = (EnemyProjectileCollidable)collidableB;
            playerCollidable = (PlayerCollidable)collidableA;
        }
        else
        {
            enemyProjectileCollidable = (EnemyProjectileCollidable)collidableA;
            playerCollidable = (PlayerCollidable)collidableB;
        }

        EnemyProjectileCollisionManager enemyProjectileCollision = enemyProjectileCollidable.EnemyProjectileCollision;
        PlayerCollisionManager playerCollision = playerCollidable.PlayerCollision;

        playerCollision.HandleEnemyProjectileCollision(collisionDirection);
        enemyProjectileCollision.HandleCollision();
    }
    public void UnExecute()
    {
        //empty
    }
}
