using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Commands;
using MonoZelda.Scenes;
using System.Diagnostics;

namespace MonoZelda.UI.NavigableMenus;

public class NavigableGridItem
{
    public Point OutlineSize { get; set; }

    private CommandType commandType;
    private object[] metadata;
    private CommandManager commandManager;

    public NavigableGridItem(CommandManager commandManager, CommandType commandType, params object[] metadata) {
        this.commandType = commandType;
        this.commandManager = commandManager;
        this.metadata = metadata;
    }

    public void Execute() {
        commandManager.Execute(commandType, metadata);
    }
}

