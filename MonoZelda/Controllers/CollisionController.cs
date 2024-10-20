using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoZelda.Collision.Collidables;
using MonoZelda.Commands;

namespace MonoZelda.Controllers;

public class CollisionController : IController
{

    private List<ICollidable> _gameObjects;
    private CommandManager _commandManager;

    private Dictionary<(CollidableType, CollidableType), CommandType>_collisionCommandDictionary;

    public CollisionController(CommandManager commandManager)
    {
        _commandManager = commandManager;

        _collisionCommandDictionary = new Dictionary<(CollidableType, CollidableType), CommandType>
        {
            {(CollidableType.Player, CollidableType.Item), CommandType.PlayerItemCollisionCommand},
            {(CollidableType.Player, CollidableType.Enemy), CommandType.PlayerEnemyCollisionCommand},
            {(CollidableType.Player, CollidableType.Projectile), CommandType.PlayerProjectileCollisionCommand},
            {(CollidableType.Player, CollidableType.Static), CommandType.PlayerStaticCollisionCommand},
            {(CollidableType.Enemy, CollidableType.Projectile), CommandType.EnemyProjectileCollisionCommand},
            {(CollidableType.Enemy, CollidableType.Static), CommandType.EnemyStaticCollisionCommand},
        };

        _gameObjects = new List<ICollidable>();
    }

    public void Update(GameTime gameTime)
    {
        for (int i = 0; i < _gameObjects.Count; i++)
        {
            for (int j = i + 1; j < _gameObjects.Count; j++)
            {
                // Debug Statement
                ICollidable collidableA = _gameObjects[i];
                ICollidable collidableB = _gameObjects[j];

                // Check for a collision between objA and objB
                if (IsColliding(collidableA, collidableB))
                {
                    // Grab the metadata we need to know about the collision
                    object[] metadata = GetMetadata(collidableA, collidableB);

                    // Handle the collision response for both objects
                    HandleCollision(collidableA, collidableB, metadata);
                }
            }
        }
    }

    public void AddCollidable(ICollidable collidable)
    {
        _gameObjects.Add(collidable);
    }

    public void RemoveCollidable(ICollidable collidable)
    {
        _gameObjects.Remove(collidable);
    }

    public void Reset()
    {
        _gameObjects = new List<ICollidable>();
    }

    // Check if two objects are colliding (AABB collision detection)
    private bool IsColliding(ICollidable collidableA, ICollidable collidableB)
    {
        // Implement collision check logic (From Collision detection and bounding boxes)
        return collidableA.Intersects(collidableB);
    }

    // Handle what happens when two objects collide
    private void HandleCollision(ICollidable collidableA, ICollidable collidableB, params object[] metadata)
    {
        System.Diagnostics.Debug.WriteLine("Handling Collision!!");
        if (_collisionCommandDictionary.ContainsKey((collidableA.type, collidableB.type)))
        {
            _commandManager.Execute(_collisionCommandDictionary[(collidableA.type, collidableB.type)], metadata);
        }
        else if (_collisionCommandDictionary.ContainsKey((collidableB.type, collidableA.type)))
        {
            _commandManager.Execute(_collisionCommandDictionary[(collidableB.type, collidableA.type)], metadata);
        }
    }

    private object[] GetMetadata(ICollidable collidableA, ICollidable collidableB)
    {
        // We need to implement this method to give back information about the collision
        var metadata = new object[]
        {
            collidableA,
            collidableB,
            this,
        };

        return metadata;
    }

    internal void Clear()
    {
        _gameObjects.Clear();
    }
}

