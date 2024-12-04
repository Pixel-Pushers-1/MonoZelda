using Microsoft.Xna.Framework;
using MonoZelda.Link;
using MonoZelda.Link.Equippables;
using MonoZelda.Link.Projectiles;

namespace MonoZelda.Shaders
{
    internal class ProjectileLight : Light
    {


        private IProjectile projectile;

        public ProjectileLight(IProjectile projectile)
        {
            this.projectile = projectile;

            Radius = 400;
            Color = Color.Orange;
            Intensity = 2;
        }

        public override Point Position => projectile.ProjectilePosition.ToPoint();
    }
}
