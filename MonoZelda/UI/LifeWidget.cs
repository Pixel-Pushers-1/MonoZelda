using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Link;
using MonoZelda.Sprites;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            var numberOfHearts = playerState.MaxHealth;
            for (int i = 0; i < numberOfHearts; i++)
            {
                Point heartPosition = WidgetLocation + margin + new Point((i * 32), 32);
                SpriteDict heart = new SpriteDict(SpriteType.Items, 0, heartPosition);
                heart.SetSprite("heart_full");

                _hearts.Add(heart);
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.DrawString(font, "-LIFE-", (WidgetLocation + margin).ToVector2(), Color.DarkRed);

            // TODO: draw n hearts based on links HP
        }

        public override void Load(ContentManager content)
        {

        }

        public override void Update()
        {
            for(int i =0 ; i < _hearts.Count; i++) {
                _hearts[i].Enabled = i < playerState.Health;
            }
        }
    }
}
