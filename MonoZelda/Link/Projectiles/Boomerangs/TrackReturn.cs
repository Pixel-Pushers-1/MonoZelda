using System;
using Microsoft.Xna.Framework;

namespace MonoZelda.Link.Projectiles.Boomerangs;

public class TrackReturn
{
    private Player player;
    private Vector2 returnPosition;
    private Vector2 Origin;
    private float boomerangSpeed;
    // Private constructor to prevent instantiation from outside
    private TrackReturn(Player player,float boomerangSpeed)
    {
        this.player = player;
        this.boomerangSpeed = boomerangSpeed;
        returnPosition = new Vector2();
    }

    // Static method to allow only specific classes to instantiate TrackReturn
    public static TrackReturn CreateInstance(object caller,Player player,float boomerangSpeed)
    {
        // Check if the caller is BoomerangBlue or BoomerangGreen
        if (caller is BoomerangBlue || caller is Boomerang)
        {
            return new TrackReturn(player,boomerangSpeed);
        }
        throw new UnauthorizedAccessException("Access denied.");
    }

    private bool updatedReturnPosition()
    {
        bool playerPositionUpdated = false;
        if(returnPosition != player.GetPlayerPosition())
        {
            returnPosition = player.GetPlayerPosition();
            playerPositionUpdated = true;
        }
        return playerPositionUpdated;
    }

    private Vector2 playerPositionUnitVector()
    {
        Vector2 playerPosition = player.GetPlayerPosition();
        Vector2 playerPathVector = new Vector2((playerPosition.X - Origin.X), (playerPosition.Y - Origin.Y));

        float magnitudePath = playerPathVector.Length();

        return new Vector2((playerPathVector.X / magnitudePath), (playerPathVector.Y / magnitudePath));
    }

    public void CheckResetOrigin(Vector2 projectilePosition)
    {
        if (updatedReturnPosition())
        {
            Origin = projectilePosition;
        }
    }

    public Vector2 getProjectileNextPosition()
    {
        Vector2 directionPlayer = playerPositionUnitVector();

        return (directionPlayer * boomerangSpeed);
    }

    public bool Returned(Vector2 ProjectilePosition)
    {
        bool returned = false;
        Vector2 playerPos = player.GetPlayerPosition();
        Vector2 distanceBoomerangPlayer = new Vector2((playerPos.X - ProjectilePosition.X),(playerPos.Y - ProjectilePosition.Y));

        if(distanceBoomerangPlayer.Length() < 48f)
        {
            returned = true;
        }

        return returned;
    }
}

