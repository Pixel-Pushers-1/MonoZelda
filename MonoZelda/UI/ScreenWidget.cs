using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoZelda.UI
{
    internal abstract class ScreenWidget : IScreenWidget
    {
        public Screen Screen { get; private set; }
        public Point WidgetLocation => Screen.Origin + position;

        private Point position;

        public ScreenWidget(Screen screen, Point position)
        {
            this.Screen = screen;
            this.position = position;
        }

        public abstract void Draw(SpriteBatch sb);
        public abstract void Load(ContentManager content);
        public abstract void Update();
    }
}
