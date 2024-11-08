using Microsoft.Xna.Framework;
using MonoZelda.Collision;
using MonoZelda.Enemies;
using MonoZelda.Enemies.EnemyClasses;
using MonoZelda.Link;
using MonoZelda.Scenes;

namespace MonoZelda.Commands.CollisionCommands;

public class PlayerEnemyCollisionCommand : ICommand
{
    private MonoZeldaGame game;
    private CommandManager commandManager;

    public PlayerEnemyCollisionCommand()
    {
        //empty
    }

    public PlayerEnemyCollisionCommand(CommandManager commandManager)
    {
        this.commandManager = commandManager;
    }

    public void Execute(params object[] metadata)
    {
        ICollidable collidableA = (ICollidable)metadata[0];
        ICollidable collidableB = (ICollidable)metadata[1];
        Direction collisionDirection = (Direction)metadata[3];
        Rectangle intersection = (Rectangle)metadata[4];

        PlayerCollidable playerCollidable;
        EnemyCollidable enemyCollidable;

        if (collidableA.type == CollidableType.Player)
        {
            playerCollidable = (PlayerCollidable)collidableA;
            enemyCollidable = (EnemyCollidable)collidableB;
        }
        else
        {
            playerCollidable = (PlayerCollidable)collidableB;
            enemyCollidable = (EnemyCollidable)collidableA;
        }

        PlayerCollisionManager playerCollision = playerCollidable.PlayerCollision;
        playerCollision.HandleEnemyCollision(collisionDirection);
        if (enemyCollidable.enemyType == EnemyList.Wallmaster)
        {
            Wallmaster enemy = (Wallmaster)enemyCollidable.getEnemy();
            enemy.grabPlayer(commandManager);
        }
    }

    public void UnExecute()
    {
        //empty
    }
}
