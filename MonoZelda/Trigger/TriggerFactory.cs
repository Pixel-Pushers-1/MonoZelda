using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Controllers;
using System;

namespace MonoZelda.Trigger
{
    public static class TriggerFactory
    {
        public static ITrigger CreateTrigger(TriggerType type, CollisionController colliderManager, ContentManager contentManager, Point position, GraphicsDevice graphicsDevice)
        {
            switch (type)
            {
                case TriggerType.push_block:
                    return new PushBlockTrigger(contentManager, colliderManager, position, graphicsDevice);
                default:
                    throw new ArgumentException("Invalid trigger type");
            }
        }
    }
}
