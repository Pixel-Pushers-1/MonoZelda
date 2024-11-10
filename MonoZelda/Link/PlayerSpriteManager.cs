using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Commands.GameCommands;
using System.Collections.Generic;
using MonoZelda.Dungeons;

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

    public PlayerSpriteManager()
    {
        playerPosition = PlayerState.Position.ToVector2();
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
        PlayerState.Position = position.ToPoint();
    }

    public void SetPlayerSpriteDict(SpriteDict spriteDict)
    {
        playerSpriteDict = spriteDict;
        playerSpriteDict.SetSprite($"walk_{DirectionToStringMap[PlayerState.Direction]}");
        playerDirection = PlayerState.Direction;
    }

    public void Move(PlayerMoveCommand moveCommand)
    {
        if (timer > 0) {
            timer -= MonoZeldaGame.GameTime.ElapsedGameTime.TotalSeconds;
            return;
        }

        playerDirection = moveCommand.PlayerDirection;

        // Determine movement vector
        Vector2 movement = DungeonConstants.DirectionVector[playerDirection];

        // Get direction string from the dictionary
        if (DirectionToStringMap.TryGetValue(playerDirection, out string directionString))
        {
            string spriteName = $"walk_{directionString}";
            playerSpriteDict.SetSprite(spriteName);
        }

        // Apply movement to player and sprite
        playerPosition += playerSpeed * movement;
        playerSpriteDict.Position = playerPosition.ToPoint();
        playerState.Position = playerPosition.ToPoint();
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
        playerState.Position = playerPosition.ToPoint();
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