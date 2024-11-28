using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoZelda.Events;

public class EventHandlers
{
    public event Action LevelComplete;
    public event Action WallmasterGrab;
    public event Action LinkDeath;

    // Trigger method for level completion
    public void TriggerLevelCompletionAnimation()
    {
        LevelComplete?.Invoke();
    }

    public  void TriggerLinkDeathAnimation()
    {
        LinkDeath?.Invoke();
    }

    public void TriggerWallMasterGrabAnimation()
    {
        WallmasterGrab?.Invoke();
    }
}

