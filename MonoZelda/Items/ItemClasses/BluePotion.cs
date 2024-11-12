using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;
using MonoZelda.Sound;
using MonoZelda.Enemies;
using MonoZelda.Link;
using System.Collections.Generic;

namespace MonoZelda.Items.ItemClasses;

public class BluePotion : Item
{
    public BluePotion(List<IEnemy> roomEnemyList, PlayerCollisionManager playerCollision, List<Item> updateList) : base(roomEnemyList, playerCollision, updateList)
    {
        itemType = ItemList.BluePotion;
    }

    public override void ItemSpawn(SpriteDict bluepotionDict, Point spawnPosition, CollisionController collisionController)
    {
        base.ItemSpawn(bluepotionDict, spawnPosition, collisionController); 
        bluepotionDict.SetSprite("potion_blue");
    }

    public override void HandleCollision(SpriteDict itemCollidableDict, CollisionController collisionController)
    {
        PlayerState.Health = PlayerState.MaxHealth;
        SoundManager.PlaySound("LOZ_Get_Item", false);
        base.HandleCollision(itemCollidableDict, collisionController);
    }
}