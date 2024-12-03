using Microsoft.Xna.Framework;
using MonoZelda.Sprites;
using MonoZelda.Items;
using MonoZelda.Items.ItemClasses;
using MonoZelda.Controllers;

namespace MonoZelda.Collision;

public class ItemCollidable : ICollidable
{
    public CollidableType type { get; set; }
    public Rectangle Bounds { get; set; }
    public SpriteDict CollidableDict { get; set; }

    private ItemList itemType;
    private Item item;
    private readonly CollisionHitboxDraw hitbox;

    public ItemCollidable(Rectangle bounds, ItemList itemType)
    {
        Bounds = bounds;
        hitbox = new CollisionHitboxDraw(this);
        type = CollidableType.Item;
        this.itemType = itemType;
    }

    public void setItem(Item item)
    {
        this.item = item;
    }
    public void HandleCollision(CollisionController collisionController)
    {
        item.HandleCollision(collisionController);
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
