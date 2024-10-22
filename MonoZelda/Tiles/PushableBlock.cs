namespace MonoZelda.Tiles;

internal class PushableBlock : TileBase, IPushable
{
    public PushableBlock()
    {
        IsPassable = false;
    }

    public void PushEast()
    {
        throw new System.NotImplementedException();
    }

    public void PushNorth()
    {
        throw new System.NotImplementedException();
    }

    public void PushSouth()
    {
        throw new System.NotImplementedException();
    }

    public void PushWest()
    {
        throw new System.NotImplementedException();
    }
}
