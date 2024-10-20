using Microsoft.Xna.Framework;
using MonoZelda.Sprites;

namespace MonoZelda.Collision.Collidables
{
    public enum CollidableType
    {
        Player,
        Enemy,
        Item,
        Projectile,
        Static,
    }

    public interface ICollidable
    {
        CollidableType type { get; set; }
        Rectangle Bounds { get; set; }
        SpriteDict CollidableDict { get; set; }
        bool Intersects(ICollidable other);
        Rectangle GetIntersectionArea(ICollidable other);

        void setSpriteDict(SpriteDict collidableDict);
    }
}
