using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Link;
using MonoZelda.Sprites;

namespace MonoZelda.Collision.Collidables;

public class PlayerCollidable : ICollidable
{
    public CollidableType type { get; set; }
    public Rectangle Bounds { get; set; }
    public SpriteDict CollidableDict { get; set; }

    private Player player;
    private readonly CollisionHitboxDraw hitbox;

    public PlayerCollidable(Player player, Rectangle bounds, GraphicsDevice graphicsDevice)
    {
        Bounds = bounds;
        type = CollidableType.Player;
        hitbox = new CollisionHitboxDraw(this, graphicsDevice);
        this.player = player;
    }

    public void updatePlayer()
    {
        player.StopPlayer();
    }
    public void setSpriteDict(SpriteDict collidableDict)
    {
        CollidableDict = collidableDict;
    }

    public bool Intersects(ICollidable other)
    {
        return Bounds.Intersects(other.Bounds);
    }

    public Rectangle GetIntersectionArea(ICollidable other)
    {
        return Rectangle.Intersect(Bounds, other.Bounds);
    }
}
