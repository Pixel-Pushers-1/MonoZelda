using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Sprites;

namespace MonoZelda.UI
{
    internal class HUDBackgroundWidget : ScreenWidget
    {
        private SpriteDict MiniMenu;
        private SpriteDict Map;
        private SpriteDict Inventory;

        private Point mapOffset = new Point(0,-320);
        private Point inventoryOffset = new Point(0,-672);

        public HUDBackgroundWidget(Screen screen, Point position) : base(screen, position)
        {
            // Create Mini-Menu
            MiniMenu = new(SpriteType.HUD, SpriteLayer.HUD - 1, position);
            string backgroundSpriteName = $"hud_background_{(MonoZeldaGame.GameMode == GameType.Classic ? "noxp" : "xp")}";
            MiniMenu.SetSprite(backgroundSpriteName);

            // Create Map and Inventory Menu
            Map = new(SpriteType.HUD, SpriteLayer.HUD - 1, position + mapOffset);
            Map.SetSprite("map_background");
            Inventory = new(SpriteType.HUD, SpriteLayer.HUD - 1, position + inventoryOffset);
            Inventory.SetSprite("inventory_background");
        }

        public override void Draw(SpriteBatch sb)
        {
        }

        public override void Load(ContentManager content)
        {
        }

        public override void Update()
        {
            MiniMenu.Position = WidgetLocation;
            Map.Position = WidgetLocation + mapOffset;
            Inventory.Position = WidgetLocation + inventoryOffset;
        }
    }
}
