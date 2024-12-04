using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Commands;
using MonoZelda.Scenes;
using MonoZelda.Sprites;
using System.Diagnostics;

namespace MonoZelda.UI.NavigableMenus;

public class UIGridItem
{
    public bool FireOnExecuted { get; set; }
    public bool FireOnSelected { get; set; }
    public bool FireOnDeselected { get; set; }

    private CommandType commandType;
    private object[] metadata;
    private CommandManager commandManager;
    private SpriteDict spriteDict;
    private Point spriteDictStartingPosition;

    public UIGridItem(SpriteDict spriteDict, CommandManager commandManager, CommandType commandType, params object[] metadata) {
        this.spriteDict = spriteDict;
        spriteDict.Enabled = false;
        spriteDictStartingPosition = spriteDict.Position;
        this.commandType = commandType;
        this.commandManager = commandManager;
        this.metadata = metadata;
        FireOnExecuted = true;
    }

    public UIGridItem(CommandManager commandManager, CommandType commandType, params object[] metadata) {
        this.commandType = commandType;
        this.commandManager = commandManager;
        this.metadata = metadata;
    }

    public void Execute() {
        if (FireOnExecuted) {
            commandManager.Execute(commandType, metadata);
        }
    }

    public void Select() {
        if (spriteDict != null) {
            spriteDict.Enabled = true;
        }
        if (FireOnSelected) {
            commandManager.Execute(commandType, metadata);
        }
    }

    public void Deselect() {
        if (spriteDict != null) {
            spriteDict.Enabled = false;
        }
        if (FireOnDeselected) {
            commandManager.Execute(commandType, metadata);
        }
    }

    public void UpdateSpriteDictOffset(Point offset) {
        spriteDict.Position = spriteDictStartingPosition + offset;
    }
}

