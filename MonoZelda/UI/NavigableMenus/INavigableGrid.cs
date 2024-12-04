using Microsoft.Xna.Framework;

namespace MonoZelda.UI.NavigableMenus;

public interface INavigableGrid
{
    public void MoveSelection(Point movement);
    public void ExecuteSelection();
}

