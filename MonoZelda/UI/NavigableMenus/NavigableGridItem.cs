using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Commands;
using MonoZelda.Scenes;
using MonoZelda.Sprites;
using System.Diagnostics;

namespace MonoZelda.UI.NavigableMenus;

public class NavigableGridItem
{
    public Point OutlineSize { get; set; }

    private CommandType commandType;
    private object[] metadata;
    private CommandManager commandManager;
    private IDrawable drawable;

    public NavigableGridItem(IDrawable drawable, CommandManager commandManager, CommandType commandType, params object[] metadata) {
        this.drawable = drawable;
        drawable.Enabled = false;
        this.commandType = commandType;
        this.commandManager = commandManager;
        this.metadata = metadata;
    }

    public NavigableGridItem(CommandManager commandManager, CommandType commandType, params object[] metadata) {
        this.commandType = commandType;
        this.commandManager = commandManager;
        this.metadata = metadata;
    }

    public void Execute() {
        commandManager.Execute(commandType, metadata);
    }

    public void Select() {
        if (drawable != null) {
            drawable.Enabled = true;
        }
    }

    public void Deselect() {
        if (drawable != null) {
            drawable.Enabled = false;
        }
    }
}

