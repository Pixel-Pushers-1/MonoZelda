using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Link;
using System.Diagnostics;

namespace MonoZelda.UI.NavigableMenus;

public class NavigableGrid
{
    private NavigableGridItem[,] grid;
    private Point selectedItem;

    public NavigableGrid(NavigableGridItem[,] grid) {
        this.grid = grid;
    }

    public void ExecuteSelection()
    {
        grid[selectedItem.X, selectedItem.Y].Execute();
    }

    public void MoveSelection(Point movement)
    {
        //set current selection and clamp to stay inside of grid
        selectedItem += movement;
        selectedItem.X = MathHelper.Clamp(selectedItem.X, 0, grid.GetLength(0) - 1);
        selectedItem.Y = MathHelper.Clamp(selectedItem.Y, 0, grid.GetLength(1) - 1);
    }
}

