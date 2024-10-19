using System;
using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace MonoZelda.Enemies.EnemyProjectiles
{
    public class AquamentusFireball
    {
        private Point pos;
        public SpriteDict FireballSpriteDict { get; private set; }

        private int speed = 4;
        private double angle;

        public AquamentusFireball(Point pos, ContentManager contentManager, int newAngle)
        {
            this.pos = pos;
            FireballSpriteDict = new(contentManager.Load<Texture2D>("Sprites/enemies"), SpriteCSVData.Enemies, 0, new Point(100, 100));
            FireballSpriteDict.SetSprite("fireball");
            angle = newAngle;
            if (angle <= 180)
            {
                if (angle <= 90)
                {
                    angle = angle / 180 - 2;
                }
                else
                {
                    angle = angle / 180 - 1;
                }
            }
            else
            {
                angle /= 180;
            }
        }

        public void Follow(Point newPos)
        {
            pos.X = newPos.X;
            pos.Y = newPos.Y - 48;
        }

        public void Update()
        {
            pos.X -= speed;
            if (angle >= 1.5)
            {
                angle = Math.Ceiling(angle);
            }
            else
            {
                angle = Math.Floor(angle);
            }

            pos.Y -= (int)angle;

            FireballSpriteDict.Position = pos;
        }
    }
}
