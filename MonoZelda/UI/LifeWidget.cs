﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Link;
using MonoZelda.Sprites;
using System.Collections.Generic;
using System.Diagnostics;

namespace MonoZelda.UI
{
    internal class LifeWidget : ScreenWidget
    {
        private Point margin = new Point(10, 10);

        private List<SpriteDict> _hearts = new();

        public LifeWidget(Screen screen, Point position) : base(screen, position)
        {

            //create heart sprites
            int numberOfHearts = (int) ((PlayerState.MaxHealth + 1) / 2f);
            for (int i = 0; i < numberOfHearts; i++)
            {
                Point heartPosition = WidgetLocation + margin + new Point(i * 32, 0);
                SpriteDict heart = new SpriteDict(SpriteType.HUD, SpriteLayer.HUD, heartPosition);
                _hearts.Add(heart);
            }
            SetHearts((int)PlayerState.Health);
        }

        public override void Draw(SpriteBatch sb)
        {
        }

        public override void Load(ContentManager content)
        {

        }

        public override void Update()
        {
            SetHearts((int)PlayerState.Health);
        }

        private void SetHearts(int health)
        {
            //add new hearts
            while (PlayerState.MaxHealth / 2 > _hearts.Count) {
                SpriteDict heart = new SpriteDict(SpriteType.HUD, SpriteLayer.HUD, Point.Zero);
                _hearts.Add(heart);
                heart.Position = WidgetLocation + margin + new Point(_hearts.IndexOf(heart) * 32, 0);
            }

            for (int i = 0; i < _hearts.Count; i++) {
                if (health >= 2)
                    _hearts[i].SetSprite("heart_full");
                else if (health == 1)
                    _hearts[i].SetSprite("heart_half");
                else
                    _hearts[i].SetSprite("heart_empty");
                health -= 2;

                Point heartPosition = WidgetLocation + margin + new Point(i * 32, 0); 
                _hearts[i].Position = heartPosition; 

            }
           

        }
    }
}
