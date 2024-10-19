using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Sprites;

namespace MonoZelda.Enemies.GoriyaFolder
{
    public class GoriyaBoomerang
    {
        private Point pos;
        public SpriteDict BoomerangSpriteDict { get; private set; }
        private int speed = 4;

        public GoriyaBoomerang(Point pos, ContentManager contentManager)
        {
            this.pos = pos;
            BoomerangSpriteDict = new(contentManager.Load<Texture2D>("Sprites/enemies"), SpriteCSVData.Enemies, 0, new Point(100, 100));
            BoomerangSpriteDict.SetSprite("boomerang");
        }

        public void Follow(Point newPos)
        {
            pos = newPos;
        }

        public void Update(GameTime gameTime, CardinalEnemyStateMachine.Direction attackDirection, double attackTime)
        {
            if (gameTime.TotalGameTime.TotalSeconds <= attackTime + 4)
            {
                switch (attackDirection)
                {
                    case CardinalEnemyStateMachine.Direction.Left:
                        pos.X -= speed;
                        break;
                    case CardinalEnemyStateMachine.Direction.Right:
                        pos.X += speed;
                        break;
                    case CardinalEnemyStateMachine.Direction.Up:
                        pos.Y -= speed;
                        break;
                    case CardinalEnemyStateMachine.Direction.Down:
                        pos.Y += speed;
                        break;
                }
            }
            else if (gameTime.TotalGameTime.TotalSeconds <= attackTime + 5)
            {
                switch (attackDirection)
                {
                    case CardinalEnemyStateMachine.Direction.Left:
                        pos.X += speed;
                        break;
                    case CardinalEnemyStateMachine.Direction.Right:
                        pos.X -= speed;
                        break;
                    case CardinalEnemyStateMachine.Direction.Up:
                        pos.Y += speed;
                        break;
                    case CardinalEnemyStateMachine.Direction.Down:
                        pos.Y -= speed;
                        break;
                }
            }
            BoomerangSpriteDict.Position = pos;
        }
    }
}