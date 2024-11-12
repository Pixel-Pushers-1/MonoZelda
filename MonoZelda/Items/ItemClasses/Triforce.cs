using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;
using MonoZelda.Sound;
using MonoZelda.Enemies;
using MonoZelda.Link;
using System.Collections.Generic;
using MonoZelda.Dungeons;

namespace MonoZelda.Items.ItemClasses;

public class Triforce : Item
{
    private float END_SCENE_TIMER = 25f;
    private SpriteDict triforceDict;
    private SpriteDict leftCurtain;
    private SpriteDict rightCurtain;
    private SpriteDict FakeLink;
    private SpriteDict FakeTriforce;
    private float timer;

    public Triforce(List<Enemy> roomEnemyList, PlayerCollisionManager playerCollision, List<Item> updateList) : base(roomEnemyList, playerCollision, updateList)
    {
        itemType = ItemList.Triforce;
    }

    public override void ItemSpawn(SpriteDict triforceDict, Point spawnPosition, CollisionController collisionController)
    {
        base.ItemSpawn(triforceDict, spawnPosition + new Point(32,12), collisionController);   
        triforceDict.SetSprite("triforce");
        this.triforceDict = triforceDict;
    }

    private void InitializeSpriteDicts()
    {
        // make curtains
        leftCurtain = new SpriteDict(SpriteType.Blocks, SpriteLayer.HUD, DungeonConstants.BackgroundPosition + DungeonConstants.adjacentTransitionRoomSpawnPoints[Direction.Right]);
        leftCurtain.SetSprite("room_41");
        rightCurtain = new SpriteDict(SpriteType.Blocks, SpriteLayer.HUD, DungeonConstants.BackgroundPosition + DungeonConstants.adjacentTransitionRoomSpawnPoints[Direction.Left]);
        rightCurtain.SetSprite("room_41");

        // create fake Link and Triforce
        FakeLink = new SpriteDict(SpriteType.Player, SpriteLayer.Triforce, PlayerState.Position);
        FakeLink.SetSprite("pickupitem_twohands");
        FakeTriforce = new SpriteDict(SpriteType.Items, SpriteLayer.Triforce, PlayerState.Position + new Point(-32, -84));
        FakeTriforce.SetSprite("triforce");
    }

    public override void HandleCollision(SpriteDict itemCollidableDict, CollisionController collisionController)
    {
        timer = END_SCENE_TIMER;
        InitializeSpriteDicts();
        updateList.Add(this);
        triforceDict.Unregister();
        SoundManager.ClearSoundDictionary();
        SoundManager.PlaySound("LOZ_Victory", false);
        playerCollision.HandleTriforceCollision();
        itemCollidable.UnregisterHitbox();
        collisionController.RemoveCollidable(itemCollidable);
    }

    public override void Update()
    {
        timer -= (float)MonoZeldaGame.GameTime.ElapsedGameTime.TotalSeconds;
        if (timer > 0) 
        {
            if (leftCurtain.Position.X != 0 && rightCurtain.Position.X != 512)
            {
                leftCurtain.Position += new Point(4, 0);
                rightCurtain.Position += new Point(-4, 0);
            }
        }
        else
        {
            PlayerState.ObtainedTriforce = true;
        }
    }

}
