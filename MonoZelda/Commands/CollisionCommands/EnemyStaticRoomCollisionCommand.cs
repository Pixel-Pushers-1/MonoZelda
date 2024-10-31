using MonoZelda.Collision;
using Microsoft.Xna.Framework;
using MonoZelda.Link;
using MonoZelda.Enemies;

namespace MonoZelda.Commands.CollisionCommands;

public class EnemyStaticRoomCollisionCommand : ICommand
{
    private MonoZeldaGame game;

    public EnemyStaticRoomCollisionCommand()
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

        if(enemyCollidable.enemyType != EnemyList.Keese)
        {
            EnemyCollisionManager enemyCollisionManager = enemyCollidable.EnemyCollision;
            enemyCollisionManager.HandleStaticCollision(collisionDirection, intersection);
        }

    }

    public void UnExecute()
    {
        //empty
    }
}
