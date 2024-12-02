
using Microsoft.Xna.Framework;
using MonoZelda.Link;
using MonoZelda.Link.Equippables;

namespace MonoZelda.Shaders
{
    internal class PlayerLight : Light
    {
        public const int MIN_RADIUS = 150;
        public const int MAX_RADIUS = 300;

        private float animatedLightDistance = 0; // Zero gives a nice intro effect

        public override Point Position => PlayerState.Position;

        public override float Radius => GetRadius();

        private float GetRadius()
        {
            var lightDistance = PlayerState.EquippedItem == EquippableType.CandleBlue
                ? MAX_RADIUS : MIN_RADIUS;

            // Animate distance changes
            animatedLightDistance = MathHelper.Lerp(animatedLightDistance, lightDistance, 0.1f);

            return animatedLightDistance;
        }
    }
}