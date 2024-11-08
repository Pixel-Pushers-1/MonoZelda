using Microsoft.Xna.Framework;
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

        public LifeWidget(Screen screen, Point position, ContentManager cm, PlayerState playerState) : base(screen, position)
        {
            this.playerState = playerState;

            //create heart sprites
            int numberOfHearts = (int) ((playerState.MaxHealth + 1) / 2f);
            for (int i = 0; i < numberOfHearts; i++)
            {
                Point heartPosition = WidgetLocation + margin + new Point((i * 32), 32);
                SpriteDict heart = new SpriteDict(SpriteType.HUD, SpriteLayer.HUD, heartPosition);
                _hearts.Add(heart);
            }
            SetHearts(playerState.Health);
        }

        public override void Draw(SpriteBatch sb)
        {
        }

        public override void Load(ContentManager content)
        {

        }

        public override void Update()
        {
            SetHearts(playerState.Health);
        }

        private void SetHearts(int health)
        {
            for (int i = 0; i < _hearts.Count; i++) {
                if (health >= 2)
                    _hearts[i].SetSprite("heart_full");
                else if (health == 1)
                    _hearts[i].SetSprite("heart_half");
                else
                    _hearts[i].SetSprite("heart_empty");
                health -= 2;
            }
        }
    }
}
