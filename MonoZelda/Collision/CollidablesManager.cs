using System.Collections.Generic;
using PixelPushers.MonoZelda.Collision.Collidables;

namespace PixelPushers.MonoZelda.Collision;
public class CollidablesManager
{
    private readonly List<Collidable> collidables;

    public CollidablesManager()
    {
        collidables = new List<Collidable>();
    }

    // Add a new hitbox
    public void AddCollidableObject(Collidable collidable)
    {
        collidables.Add(collidable);
    }

    // Remove a hitbox
    public void RemoveCollidableObject(Collidable collidable)
    {
        collidables.Remove(collidable);
    }

    // Get all hitboxes
    public List<Collidable> GetListOfCollidableObjects()
    {
        return collidables;
    }
}

