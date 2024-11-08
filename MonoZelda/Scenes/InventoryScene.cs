using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Commands;
using MonoZelda.Dungeons;
using MonoZelda.Link;
using MonoZelda.UI;
using System;
using System.Collections.Generic;

namespace MonoZelda.Scenes
{
    internal class InventoryScene : IScene
    {
        public Screen Screen { get; set; }
        public Dictionary<Type,IScreenWidget> Widgets { get; set; }
        private SpriteFont _spriteFont;
        private PlayerState _playerState;
        private GraphicsDevice graphicsDevice;

        public InventoryScene(GraphicsDevice gd, CommandManager commands, PlayerState state)
        {
            // The Inventory starts mostly off screen
            Screen = new Screen() { Origin = new Point(0, 0) }; // Screen is helpfull for moving all the widgets at once
            Widgets = new Dictionary<Type, IScreenWidget>();
            _playerState = state;

            // TODO: I'm sure there will be needs for widgets to register commands.
            graphicsDevice = gd;
        }

        public InventoryScene()
        {
        }

        public void LoadContent(ContentManager contentManager)
        {
            _spriteFont ??= contentManager.Load<SpriteFont>("Fonts/Basic");
            

            Widgets.Add(typeof(LevelTextWidget), new LevelTextWidget(_spriteFont, Screen, Point.Zero));
            Widgets.Add(typeof(LifeWidget), new LifeWidget(_spriteFont, Screen, new Point(800, 0), contentManager, _playerState));
            Widgets.Add(typeof(ItemCountWidget), new ItemCountWidget(_spriteFont, Screen, new Point(600, 0), contentManager, _playerState));
        }

        public void LoadContent(ContentManager cm, IDungeonRoom room)
        {
            Widgets.Clear();

            LoadContent(cm);
        }

        public void Update(GameTime gameTime)
        {
            foreach(var widget in Widgets.Values)
            {
                widget.Update();
            }
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (var widget in Widgets.Values)
            {
                widget.Draw(sb);
            }
        }
    }
}
