using MonoZelda.Sprites;
using Microsoft.Xna.Framework;

namespace MonoZelda.Link.Projectiles.Explosive;

public class Bomb : Projectile, IProjectile
{
    private bool Finished;
    private Vector2 InitialPosition;
    private SpriteDict projectileDict;
    private Player player;
    private int timer;
    private Vector2 Dimension = new Vector2(8, 16);

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
        if (timer < 14)
        {
            updatePosition();
        }
        else if (timer == 15)
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
        if (timer == 16)
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
}
