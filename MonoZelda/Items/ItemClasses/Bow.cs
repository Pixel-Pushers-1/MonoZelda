using MonoZelda.Controllers;
using MonoZelda.Sound;
using MonoZelda.Link;
using MonoZelda.Dungeons;
using Microsoft.Xna.Framework;
using MonoZelda.Link.Equippables;

namespace MonoZelda.Items.ItemClasses;

public class Bow : Item
{
    private PlayerCollisionManager playerCollision;
    private const float PICKUP_TIME = 3f;
    private float timer;

    public Bow(ItemManager itemManager) : base(itemManager)
    {
        itemType = ItemList.Bow;
    }

    public override void ItemSpawn(ItemSpawn itemSpawn, CollisionController collisionController)
    {
        itemBounds = new Rectangle(itemSpawn.Position, new Point(24, 56));
        base.ItemSpawn(itemSpawn, collisionController);
        itemDict.SetSprite("bow");
        playerCollision = itemManager.PlayerCollision;
    }

    public override void HandleCollision(CollisionController collisionController)
    {
        // update playerSprite and itemUpdateList
        PlayerState.EquippableManager.AddEquippable(EquippableType.Bow, false);
        playerCollision.HandleBowCollision(itemDict);
        itemManager.AddUpdateItem(this);
        timer = PICKUP_TIME;

        // Play sounds
        SoundManager.PlaySound("LOZ_New_Weapon_Recieved", false);
        SoundManager.Pause("LOZ_Dungeon_Theme");

        // Unregister collidables and remove from collisionController
        itemCollidable.UnregisterHitbox();
        collisionController.RemoveCollidable(itemCollidable);

        // remove item from roomSpawn list
        itemManager.RemoveRoomSpawnItem(itemSpawn);
    }

    public override void Update()
    {
        timer -= (float)MonoZeldaGame.GameTime.ElapsedGameTime.TotalSeconds;
        if(timer <= 0)
        {
            itemDict.Unregister();
            itemManager.RemoveUpdateItem(this);

            if(itemManager.GameMode == GameType.Classic)
            {
                SoundManager.PlaySound("LOZ_Dungeon_Theme", true);
            }
        }
    }
}