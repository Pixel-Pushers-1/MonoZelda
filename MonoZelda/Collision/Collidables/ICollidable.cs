using Microsoft.Xna.Framework;
using MonoZelda.Sprites;

namespace MonoZelda.Collision
{
    public interface ICollidable
    {
        CollidableType type { get; set; }
        Rectangle Bounds { get; set; }
        SpriteDict CollidableDict { get; set; }
        void UnregisterHitbox();
        bool Intersects(ICollidable other);
        Rectangle GetIntersectionArea(ICollidable other);
    }
}
