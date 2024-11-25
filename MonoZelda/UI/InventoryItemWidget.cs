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
    private int numItems;
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
        numItems = 0;
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
            if(availableItemSprites.ContainsKey(equippable) == false)
            {
                // update numItems and availableInventorySlot
                Point offset = GetEquippableOffset(equippable);
                numItems++;

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
        Point offset;

        if ((numItems != 0) && (numItems % 4 == 0))
        {
            offset = new Point(0, SLOT_OFFSET_DOWN);
        }
        else
        {
            offset = new Point(SLOT_OFFSET_RIGHT * numItems,0);
        }

        return offset;    
    }

    private void UpdateSelectorPosition()
    {
        if (PlayerState.EquippedItem != EquippableType.None)
        {
            itemSelector.Enabled = true;
            selectedItem.Enabled = true;
            itemSelector.Position = WidgetLocation + itemInventoryOffsetMap[PlayerState.EquippedItem];
            selectedItem.SetSprite(equippableSpriteMap[PlayerState.EquippedItem]);
            selectedItem.Position = WidgetLocation + equippableOffsetMap[PlayerState.EquippedItem] + SELECTED_POINT_OFFSET;
        }
        else
        {
            itemSelector.Enabled = false;
            selectedItem.Enabled = false;
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
