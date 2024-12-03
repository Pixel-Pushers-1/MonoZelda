using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;
using MonoZelda.Sound;
using MonoZelda.Enemies;
using MonoZelda.Link;
using System.Collections.Generic;
using MonoZelda.Dungeons;
using System;

namespace MonoZelda.Items.ItemClasses;

public class XPOrb : Item
{
    private PlayerCollisionManager playerCollision;

    public XPOrb(ItemManager itemManager) : base(itemManager)
    {
        itemType = ItemList.XPOrb;
    }

    public override void ItemSpawn(ItemSpawn itemSpawn, CollisionController collisionController)
    {
        playerCollision = itemManager.PlayerCollision;
        Random random = new Random();
        int offsetX = random.Next(-50, 51);
        int offsetY = random.Next(-50, 51); 

        Point randomizedPosition = new Point(itemSpawn.Position.X + offsetX, itemSpawn.Position.Y + offsetY);

        //need to update so sprite match hitbox?
        itemBounds = new Rectangle(randomizedPosition, new Point(36, 36)); 
        base.ItemSpawn(itemSpawn, collisionController);
        itemDict.SetSprite("xp_orb");
        itemDict.Position = randomizedPosition;
        

    }

    public override void HandleCollision(CollisionController collisionController)
    {
        bool leveledUp = PlayerState.AddXP(40);
        SoundManager.PlaySound("LOZ_orb", false);
        if (leveledUp)
        {
            playerCollision.HandleLevelUp();
        }
        //SoundManager.PlaySound("XP_Collect_Sound", false); // Replace with actual XP collect sound
        base.HandleCollision(collisionController);
    }
}
