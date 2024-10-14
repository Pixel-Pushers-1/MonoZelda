using Microsoft.Xna.Framework;

namespace PixelPushers.MonoZelda.Collision.Collidables
{
    public interface ICollidable
    {
        string name { get; set; }
        Rectangle Bounds { get; set; }
        bool Intersects(ICollidable other);
        Rectangle GetIntersectionArea(ICollidable other);
    }
}
