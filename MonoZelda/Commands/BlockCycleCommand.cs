using System;
using Microsoft.Xna.Framework.Input;
using PixelPushers.MonoZelda.Tiles;

namespace PixelPushers.MonoZelda.Commands;

public class BlockCycleCommand : ICommand
{
    ICycleable cycleable;
    int cycleAddition;

    public MonoZeldaGame Game { get; set; }

    public BlockCycleCommand()
    {
    }

    public BlockCycleCommand(ICycleable _tile)
    {
        cycleable = _tile;
    }

    public void Execute(Keys PressedKey)
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
