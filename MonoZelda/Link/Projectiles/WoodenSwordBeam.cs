using Microsoft.Xna.Framework;
using MonoZelda.Sprites;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoZelda.Link.Projectiles;

public class WoodenSwordBeam : Projectile, IProjectile
{
    private bool Finished;
    private float projectileSpeed = 8f;
    private int tilesTraveled;
    private bool rotate;
    private AnimateSwordBeamEnd animateSwordBeamEnd;
    private Vector2 InitialPosition;
    private Vector2 Dimension = new Vector2(8, 16);
    private SpriteDict projectileDict;
    private Player player;

    public WoodenSwordBeam(SpriteDict projectileDict, Player player) : base(projectileDict, player)
    {
        this.projectileDict = projectileDict;
        this.player = player;
        Finished = false;
        rotate = false;
        tilesTraveled = 0;
        InitialPosition = SetInitialPosition(Dimension);
    }

    private void AnimateSwordBeam()
    {
        animateSwordBeamEnd = AnimateSwordBeamEnd.CreateInstance(this);
    }

    private void updatePosition()
    {
        switch (playerDirection)
        {
            case Direction.Up:
                projectilePosition += projectileSpeed * new Vector2(0, -1);
                SetProjectileSprite("woodensword_item_up");
                rotate = false;
                break;
            case Direction.Down:
                projectilePosition += projectileSpeed * new Vector2(0, 1);
                SetProjectileSprite("woodensword_item_down");
                rotate = false;
                break;
            case Direction.Left:
                projectilePosition += projectileSpeed * new Vector2(-1, 0);
                SetProjectileSprite("woodensword_item_left");
                rotate = true;
                break;
            case Direction.Right:
                projectilePosition += projectileSpeed * new Vector2(1, 0);
                SetProjectileSprite("woodensword_item_right");
                rotate = true;
                break;
        }
    }

    private void updateTilesTraveled()
    {
        double tolerance = 0.000001;
        if (Math.Abs(CalculateDistance(InitialPosition) - 64f) < tolerance)
        {
            tilesTraveled++;
            InitialPosition = projectilePosition;
        }
    }

    public void UpdateProjectile()
    {
        if (tilesTraveled < 5)
        {
            updatePosition();
            projectileDict.Position = projectilePosition.ToPoint();
            updateTilesTraveled();
        }
        else if (tilesTraveled == 5)
        {
            //tilesTraveled = AnimateSwordBeamEnd.animate();
            projectileDict.SetSprite("poof");
            tilesTraveled = 6;
        }
        else if (tilesTraveled == 6)
        {
            Finished = reachedDistance();
        }

    }

    public bool reachedDistance()
    {
        bool reachedDistance = false;

        if (tilesTraveled == 6)
        {
            reachedDistance = true;
            projectileDict.SetSprite("");
            projectileDict.Enabled = false;
        }

        return reachedDistance;
    }

    public bool hasFinished()
    {
        return Finished;
    }

    public void FinishProjectile()
    {
        tilesTraveled = 6;
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

