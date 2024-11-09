﻿using Microsoft.Xna.Framework;
using MonoZelda.Collision;

namespace MonoZelda.Enemies.EnemyProjectiles;

public interface IEnemyProjectile
{
    public EnemyProjectileCollidable ProjectileHitbox { get; set; }
    public Point Pos { get; set; }
    public bool Active {get; set; }

    public void ViewProjectile(bool view, bool enemyAlive);
    public void Follow(Point newPos);

    public void ProjectileCollide();

    public void Update(GameTime gameTime, EnemyStateMachine.Direction attackDirection, Point enemyPos);
}