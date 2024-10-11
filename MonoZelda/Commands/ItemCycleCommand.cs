using System;
using Microsoft.Xna.Framework.Input;
using PixelPushers.MonoZelda.Controllers;
using PixelPushers.MonoZelda.Tiles;

namespace PixelPushers.MonoZelda.Commands;

public class ItemCycleCommand : ICommand
{
    IController controller;
    ICycleable cycleable;
    int cycleAddition;

    public ItemCycleCommand()
    {
    }

    public ItemCycleCommand(ICycleable item)
    {
        cycleable = item;
    }
    
    public GameState Execute(Keys PressedKey)
    {
        // Update the currentItem based on the value of cycleAddition
        if (cycleable != null)
        {
            if (cycleAddition > 0)
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
