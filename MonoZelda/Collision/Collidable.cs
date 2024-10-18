using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Sprites;

namespace MonoZelda.Collision
{
    public enum CollidableType {
        Player,
        Item,
        Enemy,
        Projectile,
        Static
    }
    public class Collidable : ICollidable
    {
        public CollidableType type { get; set; }
        public Rectangle Bounds { get; set; }
        public SpriteDict CollidableDict { get; private set; }

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

        public override string ToString()
        {
            return $"{type}";
        }

        public void setSpriteDict(SpriteDict collidableDict)
        {
            CollidableDict = collidableDict;
        }
    }
}
