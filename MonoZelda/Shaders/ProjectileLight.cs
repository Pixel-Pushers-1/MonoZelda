using Microsoft.Xna.Framework;
using MonoZelda.Link;
using MonoZelda.Link.Equippables;
using MonoZelda.Link.Projectiles;

namespace MonoZelda.Shaders
{
    internal class ProjectileLight : Light
    {
        public const int MIN_RADIUS = 150;
        public const int MAX_RADIUS = 300;

        private float animatedLightDistance = 0; // Zero gives a nice intro effect

        private IProjectile projectile;

        public ProjectileLight(IProjectile projectile)
        {
            this.projectile = projectile;
        }

        public override Point Position => projectile.ProjectilePosition.ToPoint();

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
