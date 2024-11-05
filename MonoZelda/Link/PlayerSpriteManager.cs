using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Commands.GameCommands;
using System.Diagnostics;
using System.Collections.Generic;
using System;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;

namespace MonoZelda.Link;

public enum Direction {
    Up = 5,
    Down = -5,
    Left = 10,
    Right = -10,
}

public class PlayerSpriteManager
{
    private Direction playerDirection;
    private SpriteDict playerSpriteDict;
    private Vector2 playerPosition;
    private float playerSpeed = 4.0f;
    private double timer;

    private static readonly Dictionary<Direction, string> DirectionToStringMap = new()
    {
       { Direction.Up, "up" },
       { Direction.Down, "down" },
       { Direction.Left, "left" },
       { Direction.Right, "right" }
    };

    public PlayerSpriteManager(PlayerState playerStateMachine)
    {
        playerPosition = new Vector2(500, 500);
    }

    public Direction PlayerDirection
    {
        get { return playerDirection; }
    }
    public Vector2 GetPlayerPosition()
    {
        return playerPosition;
    }

    public void SetPosition(Vector2 position)
    {
        playerPosition = position;
        playerSpriteDict.Position = position.ToPoint();
    }

    public void SetPlayerSpriteDict(SpriteDict spriteDict)
    {
        playerSpriteDict = spriteDict;
        playerDirection = Direction.Down;
    }

    public void Move(PlayerMoveCommand moveCommand)
    {
        if (timer > 0) {
            timer -= MonoZeldaGame.GameTime.ElapsedGameTime.TotalSeconds;
            return;
        }

        playerDirection = moveCommand.PlayerDirection;

        // Determine movement vector
        Vector2 movement = playerDirection switch
        {
            Direction.Up => new Vector2(0, -1),
            Direction.Down => new Vector2(0, 1),
            Direction.Left => new Vector2(-1, 0),
            Direction.Right => new Vector2(1, 0),
            _ => Vector2.Zero
        };

        // Get direction string from the dictionary
        if (DirectionToStringMap.TryGetValue(playerDirection, out string directionString))
        {
            string spriteName = $"walk_{directionString}";
            playerSpriteDict.SetSprite(spriteName);
        }

        // Apply movement to player and sprite
        playerPosition += playerSpeed * movement;
        playerSpriteDict.Position = playerPosition.ToPoint();
    }
        
    public void StandStill(PlayerStandingCommand standCommand)
    {
        if (timer <= 0)
        {
            // Get direction string from the dictionary
            if (DirectionToStringMap.TryGetValue(playerDirection, out string directionString))
            {
                string spriteName = $"standing_{directionString}";
                playerSpriteDict.SetSprite(spriteName);
            }
        }
        else {
            timer -= MonoZeldaGame.GameTime.ElapsedGameTime.TotalSeconds;
        }
        playerSpriteDict.Position = playerPosition.ToPoint();
    }

    public void PlayerDeath()
    {
        if (timer <= 0)
        {
            string spriteName = "hurt_down";
            playerSpriteDict.SetSprite(spriteName);
            timer = playerSpriteDict.SetSpriteOneshot(spriteName);
        }
        else
        {
            timer -= MonoZeldaGame.GameTime.ElapsedGameTime.TotalSeconds;
        }
    }

    public void Attack()
    {
        if (timer <= 0)
        {
            // Get direction string from the dictionary
            if (DirectionToStringMap.TryGetValue(playerDirection, out string directionString))
            {
                string spriteName = $"woodensword_{directionString}";
                timer = playerSpriteDict.SetSpriteOneshot(spriteName);
            }
        }
        else {
            timer -= MonoZeldaGame.GameTime.ElapsedGameTime.TotalSeconds;
        }
    }

    public void UseItem()
    {
        if(timer <= 0)
        {
            // Get direction string from the dictionary
            if (DirectionToStringMap.TryGetValue(playerDirection, out string directionString))
            {
                string spriteName = $"useitem_{directionString}";
                timer = playerSpriteDict.SetSpriteOneshot(spriteName);
            }
        } 
        else {
            timer -= MonoZeldaGame.GameTime.ElapsedGameTime.TotalSeconds;
        }
    }

    public void TakeDamage()
    {
        if (timer <= 0)
        {
            // Get direction string from the dictionary
            if (DirectionToStringMap.TryGetValue(playerDirection, out string directionString))
            {
                string spriteName = $"hurt_{directionString}";
                timer = playerSpriteDict.SetSpriteOneshot(spriteName);
            }
        }
       
    }

}