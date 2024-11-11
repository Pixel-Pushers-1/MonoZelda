using Microsoft.Xna.Framework;
using MonoZelda.Collision;

namespace MonoZelda.Enemies.EnemyProjectiles;

public interface IEnemyProjectile
{
    public EnemyProjectileCollidable ProjectileHitbox { get; set; }
    public Point Pos { get; set; }
    public bool Active {get; set; }

    public void ViewProjectile(bool view, bool enemyAlive);

    public void ProjectileCollide();

    public void Update (EnemyStateMachine.Direction attackDirection, Point enemyPos);
}