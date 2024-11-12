using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Controllers;
using System;
using MonoZelda.Commands;
using MonoZelda.Commands.GameCommands;
using MonoZelda.Dungeons;

namespace MonoZelda.Trigger
{
    public static class TriggerFactory
    {
        public static ITrigger CreateTrigger(TriggerSpawn spawn, CollisionController colliderManager, ICommand transitionCommand)
        {
            switch (spawn.Type)
            {
                case TriggerType.push_block:
                    return new PushBlockTrigger(colliderManager, spawn.Position);
                case TriggerType.mario_stairs:
                    return new MarioRoomStairsTrigger(spawn, colliderManager, transitionCommand);
                default:
                    throw new ArgumentException("Invalid trigger type");
            }
        }
    }
}
