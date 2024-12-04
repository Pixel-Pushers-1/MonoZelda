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
    None,
}

public enum PickUpType
{
    pickupitem_onehand,
    pickupitem_twohands,
}

public class PlayerSpriteManager
{
    public const int BLUE_LEVEL_REQUIREMENT = 5;
    public const int RED_LEVEL_REQUIREMENT = 10;

    private const float DAMAGE_FLASH_TIME = .5f;
    private const float CLOCK_FLASH_TIME = 3f;
    private const float DAMAGE_IMMOBILITY_TIME = .2f;
    private const float PICKUP_TIME = 3f;
    private const float LEVELUP_FLASH_TIME = 3f;
    private const int BLUE_LEVEL_REQUIREMENT = 2;
    private const int RED_LEVEL_REQUIREMENT = 4;

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
        playerSpriteDict.SetSprite($"walk_{DirectionToStringMap[PlayerState.Direction]}_{GetLinkColor()}");
        playerDirection = PlayerState.Direction;
    }

    public void DisablePlayerSprite()
    {
        playerSpriteDict.Enabled = false;   
    }

    public void LevelUp()
    {
        playerSpriteDict.SetFlashing(SpriteDict.FlashingType.LevelUp, LEVELUP_FLASH_TIME);
    }

    public void ClockFlash()
    {
        playerSpriteDict.SetFlashing(SpriteDict.FlashingType.Colorful, CLOCK_FLASH_TIME);
    }
    public void Move(PlayerMoveCommand moveCommand)
    {
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
            string spriteName = $"walk_{directionString}_{GetLinkColor()}";
            playerSpriteDict.SetSprite(spriteName);
        }

        // Apply movement to player and sprite
        playerPosition += playerSpeed * movement;
        playerSpriteDict.Position = playerPosition.ToPoint();
        PlayerState.Position = playerPosition.ToPoint();
        PlayerState.Direction = playerDirection;
    }
        
    public void StandStill(PlayerStandingCommand standCommand)
    {
        if (immobilityTimer <= 0)
        {
            // Get direction string from the dictionary
            if (DirectionToStringMap.TryGetValue(playerDirection, out string directionString))
            {
                string spriteName = $"standing_{directionString}_{GetLinkColor()}";
                playerSpriteDict.SetSprite(spriteName);
            }
        }
        else {
            immobilityTimer -= MonoZeldaGame.GameTime.ElapsedGameTime.TotalSeconds;
        }
        playerSpriteDict.Position = PlayerState.Position;
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
                string spriteName = $"attack_{directionString}_{GetLinkColor()}";
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
                string spriteName = $"useitem_{directionString}_{GetLinkColor()}";
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
            playerSpriteDict.SetSprite($"{pickUpSprite}_{GetLinkColor()}");
        }
        else
        {
            immobilityTimer -= MonoZeldaGame.GameTime.ElapsedGameTime.TotalSeconds;
        }
    }

    public static string GetLinkColor()
    {
        if (PlayerState.Level >= RED_LEVEL_REQUIREMENT) {
            return "red";
        }
        if (PlayerState.Level >= BLUE_LEVEL_REQUIREMENT) {
            return "blue";
        }
        return "green";
    }
}