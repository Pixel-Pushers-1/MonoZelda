using System;

namespace MonoZelda.Events;

public static class EventManager
{
    // Static events
    public static event Action LevelComplete;
    public static event Action WallmasterGrab;
    public static event Action LinkDeath;

    // Trigger method for level completion
    public static void TriggerLevelCompletionAnimation()
    {
        LevelComplete?.Invoke();
    }

    public static void TriggerLinkDeathAnimation()
    {
        LinkDeath?.Invoke();
    }
}
