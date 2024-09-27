﻿using PixelPushers.MonoZelda.Link;
using PixelPushers.MonoZelda.Sprites;
using PixelPushers.MonoZelda.Commands;
using Microsoft.Xna.Framework;
using System;

namespace PixelPushers.MonoZelda.Link.Projectiles;

public class CandleBlue : Projectile, ILaunch
{
    private bool Finished;
    private Vector2 InitialPosition;
    private SpriteDict projectileDict;
    private Player player;
    private float projectileSpeed = 4f;
    private int tilesTraveled;
    private Vector2 Dimension = new Vector2(16, 16);

    public CandleBlue(SpriteDict projectileDict, Player player) : base(projectileDict, player)
    {
        this.projectileDict = projectileDict;
        this.player = player;
        Finished = false;
        SetProjectileSprite("fire");
        tilesTraveled = 0;
        InitialPosition = SetInitialPosition(Dimension);
    }

    public void Launch()
    {
        if (tilesTraveled < 2)
        {
            updatePosition();
            projectileDict.Position = projectilePosition.ToPoint();
            updateTilesTraveled();
        }
        else if (tilesTraveled == 2)
        {
            projectileDict.Enabled = false;
            Finished = reachedDistance();
        }
    }

    private void updatePosition()
    {
        switch (playerDirection)
        {
            case Direction.Up:
                projectilePosition += projectileSpeed * (new Vector2(0, -1));
                break;
            case Direction.Down:
                projectilePosition += projectileSpeed * (new Vector2(0, 1));
                break;
            case Direction.Left:
                projectilePosition += projectileSpeed * (new Vector2(-1, 0));
                break;
            case Direction.Right:
                projectilePosition += projectileSpeed * (new Vector2(1, 0));
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

    public bool reachedDistance()
    {
        bool reachedDistance = false;

        if (tilesTraveled == 2)
        {
            reachedDistance = true;
        }

        return reachedDistance;
    }

    public bool hasFinished()
    {
        return Finished;
    }
}