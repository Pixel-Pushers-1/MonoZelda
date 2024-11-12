using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;
using MonoZelda.Sound;
using MonoZelda.Enemies;
using MonoZelda.Link;
using System.Collections.Generic;
using System;

namespace MonoZelda.Items.ItemClasses;

public class Fairy : Item
{
    private Random rnd;
    private const float FAIRY_ALIVE = 3f;
    private float timer;
    private float directionUpdateTimer;
    private int randomNum;
    private SpriteDict fairyDict;
    private const float FAIRY_SPEED = 12f;

    private Dictionary<int, Vector2> DirectionVector = new()
    {
        { 1, new Vector2(-1,0) },
        { 2, new Vector2(1,0) },
        { 3, new Vector2(0,-1) },
        { 4, new Vector2(0,1) },
    };

    public Fairy(List<IEnemy> roomEnemyList, PlayerCollisionManager playerCollision, List<Item> updateList) : base(roomEnemyList, playerCollision, updateList)
    {
        itemType = ItemList.Fairy;
        rnd = new Random(); 
    }

    public override void ItemSpawn(SpriteDict fairyDict, Point spawnPosition, CollisionController collisionController)
    {
        base.ItemSpawn(fairyDict, spawnPosition, collisionController);
        fairyDict.SetSprite("fairy");
        this.fairyDict = fairyDict;
    }

    public override void HandleCollision(SpriteDict itemCollidableDict, CollisionController collisionController)
    {
        updateList.Add(this);
        timer = FAIRY_ALIVE;
        randomNum = 1;
        PlayerState.Health = PlayerState.MaxHealth;
        SoundManager.PlaySound("LOZ_Fairy_Heal", false);
        itemCollidable.UnregisterHitbox();
        collisionController.RemoveCollidable(itemCollidable);
    }

    public override void Update()
    {
        timer -= (float)MonoZeldaGame.GameTime.ElapsedGameTime.TotalSeconds;
        directionUpdateTimer += (float)MonoZeldaGame.GameTime.ElapsedGameTime.TotalSeconds;
        Vector2 fairyPosition = fairyDict.Position.ToVector2();
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
            fairyDict.Position = fairyPosition.ToPoint();
        }
        else
        {
            fairyDict.Unregister();
            updateList.Remove(this);
        }
        
    }
}
