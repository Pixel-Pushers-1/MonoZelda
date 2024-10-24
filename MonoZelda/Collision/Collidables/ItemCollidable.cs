using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoZelda.Sprites;
using MonoZelda.Items;

namespace MonoZelda.Collision;

public class ItemCollidable : ICollidable
{
    public CollidableType type { get; set; }
    public ItemList itemType { get; set; }
    public Rectangle Bounds { get; set; }
    public SpriteDict CollidableDict { get; private set; }

    private readonly CollisionHitboxDraw hitbox;

    public ItemCollidable(Rectangle bounds, GraphicsDevice graphicsDevice, ItemList itemType)
    {
        Bounds = bounds;
        hitbox = new CollisionHitboxDraw(this, graphicsDevice);
        type = CollidableType.Item;
        this.itemType = itemType;
    }

    public void UnregisterHitbox()
    {
        hitbox.Unregister();
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
