using Microsoft.Xna.Framework;
using MonoZelda.Controllers;
using MonoZelda.Sound;
using MonoZelda.Enemies;
using MonoZelda.Link;
using System.Collections.Generic;
using MonoZelda.Dungeons;

namespace MonoZelda.Items.ItemClasses;

public class Heart : Item
{
    public Heart(ItemManager itemManager) : base(itemManager)
    {
        itemType = ItemList.Heart;
    }

    public override void ItemSpawn(ItemSpawn itemSpawn, CollisionController collisionController)
    {
        base.ItemSpawn(itemSpawn, collisionController);  
        itemDict.SetSprite("heart_full");
    }

    public override void HandleCollision(CollisionController collisionController)
    {
        PlayerState.Health = MathHelper.Clamp(PlayerState.Health + 2, 0, PlayerState.MaxHealth);
        SoundManager.PlaySound("LOZ_Get_Heart", false);
        base.HandleCollision(collisionController);
    }
}