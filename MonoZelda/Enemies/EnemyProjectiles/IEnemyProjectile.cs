﻿using Microsoft.Xna.Framework;
using MonoZelda.Collision;

namespace MonoZelda.Enemies.EnemyProjectiles;

public interface IEnemyProjectile
{
    public Collidable ProjectileHitbox { get; set; }
    public Point Pos { get; set; }

    public void ViewProjectile(bool view);
    public void Follow(Point newPos);

    public void Update(GameTime gameTime, CardinalEnemyStateMachine.Direction attackDirection, Point enemyPos);
}