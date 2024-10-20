using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Commands.GameCommands;

namespace MonoZelda.Link;

public enum Direction {
    Up,
    Down,
    Left,
    Right
}

public class Player
{
    private Direction playerDirection;
    private SpriteDict playerSpriteDict;
    private Vector2 playerPostition;
    private float playerSpeed = 4.0f;
    private int frameTimer;

    public Player()
    {
        playerPostition = new Vector2(500, 500);
    }

    public void SetPlayerSpriteDict(SpriteDict spriteDict)
    {
        playerSpriteDict = spriteDict;
    }

    public void Move(PlayerMoveCommand moveCommand)
    {
        if (frameTimer > 0) {
            frameTimer--;
            return;
        }

        playerDirection = moveCommand.PlayerDirection;
        Vector2 movement = new();
        switch (playerDirection) {
            case Direction.Up: {
                movement = new Vector2(0, -1);
                playerSpriteDict.SetSprite("walk_up");
                break;
            }
            case Direction.Down: {
                movement = new Vector2(0, 1);
                playerSpriteDict.SetSprite("walk_down");
                break;
            }
            case Direction.Left: {
                movement = new Vector2(-1, 0);
                playerSpriteDict.SetSprite("walk_left");
                break;
            }
            case Direction.Right: {
                movement = new Vector2(1, 0);
                playerSpriteDict.SetSprite("walk_right");
                break;
            }
        }

        //apply movement to player and sprite
        playerPostition += playerSpeed * movement;
        playerSpriteDict.Position = playerPostition.ToPoint();
    }
        
    public void StandStill(PlayerStandingCommand standCommand)
    {
        if(frameTimer == 0)
        {
            switch (playerDirection)
            {
                case Direction.Up:
                    playerSpriteDict.SetSprite("standing_up");
                    break;
                case Direction.Down:
                    playerSpriteDict.SetSprite("standing_down");
                    break;
                case Direction.Left:
                    playerSpriteDict.SetSprite("standing_left");
                    break;
                case Direction.Right:
                    playerSpriteDict.SetSprite("standing_right");
                    break;
            }
        }
        else
        {
            frameTimer--;
        }

        playerSpriteDict.Position = playerPostition.ToPoint();
    }

    public void Attack()
    {
        if (frameTimer == 0)
        {
            frameTimer = 20;
            switch (playerDirection)
            {
                case Direction.Up:
                    playerSpriteDict.SetSprite("woodensword_up");
                    break;
                case Direction.Down:
                    playerSpriteDict.SetSprite("woodensword_down");
                    break;
                case Direction.Left:
                    playerSpriteDict.SetSprite("woodensword_left");
                    break;
                case Direction.Right:
                    playerSpriteDict.SetSprite("woodensword_right");
                    break;
            }
        }
        else
        {
            frameTimer--;
        }
        
    }

    public void UseItem()
    {
        if(frameTimer == 0)
        {
            frameTimer = 20;
            switch (playerDirection)
            {
                case Direction.Up:
                    playerSpriteDict.SetSprite("useitem_up");
                    break;
                case Direction.Down:
                    playerSpriteDict.SetSprite("useitem_down");
                    break;
                case Direction.Left:
                    playerSpriteDict.SetSprite("useitem_left");
                    break;
                case Direction.Right:
                    playerSpriteDict.SetSprite("useitem_right");
                    break;
            }
        }
        else
        {
            frameTimer--;
        }
        
    }


    public void TakeDamage()
    {
        if (frameTimer == 0)
        {
            frameTimer = 20;
            switch (playerDirection)
            {
                case Direction.Up:
                    playerSpriteDict.SetSprite("hurt_up");
                    break;
                case Direction.Down:
                    playerSpriteDict.SetSprite("hurt_down");
                    break;
                case Direction.Left:
                    playerSpriteDict.SetSprite("hurt_left");
                    break;
                case Direction.Right:
                    playerSpriteDict.SetSprite("hurt_right");
                    break;
            }
        }
        else
        {
            frameTimer--;
        }
        
    }

    public Direction PlayerDirection
    {
        get { return playerDirection; }
    }
    public int FrameTimer
    {
        get { return frameTimer; }
        set { frameTimer = value; }
    }
    public Vector2 GetPlayerPosition()
    {
        return playerPostition;
    }
    public void SetPosition(Vector2 position)
    {
        playerPostition = position;
        playerSpriteDict.Position = position.ToPoint();
    }

}