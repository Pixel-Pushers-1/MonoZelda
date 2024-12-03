using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Link;
using MonoZelda.Link.Equippables;
using MonoZelda.Sprites;
using System.Collections.Generic;

namespace MonoZelda.UI;

internal class InventoryItemWidget : ScreenWidget
{
    private static readonly Point gridItemSpacing = new Point(64, 64);
    private static readonly Point gridOffset = new Point(16, 0);

    private SpriteDict[,] itemSpriteDicts;
    private SpriteDict selectionSpriteDict;

    private readonly Dictionary<EquippableType, string> equippableSpriteMap = new()
    {
         { EquippableType.Bow, "bow" },
         { EquippableType.Boomerang, "boomerang" },
         { EquippableType.CandleBlue, "candle_blue" },
         { EquippableType.Bomb, "bomb" },
         { EquippableType.BluePotion, "potion_blue" },
         { EquippableType.RedPotion, "potion_red" }
    };

    public InventoryItemWidget(Screen screen, Point position) : base(screen, position)
    {
        //set up spritedict grid
        EquippableType[,] grid = PlayerState.EquippableManager.GetGrid();
        itemSpriteDicts = new SpriteDict[grid.GetLength(0), grid.GetLength(1)];
        for (int y = 0; y < grid.GetLength(1); y++) {
            for (int x = 0; x < grid.GetLength(0); x++) {
                itemSpriteDicts[x, y] = new SpriteDict(SpriteType.HUD, SpriteLayer.HUD + 1, WidgetLocation + gridOffset + new Point(x, y) * gridItemSpacing);
            }
        }
        UpdateEquippablesGrid();

        selectionSpriteDict = new SpriteDict(SpriteType.HUD, SpriteLayer.HUD + 2, WidgetLocation);
        selectionSpriteDict.SetSprite("outline_red");
    }

    public override void Update()
    {
        UpdateEquippablesGrid();
        UpdateSelection();
    }

    public override void Draw(SpriteBatch sb)
    {
        //empty
    }

    public override void Load(ContentManager content)
    {
        //empty
    }

    private void UpdateEquippablesGrid() {
        EquippableType[,] grid = PlayerState.EquippableManager.GetGrid();
        for (int y = 0; y < grid.GetLength(1); y++) {
            for (int x = 0; x < grid.GetLength(0); x++) {
                if (grid[x, y] == EquippableType.None) {
                    itemSpriteDicts[x, y].Enabled = false;
                }
                else {
                    itemSpriteDicts[x, y].SetSprite(equippableSpriteMap[grid[x, y]]);
                    itemSpriteDicts[x, y].Enabled = true;
                }
                itemSpriteDicts[x, y].Position = WidgetLocation + gridOffset + new Point(x, y) * gridItemSpacing;
            }
        }
    }

    private void UpdateSelection()
    {
        selectionSpriteDict.Position = WidgetLocation + PlayerState.EquippableManager.GetSelection() * gridItemSpacing;
    }
}
