using Microsoft.Xna.Framework;
using MonoZelda.Sprites;

namespace MonoZelda.Link.Projectiles;

public class WoodenSword : Projectile,IProjectile
{
    private bool Finished;
    private int timer;
    private const int HITBOX_TIMER = 4;
    private bool rotate;
    private Vector2 InitialPosition;
    private Vector2 Dimension = new Vector2(8, 16);
    private SpriteDict projectileDict;
    private Player player;

    public WoodenSword(SpriteDict projectileDict, Vector2 playerPosition, Direction playerDirection)
    : base(projectileDict, playerPosition, playerDirection)
    {
        this.projectileDict = projectileDict;
        Finished = false;
        rotate = false;
        timer = 0;
        InitialPosition = SetInitialPosition(Dimension);
    }

    private void updateRotate()
    {
        if (playerDirection == Direction.Right || playerDirection == Direction.Left)
        {
            rotate = true;
        }
    }

    public bool hasFinished()
{
        return Finished;
    }

    public void FinishProjectile()
    {
        timer = HITBOX_TIMER;
    }

    public Rectangle getCollisionRectangle()
    {
        Point spawnPosition = projectilePosition.ToPoint();
        int width = rotate ? 64 : 32;
        int height = rotate ? 32 : 64;

        return new Rectangle(spawnPosition.X - width / 2, spawnPosition.Y - height / 2, width, height);
    }

    public void UpdateProjectile()
    {
        projectileDict.Enabled = false;
        if (timer < HITBOX_TIMER)
        {
            timer++;
            updateRotate();
        }
        else
        {
            Finished = true;
        }
    }
}
