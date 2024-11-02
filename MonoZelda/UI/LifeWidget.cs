using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
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

        public LifeWidget(SpriteFont spriteFont, Screen screen, Point position) : base(screen, position)
        {
            font = spriteFont;
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
        }
    }
}
