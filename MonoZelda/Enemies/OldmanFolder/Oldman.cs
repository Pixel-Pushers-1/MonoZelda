using Microsoft.Xna.Framework;
using MonoZelda.Sprites;

namespace MonoZelda.Enemies.OldmanFolder
{
    public class Oldman : IEnemy
    {
        private Point pos;
        private SpriteDict oldmanSpriteDict;
        private readonly int spawnX;
        private readonly int spawnY;
        private bool spawning;
        private double startTime;

        public Oldman(SpriteDict spriteDict, GraphicsDeviceManager graphics)
        {
            oldmanSpriteDict = spriteDict;
            spawnX = 3 * graphics.PreferredBackBufferWidth / 5;
            spawnY = 3 * graphics.PreferredBackBufferHeight / 5;
            pos = new(spawnX, spawnY);

        }
        public void SetOgPos(GameTime gameTime)
        {
            pos.X = spawnX;
            pos.Y = spawnY;
            oldmanSpriteDict.Position = pos;
            oldmanSpriteDict.SetSprite("cloud");
            spawning = true;
            startTime = gameTime.TotalGameTime.TotalSeconds;
        }

        public void ChangeDirection()
        {
        }

        public void Update(GameTime gameTime)
        {
            if (spawning)
            {
                if (gameTime.TotalGameTime.TotalSeconds >= startTime + 0.3)
                {
                    spawning = false;
                    oldmanSpriteDict.SetSprite("oldman");
                }
            }
        }

        public void DisableProjectile()
        {
        }
    }
}
