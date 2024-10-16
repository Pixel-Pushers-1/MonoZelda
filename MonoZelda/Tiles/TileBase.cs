using Microsoft.Xna.Framework;
using MonoZelda.Sprites;

namespace MonoZelda.Tiles;

internal abstract class TileBase : ITile
{
    public Point Position { get; set; }
    public bool IsPassable { get; set; }

    private SpriteDict _spriteDict;

    public TileBase()
    {

    }

    public void SetSpriteDict(SpriteDict s)
    {
        _spriteDict = s;
    }

    public TileBase(SpriteDict tileSprite)
    {
        _spriteDict = tileSprite;
    }

    public void SetSprite(string name)
    {
        if(_spriteDict != null)
        {
        _spriteDict.SetSprite(name);
    }
}
}