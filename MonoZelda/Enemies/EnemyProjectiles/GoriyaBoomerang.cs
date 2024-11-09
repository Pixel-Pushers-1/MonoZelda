using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Collision;
using MonoZelda.Controllers;
using MonoZelda.Enemies.EnemyProjectiles;
using MonoZelda.Sprites;

namespace MonoZelda.Enemies.GoriyaFolder
{
    public class GoriyaBoomerang : IEnemyProjectile
    {
        public Point Pos { get; set; }
        public SpriteDict BoomerangSpriteDict { get; private set; }
        public bool Active { get; set; }
        public EnemyProjectileCollidable ProjectileHitbox { get; set; }
        private EnemyStateMachine.Direction attackDirection;
        private float velocity = 360;
        private float attackTimer;
        private float dt;
        private Boolean returning;
        private CollisionController collisionController;
        private Vector2 movement;

        public GoriyaBoomerang(Point pos, CollisionController collisionController, EnemyStateMachine.Direction direction)
        {
            
            movement = direction switch
            {
                EnemyStateMachine.Direction.Up => new Vector2(0, -1),
                EnemyStateMachine.Direction.Down => new Vector2(0, 1),
                EnemyStateMachine.Direction.Left => new Vector2(-1, 0),
                EnemyStateMachine.Direction.Right => new Vector2(1, 0),
                _ => Vector2.Zero
            };
            this.Pos = (pos.ToVector2() + movement*30).ToPoint();
            returning = false;
            this.collisionController = collisionController;
            BoomerangSpriteDict = new(SpriteType.Enemies, 0, new Point(0, 0));
            BoomerangSpriteDict.SetSprite("boomerang");
            ViewProjectile(false, true);
            ProjectileHitbox = new EnemyProjectileCollidable(new Rectangle(pos.X, pos.Y, 30, 30));
            collisionController.AddCollidable(ProjectileHitbox);
        }
        public void ViewProjectile(bool view, bool goriyaAlive)
        {
            BoomerangSpriteDict.Enabled = view;
            if (!goriyaAlive)
            {
                collisionController.RemoveCollidable(ProjectileHitbox);
                ProjectileHitbox.UnregisterHitbox();
            }
        }

        public void Follow(Point newPos)
        {
            returning = false;
            Pos = newPos;
            velocity = Math.Abs(velocity);
            attackTimer = 0;
        }

        public void ProjectileCollide()
        {
            if (!returning)
            {
                velocity *= -1;
                returning = true;
            }
        }

        public void Update(GameTime gameTime, EnemyStateMachine.Direction direction, Point enemyPos)
        {
            attackDirection = direction;
            dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            attackTimer += dt;
            var pos = new Vector2();
            pos = Pos.ToVector2();
            if (attackTimer < 1 || velocity < 0)
            {
                pos += (velocity * movement) * dt;
            }
            else
            {
                returning = true;
                velocity *= -1;
                attackTimer = 0;
            }
            Pos = pos.ToPoint();
            BoomerangSpriteDict.Position = Pos;
        }
    }
}