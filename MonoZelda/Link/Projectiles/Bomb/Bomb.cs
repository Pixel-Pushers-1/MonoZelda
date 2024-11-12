using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Sound;
using System.Diagnostics;

namespace MonoZelda.Link.Projectiles;

public class Bomb : ProjectileFactory, IProjectile
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
    private Vector2 dimension = new Vector2(8, 16);
    private SpriteDict projectileDict;
    private SpriteDict[] explosionSpriteDicts;
    private bool exploded;

    public bool Exploded
    {
        get => exploded;
    }

    public Bomb(SpriteDict projectileDict, Vector2 playerPosition, Direction playerDirection)
    : base(projectileDict, playerPosition, playerDirection)
    {
        this.projectileDict = projectileDict;
        finished = false;
        explosionTimer = EXPLODE_TIME;
        initialPosition = SetInitialPosition(dimension);
        SetProjectileSprite("bomb");
        projectileDict.Position = initialPosition.ToPoint();
        exploded = false;
    }

    public bool hasFinished()
    {
        return finished;
    }

    public void FinishProjectile()
    {
        // Empty
    }

    public Rectangle getCollisionRectangle()
    {
        Point spawnPosition = projectilePosition.ToPoint();
        return new Rectangle(spawnPosition.X - 64 / 2, spawnPosition.Y - 64 / 2, 64, 64);
    }

    public void UpdateProjectile()
    {
        if (explosionTimer <= 0f && !exploded)
        {
            Explode();
        }

        if(animationTimer <= 0f && exploded)
        {
            finished = true;

            //clean up spritedicts
            projectileDict.Enabled = false;
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
