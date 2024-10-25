using Microsoft.Xna.Framework;
using MonoZelda.Collision;
using MonoZelda.Enemies;
using MonoZelda.Link;
using System.Collections.Generic;

namespace MonoZelda.Commands.CollisionCommands;

public class EnemyStaticBoundaryCollisionCommand : ICommand
{
    private MonoZeldaGame game;

    public EnemyStaticBoundaryCollisionCommand()
    {
        //empty
    }

    public void Execute(params object[] metadata)
    {
        ICollidable collidableA = (ICollidable)metadata[0];
        ICollidable collidableB = (ICollidable)metadata[1];
        Direction collisionDirection = (Direction)metadata[3];
        Rectangle intersection = (Rectangle)metadata[4];

        EnemyCollidable enemyCollidable;

        if (collidableA.type == CollidableType.Enemy)
        {
            enemyCollidable = (EnemyCollidable)collidableA;
        }
        else
        {
            enemyCollidable = (EnemyCollidable)collidableB;
        }

        EnemyCollisionManager enemyCollisionManager = enemyCollidable.EnemyCollision;
        enemyCollisionManager.HandleStaticCollision(collisionDirection, intersection);
    }

    public void UnExecute()
    {
        // empty
    }
}
