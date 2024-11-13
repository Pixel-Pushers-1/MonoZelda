using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;
using MonoZelda.Sound;
using MonoZelda.Enemies;
using MonoZelda.Link;
using System.Collections.Generic;

namespace MonoZelda.Items.ItemClasses;
public class Bow : Item
{
    private const float PICKUP_TIME = 3f;
    private SpriteDict bowDict;
    private float timer;

    public Bow(List<Enemy> roomEnemyList, PlayerCollisionManager playerCollision, List<Item> updateList) : base(roomEnemyList, playerCollision, updateList)
    {
        itemType = ItemList.Bow;
    }

    public override void ItemSpawn(SpriteDict bowDict, Point spawnPosition, CollisionController collisionController)
    {
        base.ItemSpawn(bowDict, spawnPosition, collisionController);
        bowDict.SetSprite("bow");
        this.bowDict = bowDict;
    }

    public override void HandleCollision(SpriteDict itemCollidableDict, CollisionController collisionController)
    {
        updateList.Add(this);
        timer = PICKUP_TIME;
        playerCollision.HandleBowCollision(bowDict);
        SoundManager.PlaySound("LOZ_New_Weapon_Recieved", false);
        SoundManager.Pause("LOZ_Dungeon_Theme");
        itemCollidable.UnregisterHitbox();
        collisionController.RemoveCollidable(itemCollidable);
    }

    public override void Update()
    {
        timer -= (float)MonoZeldaGame.GameTime.ElapsedGameTime.TotalSeconds;
        if(timer <= 0)
        {
            bowDict.Unregister();
            updateList.Remove(this);
            SoundManager.PlaySound("LOZ_Dungeon_Theme", true);
        }
    }
}