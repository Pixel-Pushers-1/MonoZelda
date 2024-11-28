using MonoZelda.Commands.GameCommands;
using System;

namespace MonoZelda.Events;

public static class EventManager
{
    private static EventHandlers handlers = new EventHandlers();

    public static void ResetHandlers() {
        handlers = new EventHandlers();
    }


    // Trigger method for level completion
    public static void TriggerLevelCompletionAnimation()
    {
        handlers.TriggerLevelCompletionAnimation();
    }

    public static void RegisterLevelCompletionAnimation(Scenes.SceneManager dungenScene)
    {
        handlers.LevelComplete += dungenScene.LevelCompleteScene;
    }

    public static void TriggerLinkDeathAnimation()
    {
        handlers.TriggerLinkDeathAnimation();
    }

    public static void RegisterLinkDeathAnimation(Scenes.SceneManager dungenScene)
    {
        handlers.LinkDeath += dungenScene.LinkDeathScene;
    }

    public static void TriggerWallMasterGrabAnimation()
    {
        handlers.TriggerWallMasterGrabAnimation();
    }


    public static void RegisterWallMasterGrabAnimation(Scenes.SceneManager dungenScene)
    {
        handlers.WallmasterGrab += dungenScene.WallMasterGrabScene;
    }
}
