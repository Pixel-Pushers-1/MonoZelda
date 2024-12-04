using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Sound;
using MonoZelda.Controllers;
using MonoZelda.Collision;

namespace MonoZelda.Link.Projectiles;

public class Bomb : IProjectile
{
    private const float EXPLODE_TIME = 1.5f;

    private static readonly Point[] explosionCloudPositions = new Point[] {
        new(-64, 0),
        new(-32, 64),
        new(32, 64),
        new(64, 0),
        new(32, -64),
        new(-32, -64),
    };

    private bool finished;
    private float explosionTimer;
    private float animationTimer;
    private Vector2 initialPosition;
    private Vector2 projectilePosition;
    private PlayerProjectileCollidable projectileCollidable;
    private SpriteDict projectileDict;
    private SpriteDict[] explosionSpriteDicts;
    private CollisionController collisionController;
    private bool exploded;
    private bool bombAte;

    public Vector2 ProjectilePosition
    {
        get { return projectilePosition; }
        set { projectilePosition = value; }
    }

    public bool Exploded
    {
        get => exploded;
    }

    public Bomb(Vector2 spawnPosition, CollisionController collisionController)
    {
        finished = false;
        explosionTimer = EXPLODE_TIME;
        initialPosition = spawnPosition;
        exploded = false;
        this.collisionController = collisionController;
    }

    public bool hasFinished()
    {
        return finished;
    }

    public void FinishProjectile()
    {
        bombAte = true;
    }

    public Rectangle getCollisionRectangle()
    {
        Point spawnPosition = projectilePosition.ToPoint();
        return new Rectangle(spawnPosition.X - 192 / 2, spawnPosition.Y - 192 / 2, 192, 192);
    }

    public void Setup(params object[] args)
    {
        projectilePosition = initialPosition;
        SoundManager.PlaySound("LOZ_Bomb_Drop", false);
        projectileDict = new SpriteDict(SpriteType.Projectiles, SpriteLayer.Projectiles, initialPosition.ToPoint());
        Point spawnPosition = projectilePosition.ToPoint();
        projectileCollidable = new PlayerProjectileCollidable(new Rectangle(spawnPosition.X - 64 / 2, spawnPosition.Y - 64 / 2, 64, 64), ProjectileType.Bomb);
        projectileCollidable.setProjectile(this);
        collisionController.AddCollidable(projectileCollidable);
        projectileDict.SetSprite("bomb");

    }

    public void Update()
    {
        if(bombAte){
            projectileDict.Unregister();
            projectileCollidable.UnregisterHitbox();
            collisionController.RemoveCollidable(projectileCollidable);
        }
        if (explosionTimer <= 0f && !exploded)
        {
            projectileCollidable.UnregisterHitbox();
            collisionController.RemoveCollidable(projectileCollidable);
            Explode();
        }
        if(animationTimer <= 0f && exploded)
        {
            finished = true;

            //clean up spritedicts
            projectileDict.Unregister();
            projectileCollidable.UnregisterHitbox();
            collisionController.RemoveCollidable(projectileCollidable);
            for (int i = 0; i < explosionSpriteDicts.Length; i++) {
                explosionSpriteDicts[i].Unregister();
                explosionSpriteDicts[i] = null;
            }
        }

        if (explosionTimer > 0f)
            explosionTimer -= (float) MonoZeldaGame.GameTime.ElapsedGameTime.TotalSeconds;
        if (animationTimer > 0f)
            animationTimer -= (float) MonoZeldaGame.GameTime.ElapsedGameTime.TotalSeconds;
    }

    private void Explode() {
        exploded = true;
        projectileCollidable = new PlayerProjectileCollidable(getCollisionRectangle(), ProjectileType.BombExplosion);
        projectileCollidable.setProjectile(this);
        collisionController.AddCollidable(projectileCollidable);
        SoundManager.PlaySound("LOZ_Bomb_Blow", false);
        animationTimer = (float) projectileDict.SetSpriteOneshot("cloud_slow");

        //set up explosion clouds
        explosionSpriteDicts = new SpriteDict[6];
        for (int i = 0; i < explosionSpriteDicts.Length; i++) {
            explosionSpriteDicts[i] = new(SpriteType.Projectiles, SpriteLayer.Projectiles, initialPosition.ToPoint() + explosionCloudPositions[i]);
            explosionSpriteDicts[i].SetSprite("cloud_slow");
            explosionSpriteDicts[i].FlashingRate = 30f;
            explosionSpriteDicts[i].SetFlashing(SpriteDict.FlashingType.OnOff, animationTimer);
        }
        BlankSprite bs = new(SpriteLayer.Gizmos, new Point(0, 193), new Point(1024, 704), new Color(.75f, .75f, .75f, .75f));
        bs.FadeOut(animationTimer);
    }
}
