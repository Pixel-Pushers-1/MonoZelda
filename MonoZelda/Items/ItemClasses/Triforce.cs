using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;
using MonoZelda.Sound;
using MonoZelda.Link;
using MonoZelda.Dungeons;
using MonoZelda.Collision;
using System;

namespace MonoZelda.Items.ItemClasses;

public class Triforce : Item
{
    private float FLASHING_TIME = 0.75f;
    private PlayerCollisionManager playerCollision;
    private event Action LevelComplete;

    public Triforce(ItemManager itemManager) : base(itemManager)
    {
        itemType = ItemList.Triforce;
        playerCollision = itemManager.PlayerCollision;
    }

    public override void ItemSpawn(ItemSpawn itemSpawn, CollisionController collisionController)
    {
        // create item SpriteDict
        itemDict = new SpriteDict(SpriteType.Items, SpriteLayer.Items, itemSpawn.Position + new Point(32, 12));
        itemDict.SetFlashing(SpriteDict.FlashingType.OnOff, FLASHING_TIME);

        // create item Collidable 
        itemCollidable = new ItemCollidable(itemBounds, itemType);
        collisionController.AddCollidable(itemCollidable);
    }

    //private void InitializeSpriteDicts()
    //{
    //    // make curtains
    //    var leftPosition = new Point(-512, 192);
    //    var rightPosition = new Point(1024, 192);
    //    var curtainSize = new Point(512, 704);
    //    leftCurtain = new BlankSprite(SpriteLayer.Triforce - 1, leftPosition, curtainSize, Color.Black);
    //    rightCurtain = new BlankSprite(SpriteLayer.Triforce - 1, rightPosition, curtainSize, Color.Black);

    //    // create fake Link and Triforce
    //    FakeLink = new SpriteDict(SpriteType.Player, SpriteLayer.Triforce, PlayerState.Position);
    //    FakeLink.SetSprite("pickupitem_twohands");
    //    FakeTriforce = new SpriteDict(SpriteType.Items, SpriteLayer.Triforce, PlayerState.Position + new Point(-32, -84));
    //    FakeTriforce.SetSprite("triforce");
    //}

    public override void HandleCollision(CollisionController collisionController)
    {
        // unregister collidable and remove from collisionController
        itemDict.Unregister();
        itemCollidable.UnregisterHitbox();
        collisionController.RemoveCollidable(itemCollidable);

        // remove item from roomSpawn list
        itemManager.RemoveRoomSpawnItem(itemSpawn);

        // call the end game envent
        LevelComplete?.Invoke();
    }
}
