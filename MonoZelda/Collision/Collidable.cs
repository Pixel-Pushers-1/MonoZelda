using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoZelda.Collision
{

    public enum CollidableType {
        Player,
        Item,
        Enemy,
        Projectile,

    }
    public class Collidable : ICollidable
    {
        public CollidableType type { get; set; }
        public Rectangle Bounds { get; set; }

        private readonly CollisionHitboxDraw hitbox;

        public Collidable(Rectangle bounds, GraphicsDevice graphicsDevice, CollidableType type)
        {
            Bounds = bounds;
            hitbox = new CollisionHitboxDraw(this, graphicsDevice);
            this.type = type;
        }

        public bool Intersects(ICollidable other)
        {
            return Bounds.Intersects(other.Bounds);
        }

        public Rectangle GetIntersectionArea(ICollidable other)
        {
            return Rectangle.Intersect(Bounds, other.Bounds);
        }

        public void SetGizmoColor(Color color) {
            hitbox.GizmoColor = color;
        }

        public void SetGizmoThickness(int thickness) {
            hitbox.Thickness = thickness;
        }

        public override string ToString()
        {
            return $"{type}";
        }
    }
}
