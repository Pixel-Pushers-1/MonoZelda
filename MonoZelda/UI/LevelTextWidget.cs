using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoZelda.UI
{
    internal class LevelTextWidget : ScreenWidget
    {
        public static string LevelName = "Level 1";
        
        private SpriteFont font;
        private Point margin = new Point(400, 10);

        public LevelTextWidget(SpriteFont spriteFont, Screen screen, Point position) : base(screen, position)
        {
            font = spriteFont;
        }

        public override void Draw(SpriteBatch sb)
        {
            if(string.IsNullOrEmpty(LevelName)) return;
            
            // Are text origins in the upper left?
            sb.DrawString(font, LevelName, (WidgetLocation + margin).ToVector2(), Color.White);
        }

        public override void Load(ContentManager content)
        {
        }

        public override void Update()
        {
        }
    }
}
