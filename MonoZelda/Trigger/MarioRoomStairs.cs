using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using MonoZelda.Collision;
using MonoZelda.Commands;
using MonoZelda.Commands.GameCommands;
using MonoZelda.Controllers;
using MonoZelda.Dungeons;
using MonoZelda.Link;
using MonoZelda.Scenes;
using MonoZelda.Sprites;

namespace MonoZelda.Trigger;

public class MarioRoomStairsTrigger : TriggerCollidable, IGameUpdate, IDisposable
{
    private readonly ICommand transitionCommand;
    private Direction lastDirection;
    
    public MarioRoomStairsTrigger(TriggerSpawn spawn, CollisionController colliderManager, ICommand transitionCommand) 
        : base(new Rectangle(spawn.Position, new Point(64, 64)))
    {
        this.transitionCommand = transitionCommand;
        OnTrigger += MarioRoomStairs;
        
        // stairs sprite
        var stairsSprite = new SpriteDict(SpriteType.Blocks, SpriteLayer.Background + 1, spawn.Position);
        stairsSprite.SetSprite(nameof(Dungeon1Sprite.tile_stairs_right));
        
        // The trigger collider sits on top of the static collider
        colliderManager.AddCollidable(this);
    }
    
    private void MarioRoomStairs(Direction direction)
    {
        if (lastDirection != 0 && direction != lastDirection)
        {
            transitionCommand.Execute(DungeonScene.MARIO_ROOM, direction);
            OnTrigger -= MarioRoomStairs;
        }
        
        lastDirection = direction;
    }

    public void Update(GameTime time)
    {
    }

    public void Dispose()
    {
        OnTrigger -= MarioRoomStairs;
    }
}