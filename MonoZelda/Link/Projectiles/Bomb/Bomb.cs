using MonoZelda.Sprites;
using Microsoft.Xna.Framework;

namespace MonoZelda.Link.Projectiles;

public class Bomb : Projectile, IProjectile
{
    private bool Finished;
    private int timer;
    private Vector2 InitialPosition;
    private Vector2 Dimension = new Vector2(8, 16);
    private SpriteDict projectileDict;

    public Bomb(SpriteDict projectileDict, Vector2 playerPosition, Direction playerDirection)
    : base(projectileDict, playerPosition, playerDirection)
    {
        this.projectileDict = projectileDict;
        Finished = false;
        timer = 0;
        InitialPosition = SetInitialPosition(Dimension);
        SetProjectileSprite("bomb");
        projectileDict.Position = InitialPosition.ToPoint();
    }

    public bool hasFinished()
    {
        return Finished;
    }

    public void FinishProjectile()
    {
        // Empty
    }

    public Rectangle getCollisionRectangle()
    {
        Point spawnPosition = projectilePosition.ToPoint();
        return new Rectangle(spawnPosition.X - 32 / 2, spawnPosition.Y - 64 / 2, 32, 64);
    }

    public void UpdateProjectile()
    {
        if (timer >= 90 && timer < 100)
        {
            SetProjectileSprite("cloud");
        }
        else if(timer == 100)
        {
            Finished = true;
            projectileDict.SetSprite("");
            projectileDict.Enabled = false;
        }
        timer++;
    }
}
