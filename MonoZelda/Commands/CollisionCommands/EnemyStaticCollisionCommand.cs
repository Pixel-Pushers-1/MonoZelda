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

    private void setPosition(IEnemy enemy, Direction collisionDirection, Rectangle Intersection)
    {
        Point currentPos = enemy.Pos;
        switch (collisionDirection)
        {
            case Direction.Left:
                currentPos.X -= Intersection.Width;
                break;
            case Direction.Right:
                currentPos.X += Intersection.Width;
                break;
            case Direction.Up:
                currentPos.Y -= Intersection.Height;
                break;
            case Direction.Down:
                currentPos.Y += Intersection.Height;
                break;
        }
        enemy.Pos = currentPos;
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
            IEnemy enemy = collidableA.Enemy;
            setPosition(enemy, collisionDirection, intersection);  
        }
        else
        {
            IEnemy enemy = collidableB.Enemy;
            setPosition(enemy, collisionDirection, intersection);
        }
    }

    public void UnExecute()
    {
        //empty
    }
}
