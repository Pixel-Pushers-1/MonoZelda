using System;
using Microsoft.Xna.Framework.Input;
using MonoZelda.Enemies;

namespace PixelPushers.MonoZelda.Commands;

public class EnemyCycleCommand : ICommand
{
    EnemyCycler enemyCycler;
    int cycleAddition;

    public MonoZeldaGame _game { get; set; }

    public EnemyCycleCommand()
    {
    }

    public EnemyCycleCommand(EnemyCycler enemyCycler)
    {
        this.enemyCycler = enemyCycler;
    }

    public void Execute(Keys PressedKey)
    {
        if (enemyCycler == null)
            return;

        // Apply cycle addition to enemy list
        enemyCycler.SetCycle(cycleAddition);

    }

    public void UnExecute()
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
}
