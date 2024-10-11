using System;
using Microsoft.Xna.Framework.Input;
using PixelPushers.MonoZelda.Controllers;
using PixelPushers.MonoZelda.Tiles;

namespace PixelPushers.MonoZelda.Commands;

public class BlockCycleCommand : ICommand
{
    IController controller;
    ICycleable cycleable;
    int cycleAddition;
    public BlockCycleCommand()
    {
    }

    public BlockCycleCommand(ICycleable _tile)
    {
        cycleable = _tile;
    }

    public GameState Execute(Keys PressedKey)
    {
        if (cycleable != null)
        {
            if(cycleAddition > 0)
            {
                cycleable.Next();
            }
            else
            {
                cycleable.Previous();
            }
        }

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

    public void SetController(IController controller)
    {
        this.controller = controller;
    }
}
