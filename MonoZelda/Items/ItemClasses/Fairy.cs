using Microsoft.Xna.Framework;
using MonoZelda.Controllers;
using MonoZelda.Sound;
using MonoZelda.Enemies;
using MonoZelda.Link;
using System.Collections.Generic;
using System;
using MonoZelda.Dungeons;

namespace MonoZelda.Items.ItemClasses;

public class Fairy : Item
{
    private Random rnd;
    private int randomNum;
    private float timer;
    private float directionUpdateTimer;
    private const float FAIRY_ALIVE = 3f;
    private const float FAIRY_SPEED = 12f;

    private Dictionary<int, Vector2> DirectionVector = new()
    {
        { 1, new Vector2(-1,0) },
        { 2, new Vector2(1,0) },
        { 3, new Vector2(0,-1) },
        { 4, new Vector2(0,1) },
    };

    public Fairy(ItemManager itemManager) : base(itemManager)
    {
        itemType = ItemList.Fairy;
        rnd = new Random(); 
    }

    public override void ItemSpawn(ItemSpawn itemSpawn, CollisionController collisionController)
    {
        itemBounds = new Rectangle(itemSpawn.Position, new Point(24,56));
        base.ItemSpawn(itemSpawn, collisionController);
        itemDict.SetSprite("fairy");
    }

    public override void HandleCollision(CollisionController collisionController)
    {
        // Add fairy as an update item in itemManager list
        itemManager.AddUpdateItem(this);
        timer = FAIRY_ALIVE;
        randomNum = rnd.Next(1,5);

        // update playerHealth and play Fairy Sound
        PlayerState.Health = PlayerState.MaxHealth;
        SoundManager.PlaySound("LOZ_Fairy_Heal", false);

        // unregister collidable and remove from collisionController
        itemCollidable.UnregisterHitbox();
        collisionController.RemoveCollidable(itemCollidable);

        // remove item from roomSpawn list
        itemManager.RemoveRoomSpawnItem(itemSpawn);
    }

    public override void Update()
    {
        timer -= (float)MonoZeldaGame.GameTime.ElapsedGameTime.TotalSeconds;
        directionUpdateTimer += (float)MonoZeldaGame.GameTime.ElapsedGameTime.TotalSeconds;
        Vector2 fairyPosition = itemDict.Position.ToVector2();
        if (timer > 0)
        {
            // get movement vector
            if(directionUpdateTimer > 0.1f)
            {
                randomNum = rnd.Next(1, 5);
                directionUpdateTimer = 0f;
            }
            Vector2 movementVector = DirectionVector[randomNum];

            // update fairy position
            fairyPosition += FAIRY_SPEED * movementVector;
            itemDict.Position = fairyPosition.ToPoint();
        }
        else
        {
            itemDict.Unregister();
            itemManager.RemoveUpdateItem(this);
        }
        
    }
}
