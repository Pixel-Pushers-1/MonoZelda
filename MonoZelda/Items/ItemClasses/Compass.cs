using Microsoft.Xna.Framework;
using MonoZelda.Controllers;
using MonoZelda.Sound;
using MonoZelda.UI;

namespace MonoZelda.Items.ItemClasses;

public class Compass : Item
{
    public Compass(ItemManager itemManager) : base(itemManager)
    {
        itemType = ItemList.Compass;
    }

    public override void ItemSpawn(Point spawnPosition, CollisionController collisionController)
    {
        base.ItemSpawn(spawnPosition, collisionController);
        itemDict.SetSprite("compass");
    }

    public override void HandleCollision(CollisionController collisionController)
    {
        HUDMapWidget.SetCompassMarkerVisible(true);
        SoundManager.PlaySound("LOZ_Get_Item", false);
        base.HandleCollision(collisionController);
    }
}
