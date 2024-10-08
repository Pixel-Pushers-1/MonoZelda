﻿using System;
using PixelPushers.MonoZelda.Link;
using PixelPushers.MonoZelda.Controllers;
using PixelPushers.MonoZelda.Link.Projectiles;


namespace PixelPushers.MonoZelda.Commands;

public class PlayerUseItemCommand : ICommand
{
    Player player;  
    private IController controller;
    private int itemIdx;
    private Projectile projectiles;
    private ILaunch launchProjectile;
    private bool projectileFired;
    
    public PlayerUseItemCommand()
    {
    }

    public PlayerUseItemCommand(Projectile projectile, Player player)
    {
        this.projectiles = projectile;
        this.player = player;
    }

    public GameState Execute()
    {
        if (launchProjectile == null)
            return controller.GameState;

        // Swap player item idx to itemIdx
        launchProjectile.Launch();
        System.Diagnostics.Debug.WriteLine("Player is launching Projectile: " + projectiles.CurrentProjectile);

        // Keep GameState the same inside the controller
        return controller.GameState;
    }

    public GameState UnExecute()
    {
        throw new NotImplementedException();
    }

    public void UseItem()
    {
        if (player == null)
            return;

        player.PlayerUseItem();
    }

    public void SetProjectile(int itemIdx)
    {
        if (projectiles == null)
            return;

        projectiles.CurrentProjectile = (ProjectileType) itemIdx;
        launchProjectile = projectiles.GetProjectileObject();
        projectiles.enableDict();
    }

    public void SetController(IController controller)
    {
        this.controller = controller;
    }

    public bool getProjectileState()
    {
        if(launchProjectile == null)
            return true;

        projectileFired = launchProjectile.hasFinished();
        if (projectileFired)
        {
            launchProjectile = null;
        }
        return projectileFired;
    }
}
