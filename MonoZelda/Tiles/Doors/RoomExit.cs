using System;

namespace MonoZelda.Tiles;

internal class RoomExit : TileBase, IInteractiveTile
{
    private TileFacing _facing;
    private bool _isOpen;

    public RoomExit() : base()
    {
        _isOpen = true;
    }

    public RoomExit(TileFacing facing) : base()
    {
        _facing = facing;
    }

    public void Interact(object player)
    {
        throw new NotImplementedException();
    }
}

