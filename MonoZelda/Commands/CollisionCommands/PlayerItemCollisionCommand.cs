using MonoZelda.Collision;
using MonoZelda.Controllers;
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
        Collidable collidableA = (Collidable) metadata[0];
        Collidable collidableB = (Collidable) metadata[1];
        CollisionController collisionController = (CollisionController) metadata[2];

        if (collidableA.type == CollidableType.Item)
        {
            SpriteDict collidableDict = collidableA.CollidableDict;
            collidableDict.Enabled = false;
            collisionController.RemoveCollidable(collidableA);
        }
        else
        {
            SpriteDict collidableDict = collidableB.CollidableDict;
            collidableDict.Enabled = false;
            collisionController.RemoveCollidable(collidableB);
        }
    }

    public void UnExecute()
    {
        //empty
    }
}
