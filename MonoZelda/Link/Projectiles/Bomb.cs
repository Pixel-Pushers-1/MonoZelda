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
    private Player player;

    public Bomb(SpriteDict projectileDict, Player player) : base(projectileDict, player)
    {
        this.projectileDict = projectileDict;
        this.player = player;
        Finished = false;
        SetProjectileSprite("bomb");
        timer = 0;
        InitialPosition = SetInitialPosition(Dimension);
    }

    private void updatePosition()
    {
        projectileDict.Position = InitialPosition.ToPoint(); ;
        Finished = reachedDistance();
    }

    public void UpdateProjectile()
    {
        if (timer < 90)
        {
            updatePosition();
        }
        else if (timer >= 90 && timer < 100)
        {
            SetProjectileSprite("cloud");
        }
        else
        {
            Finished = reachedDistance();
        }
        timer++;

    }

    public bool reachedDistance()
    {
        bool reachedDistance = false;
        if (timer == 100)
        {
            reachedDistance = true;
            projectileDict.Enabled = false;
        }
        return reachedDistance;
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
}
