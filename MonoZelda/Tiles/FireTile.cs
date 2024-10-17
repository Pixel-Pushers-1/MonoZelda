using MonoZelda.Sprites;
using System;

namespace MonoZelda.Tiles;

internal class FireTile : TileBase, IInteractiveTile
{
    public FireTile(SpriteDict spriteDict) : base(spriteDict)
    {
        IsPassable = true;
        spriteDict.SetSprite("fire");
    }

    public void Interact(object player)
    {
        throw new NotImplementedException();
    }
}
