using System;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;
using MonoZelda.Enemies;
using PixelPushers.MonoZelda.Controllers;

namespace PixelPushers.MonoZelda.Commands;

public class EnemyCycleCommand : ICommand
{
    IController controller;
    EnemyCycler enemyCycler;
    int cycleAddition;
    public EnemyCycleCommand()
    {
    }

    public EnemyCycleCommand(EnemyCycler enemyCycler)
    {
        this.enemyCycler = enemyCycler;
    }

    public GameState Execute(Keys PressedKey)
    {
        if (enemyCycler == null)
            return controller.GameState;

        // Apply cycle addition to enemy list
        enemyCycler.SetCycle(cycleAddition);

        // Keep GameState the same inside the controller
        return controller.GameState;
    }

    public GameState UnExecute()
    {
        throw new NotImplementedException();
    }

    public void SetCycleAddition(int cycleAddition)
    {
        this.cycleAddition = cycleAddition;
    }

    public void SetCycler(EnemyCycler enemyCycler)
    {
        this.enemyCycler = enemyCycler;
    }

    public void SetController(IController controller)
    {
        this.controller = controller;
    }
}
