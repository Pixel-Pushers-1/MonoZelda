using MonoZelda.Collision;
using MonoZelda.Controllers;
using Microsoft.Xna.Framework;
using MonoZelda.Link;
using MonoZelda.Enemies;
using Microsoft.VisualBasic;

namespace MonoZelda.Commands.CollisionCommands;

public class EnemyStaticCollisionCommand : ICommand
{
    private MonoZeldaGame game;

    public EnemyStaticCollisionCommand()
    {
        //empty
    }

    public EnemyStaticCollisionCommand(MonoZeldaGame game)
    {
        this.game = game;
    }

    private void setPosition(Collidable enemyCollidable, Direction collisionDirection, Rectangle Intersection)
    {
        CardinalEnemyStateMachine enemyStateMachine = enemyCollidable.CardinalEnemyStateMachine;
        Point currentPos = enemyStateMachine.currentPosition;
        switch (collisionDirection)
        {
            case Direction.Left:
                enemyStateMachine.ChangeDirection(CardinalEnemyStateMachine.Direction.Left);
                currentPos.X -= Intersection.Width;
                break;
            case Direction.Right:
                enemyStateMachine.ChangeDirection(CardinalEnemyStateMachine.Direction.Right);
                currentPos.X += Intersection.Width;
                break;
            case Direction.Up:
                enemyStateMachine.ChangeDirection(CardinalEnemyStateMachine.Direction.Up);
                currentPos.Y -= Intersection.Height;
                break;
            case Direction.Down:
                enemyStateMachine.ChangeDirection(CardinalEnemyStateMachine.Direction.Down);
                currentPos.Y += Intersection.Height;
                break;
        }
        enemyStateMachine.Update(currentPos);
    }

    public void Execute(params object[] metadata)
    {
        Collidable collidableA = (Collidable)metadata[0];
        Collidable collidableB = (Collidable)metadata[1];
        CollisionController collisionController = (CollisionController)metadata[2];
        Direction collisionDirection = (Direction)metadata[3];
        Rectangle intersection = (Rectangle)metadata[4];

        if(collidableA.type == CollidableType.Enemy)
        {
            setPosition(collidableA, collisionDirection, intersection);
        }
        else
        {
            setPosition(collidableB, collisionDirection, intersection);
        }
    }

    public void UnExecute()
    {
        //empty
    }
}
