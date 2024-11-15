using Microsoft.Xna.Framework;
using MonoZelda.Controllers;
using MonoZelda.Dungeons;
using MonoZelda.Sound;
using MonoZelda.UI;

namespace MonoZelda.Items.ItemClasses;

public class Compass : Item
{
    public Compass(ItemManager itemManager) : base(itemManager)
    {
        itemType = ItemList.Compass;
    }

    public override void ItemSpawn(ItemSpawn itemSpawn, CollisionController collisionController)
    {
        itemBounds = new Rectangle(itemSpawn.Position, new Point(56, 56));
        base.ItemSpawn(itemSpawn, collisionController);
        itemDict.SetSprite("compass");
    }

    public override void HandleCollision(CollisionController collisionController)
    {
        HUDMapWidget.SetCompassMarkerVisible(true);
        SoundManager.PlaySound("LOZ_Get_Item", false);
        base.HandleCollision(collisionController);
    }
}
