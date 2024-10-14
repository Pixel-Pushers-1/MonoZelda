using System;
using Microsoft.Xna.Framework.Input;
using PixelPushers.MonoZelda.Tiles;

namespace PixelPushers.MonoZelda.Commands;

public class ItemCycleCommand : ICommand
{
    ICycleable cycleable;
    int cycleAddition;

    public MonoZeldaGame Game { get; set; }

    public ItemCycleCommand()
    {
    }

    public ItemCycleCommand(ICycleable item)
    {
        cycleable = item;
    }
    
    public void Execute(Keys PressedKey)
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
    }

    public void UnExecute()
    {
        throw new NotImplementedException();
    }

    public void SetCycleAddition(int cycleAddition)
    {
        this.cycleAddition = cycleAddition;
    }
}
