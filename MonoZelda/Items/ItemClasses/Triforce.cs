using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;
using MonoZelda.Sound;
using MonoZelda.Link;
using MonoZelda.Dungeons;
using MonoZelda.Collision;

namespace MonoZelda.Items.ItemClasses;

public class Triforce : Item
{
    private float FLASHING_TIME = 0.75f;
    private float END_SCENE_TIMER = 25f;
    private PlayerCollisionManager playerCollision;
    private SpriteDict triforceDict;
    private BlankSprite leftCurtain;
    private BlankSprite rightCurtain;
    private SpriteDict FakeLink;
    private SpriteDict FakeTriforce;
    private float timer;

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

    private void InitializeSpriteDicts()
    {
        // make curtains
        var leftPosition = new Point(-512, 192);
        var rightPosition = new Point(1024, 192);
        var curtainSize = new Point(512, 704);
        leftCurtain = new BlankSprite(SpriteLayer.Triforce - 1, leftPosition, curtainSize, Color.Black);
        rightCurtain = new BlankSprite(SpriteLayer.Triforce - 1, rightPosition, curtainSize, Color.Black);

        // create fake Link and Triforce
        FakeLink = new SpriteDict(SpriteType.Player, SpriteLayer.Triforce, PlayerState.Position);
        FakeLink.SetSprite("pickupitem_twohands");
        FakeTriforce = new SpriteDict(SpriteType.Items, SpriteLayer.Triforce, PlayerState.Position + new Point(-32, -84));
        FakeTriforce.SetSprite("triforce");
    }

    public override void HandleCollision(CollisionController collisionController)
    {
        timer = END_SCENE_TIMER;
        InitializeSpriteDicts();
        itemManager.AddUpdateItem(this);
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
