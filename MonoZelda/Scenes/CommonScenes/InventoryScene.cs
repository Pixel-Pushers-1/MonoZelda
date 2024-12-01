using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Commands;
using MonoZelda.Dungeons;
using MonoZelda.UI;
using System;
using System.Collections.Generic;

namespace MonoZelda.Scenes
{
    internal class InventoryScene : Scene
    {
        private static readonly Point HUDBackgroundPosition = new (0, -32);
        private static readonly Point HUDMapPosition = new (64, 80);
        private static readonly Point LifePosition = new (720, 128);
        private static readonly Point ItemCountPosition = new (416, 64);
        private static readonly Point LevelTextPosition = new(10, 10);
        private static readonly Point InventoryMapPosition = new(528, -288);
        private static readonly Point InventoryPosition = new(516, -514);
        private static readonly Point XpBarPosition = new(712, 44);

        private const int INVENTORY_OPEN_Y = 704;
        private const int INVENTORY_OPEN_SPEED = 16;

        public Screen Screen { get; set; }
        public Dictionary<Type,IScreenWidget> Widgets { get; set; }
        private SpriteFont _spriteFont;
        private GraphicsDevice graphicsDevice;
        private bool isInventoryOpen = false;

        public InventoryScene(GraphicsDevice gd, CommandManager commands)
        {
            // The Inventory starts mostly off-screen
            Screen = new Screen() { Origin = new Point(0, 0) }; // Screen is helpfull for moving all the widgets at once
            Widgets = new Dictionary<Type, IScreenWidget>();
            graphicsDevice = gd;
        }

        public InventoryScene()
        {
        }

        public void LoadContent(ContentManager contentManager)
        {
            _spriteFont ??= contentManager.Load<SpriteFont>("Fonts/Basic");

            Widgets.Add(typeof(HUDBackgroundWidget), new HUDBackgroundWidget(Screen, HUDBackgroundPosition));
            Widgets.Add(typeof(HUDMapWidget), new HUDMapWidget(Screen, HUDMapPosition));
            Widgets.Add(typeof(LifeWidget), new LifeWidget(Screen, LifePosition));
            Widgets.Add(typeof(ItemCountWidget), new ItemCountWidget(_spriteFont, Screen, ItemCountPosition));
            Widgets.Add(typeof(LevelTextWidget), new LevelTextWidget(_spriteFont, Screen, LevelTextPosition));
            Widgets.Add(typeof(InventoryMapWidget), new InventoryMapWidget(Screen, InventoryMapPosition));
            Widgets.Add(typeof(InventoryItemWidget), new InventoryItemWidget(Screen, InventoryPosition));
            Widgets.Add(typeof(XpBarWidget), new XpBarWidget(Screen, XpBarPosition));
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
            
            if (isInventoryOpen && Screen.Origin.Y <= INVENTORY_OPEN_Y)
            {
                Screen.Origin = new Point(0, Math.Min(Screen.Origin.Y + INVENTORY_OPEN_SPEED, INVENTORY_OPEN_Y));
            }
            else if (Screen.Origin.Y > 0)
            {
                Screen.Origin = new Point(0, Math.Max(Screen.Origin.Y - INVENTORY_OPEN_SPEED, 0));
            }
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (var widget in Widgets.Values)
            {
                widget.Draw(sb);
            }
        }

        public void UnloadContent()
        {
        }

        public bool ToggleInventory()
        {
            isInventoryOpen = !isInventoryOpen;
            return isInventoryOpen;
        }

        public void SetPlayerMapMarker(Point? coord) {
            ((HUDMapWidget) Widgets[typeof(HUDMapWidget)]).SetPlayerMapMarker(coord);
            ((InventoryMapWidget) Widgets[typeof(InventoryMapWidget)]).SetPlayerMapMarker(coord);
        }
    }
}
