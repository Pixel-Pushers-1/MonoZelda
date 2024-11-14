using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Commands.GameCommands;
using System.Collections.Generic;
using MonoZelda.Dungeons;
using System.Diagnostics;

namespace MonoZelda.Link;

public enum Direction {
    Up = 5,
    Down = -5,
    Left = 10,
    Right = -10,
    None,
}

public enum PickUpType
{
    pickupitem_onehand,
    pickupitem_twohands,
}

public class PlayerSpriteManager
{
    private const float DAMAGE_FLASH_TIME = .5f;
    private const float CLOCK_FLASH_TIME = 3f;
    private const float DAMAGE_IMMOBILITY_TIME = .2f;
    private const float PICKUP_TIME = 3f;

    private Direction playerDirection;
    private SpriteDict playerSpriteDict;
    private Vector2 playerPosition;
    private float playerSpeed = 6.0f;
    private double immobilityTimer;

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

    public void DisablePlayerSprite()
    {
        playerSpriteDict.Enabled = false;   
    }

    public void ClockFlash()
    {
        playerSpriteDict.SetFlashing(SpriteDict.FlashingType.Colorful, CLOCK_FLASH_TIME);
    }

    public void Move(PlayerMoveCommand moveCommand)
    {
        if (!PlayerState.CanMove) {
            return;
        }

        if (immobilityTimer > 0) {
            immobilityTimer -= MonoZeldaGame.GameTime.ElapsedGameTime.TotalSeconds;
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
        PlayerState.Position = playerPosition.ToPoint();
    }

    public void StandStill(PlayerStandingCommand standCommand)
    {
        if (immobilityTimer <= 0)
        {
            // Get direction string from the dictionary
            if (DirectionToStringMap.TryGetValue(playerDirection, out string directionString))
            {
                string spriteName = $"standing_{directionString}";
                playerSpriteDict.SetSprite(spriteName);
            }
        }
        else {
            immobilityTimer -= MonoZeldaGame.GameTime.ElapsedGameTime.TotalSeconds;
        }
        playerSpriteDict.Position = playerPosition.ToPoint();
        PlayerState.Position = playerPosition.ToPoint();
    }

    public void PlayerDeath()
    {
        if (immobilityTimer <= 0)
        {
            string spriteName = "hurt_down";
            playerSpriteDict.SetSprite(spriteName);
            immobilityTimer = playerSpriteDict.SetSpriteOneshot(spriteName);
        }
        else
        {
            immobilityTimer -= MonoZeldaGame.GameTime.ElapsedGameTime.TotalSeconds;
        }
    }

    public void Attack()
    {
        if (immobilityTimer <= 0)
        {
            // Get direction string from the dictionary
            if (DirectionToStringMap.TryGetValue(playerDirection, out string directionString))
            {
                string spriteName = $"woodensword_{directionString}";
                immobilityTimer = playerSpriteDict.SetSpriteOneshot(spriteName);
            }
        }
        else {
            immobilityTimer -= MonoZeldaGame.GameTime.ElapsedGameTime.TotalSeconds;
        }
    }

    public void UseItem()
    {
        if(immobilityTimer <= 0)
        {
            // Get direction string from the dictionary
            if (DirectionToStringMap.TryGetValue(playerDirection, out string directionString))
            {
                string spriteName = $"useitem_{directionString}";
                immobilityTimer = playerSpriteDict.SetSpriteOneshot(spriteName);
            }
        } 
        else {
            immobilityTimer -= MonoZeldaGame.GameTime.ElapsedGameTime.TotalSeconds;
        }
    }

    public void TakeDamage()
    {
        if (immobilityTimer <= 0)
        {
            // Get direction string from the dictionary
            if (DirectionToStringMap.TryGetValue(playerDirection, out string directionString))
            {
                playerSpriteDict.SetFlashing(SpriteDict.FlashingType.Colorful, DAMAGE_FLASH_TIME);
                immobilityTimer = DAMAGE_IMMOBILITY_TIME;
            }
        }
       
    }

    public void PickUpItem(PickUpType pickUpSprite)
    {
        immobilityTimer = PICKUP_TIME;
        if (immobilityTimer > 0)
        {
            playerSpriteDict.SetSprite(pickUpSprite.ToString());
        }
        else
        {
            immobilityTimer -= MonoZeldaGame.GameTime.ElapsedGameTime.TotalSeconds;
        }
    }

}