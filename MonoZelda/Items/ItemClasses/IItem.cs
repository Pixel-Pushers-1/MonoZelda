using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;

namespace MonoZelda.Items.ItemClasses;

public interface IItem
{
    public bool ItemPickedUp { get; set; }
    void itemSpawn(SpriteDict itemDict, Point  spawnPosition, CollisionController collisionController);
}

