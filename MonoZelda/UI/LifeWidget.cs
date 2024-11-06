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
        private SpriteFont font;
        private Point margin = new Point(10, 10);
        private PlayerState playerState;

        private List<SpriteDict> _hearts = new();

        public LifeWidget(SpriteFont spriteFont, Screen screen, Point position, ContentManager cm, PlayerState playerState) : base(screen, position)
        {
            font = spriteFont;
            this.playerState = playerState;

            //create heart sprites
            int numberOfHearts = (int) ((playerState.MaxHealth + 1) / 2f);
            for (int i = 0; i < numberOfHearts; i++)
            {
                Point heartPosition = WidgetLocation + margin + new Point((i * 32), 32);
                SpriteDict heart = new SpriteDict(SpriteType.HUD, 0, heartPosition);
                _hearts.Add(heart);
            }
            SetHearts(playerState.Health);
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.DrawString(font, "-LIFE-", (WidgetLocation + margin).ToVector2(), Color.DarkRed);
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
