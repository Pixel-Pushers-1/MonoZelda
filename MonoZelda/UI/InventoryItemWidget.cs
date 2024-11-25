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
    private int rowItemCount;
    private int rows;
    private const int SLOT_OFFSET_RIGHT = 92;
    private const int SLOT_OFFSET_DOWN = 72;
    private SpriteDict itemSelector;
    private SpriteDict selectedItem;
    private EquippableType currentItem;
    private Point SELECTED_POINT_OFFSET = new Point(-260, 0);
    private Point AvailableInventorySlot;
    private Point SelectedItemSlot;
    private Point position;
    private readonly Dictionary<EquippableType, SpriteDict> availableItemSprites;
    private readonly Dictionary<EquippableType, Point> itemInventoryOffsetMap;

    private readonly Dictionary<EquippableType, string> equippableSpriteMap = new()
    {
         { EquippableType.Bow, "bow" },
         { EquippableType.Boomerang, "boomerang" },
         { EquippableType.CandleBlue, "candle_blue" },
         { EquippableType.Bomb, "bomb" },
         { EquippableType.BluePotion, "potion_blue" },
         { EquippableType.RedPotion, "potion_red" }
    };

    private readonly Dictionary<EquippableType, Point> equippableOffsetMap = new()
    {
        { EquippableType.Bow, new Point(16,0) },
        { EquippableType.Boomerang, new Point(16,0) },
        { EquippableType.CandleBlue, new Point(16,0) },
        { EquippableType.Bomb, new Point(16, 0) },
        { EquippableType.BluePotion, new Point(16, 0) },
        { EquippableType.RedPotion, new Point(16, 0) },
    };

    public InventoryItemWidget(Screen screen, Point position) : base(screen,position)
    {
        // initialize formatting variables
        rowItemCount = 0;
        rows = 0;
        currentItem = PlayerState.EquippedItem;
        availableItemSprites = new();
        itemInventoryOffsetMap = new();

        // initialize selectedItemSlot and inventorySlot to first slot
        SelectedItemSlot = WidgetLocation + SELECTED_POINT_OFFSET;

        // initialize item Selector
        selectedItem = new SpriteDict(SpriteType.HUD, SpriteLayer.HUD + 1, SelectedItemSlot);
        itemSelector = new SpriteDict(SpriteType.HUD, SpriteLayer.HUD + 1, position);
        itemSelector.SetSprite("outline_red");
    }

    private void UpdateAvailableItems()
    {
        foreach(var equippable in PlayerState.EquippableInventory)
        {
            if((equippable != EquippableType.None) && (availableItemSprites.ContainsKey(equippable) == false))
            {
                // update numItems and availableInventorySlot
                Point offset = GetEquippableOffset(equippable);
                rowItemCount = rowItemCount + 1;

                // add sprite of newItem to list of sprites
                SpriteDict newItem = new SpriteDict(SpriteType.HUD, SpriteLayer.HUD + 1, WidgetLocation + offset);
                newItem.SetSprite(equippableSpriteMap[equippable]);
                availableItemSprites.Add(equippable, newItem);
                itemInventoryOffsetMap.Add(equippable, offset);
            }
        }
    }

    private Point GetEquippableOffset(EquippableType equippable)
    {
        if ((rowItemCount != 0) && (rowItemCount % 4 == 0))
        {
            rows = rows + 1;
            rowItemCount = 0;
        }

        Point offset = new Point(rowItemCount * SLOT_OFFSET_RIGHT, SLOT_OFFSET_DOWN * rows);
        return offset;
    }

    private void UpdateSelectorPosition()
    {
        if (PlayerState.EquippedItem != EquippableType.None)
        {
            selectedItem.Enabled = true;
            itemSelector.SetSprite("outline_red");
            itemSelector.Position = WidgetLocation + itemInventoryOffsetMap[PlayerState.EquippedItem];
            selectedItem.SetSprite(equippableSpriteMap[PlayerState.EquippedItem]);
            selectedItem.Position = WidgetLocation + equippableOffsetMap[PlayerState.EquippedItem] + SELECTED_POINT_OFFSET;
        }
        else
        {
            selectedItem.Enabled = false;
            itemSelector.Position = WidgetLocation + itemInventoryOffsetMap[PlayerState.EquippedItem];
            itemSelector.SetSprite("outline_blue");
        }
        
    }

    private void UpdateItemSpritePosition()
    {
        foreach (KeyValuePair<EquippableType, SpriteDict> equippable in availableItemSprites)
        {
            equippable.Value.Position = WidgetLocation + itemInventoryOffsetMap[equippable.Key] + equippableOffsetMap[equippable.Key];
        }
    }

    public override void Update()
    {
        UpdateAvailableItems();
        UpdateItemSpritePosition();
        UpdateSelectorPosition();
    }

    public override void Draw(SpriteBatch sb)
    {
        //empty
    }

    public override void Load(ContentManager content)
    {
        //empty
    }
}
