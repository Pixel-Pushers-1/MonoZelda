using Microsoft.Xna.Framework;
using MonoZelda.Sprites;
using System;

namespace MonoZelda.Link.Projectiles;

public class WoodenSword : Projectile,IProjectile
{
    private bool Finished;
    private float projectileSpeed = 4f;
    private int tilesTraveled;
    private bool rotate;
    private Vector2 InitialPosition;
    private Vector2 Dimension = new Vector2(8, 16);
    private SpriteDict projectileDict;
    private Player player;

    public WoodenSword(SpriteDict projectileDict, Player player) : base(projectileDict, player)
    {
        this.projectileDict = projectileDict;
        this.player = player;
        Finished = false;
        rotate = false;
        tilesTraveled = 0;
        InitialPosition = SetInitialPosition(Dimension);
    }

    private void updateRotate()
    {
        Direction playerDirection = player.PlayerDirection;
        if (playerDirection == Direction.Right || playerDirection == Direction.Left)
        {
            rotate = true;
        }
    }

    public void UpdateProjectile()
    {
        projectileDict.Enabled = false;
        if (tilesTraveled < 4)
        {
            tilesTraveled++;
            updateRotate();
        }
        else if(tilesTraveled == 4)
        {
            Finished = reachedDistance();
        }

    }

    public bool reachedDistance()
    {
        bool reachedDistance = false;

        if (tilesTraveled == 4)
        {
            reachedDistance = true;
        }

        return reachedDistance;
    }

    public bool hasFinished()
    {
        return Finished;
    }

    public void FinishProjectile()
    {
        tilesTraveled = 4;
    }

    public Rectangle getCollisionRectangle()
    {
        Point spawnPosition = projectilePosition.ToPoint();
        if (rotate)
        {
            return new Rectangle(spawnPosition.X - 64 / 2, spawnPosition.Y - 32 / 2, 64, 32);
        }
        else
        {
            return new Rectangle(spawnPosition.X - 32 / 2, spawnPosition.Y - 64 / 2, 32, 64);
        }
    }
}
