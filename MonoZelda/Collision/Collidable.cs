using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoZelda.Collision
{
    public class Collidable : ICollidable
    {
        public string name { get; set; }
        public Rectangle Bounds { get; set; }

        private readonly CollisionHitboxDraw hitbox;

        public Collidable(Rectangle bounds, GraphicsDevice graphicsDevice, string name)
        {
            Bounds = bounds;
            hitbox = new CollisionHitboxDraw(this, graphicsDevice);
            this.name = name;
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
            return $"{name}";
        }
    }
}
