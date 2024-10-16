using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoZelda.Collision;
using MonoZelda.Commands;

namespace MonoZelda.Controllers;

public class CollisionController : IController
{

    private List<Collidable> _gameObjects;
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

        _gameObjects = new List<Collidable>();
    }

    public void Update(GameTime gameTime)
    {
        for (int i = 0; i < _gameObjects.Count; i++)
        {
            for (int j = i + 1; j < _gameObjects.Count; j++)
            {
                // Debug Statement
                Collidable collidableA = _gameObjects[i];
                Collidable collidableB = _gameObjects[j];

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

    public void AddCollidable(Collidable collidable)
    {
        _gameObjects.Add(collidable);
    }

    public void RemoveCollidable(Collidable collidable)
    {
        _gameObjects.Remove(collidable);
    }

    public void Reset()
    {
        _gameObjects = new List<Collidable>();
    }

    // Check if two objects are colliding (AABB collision detection)
    private bool IsColliding(Collidable collidableA, Collidable collidableB)
    {
        // Implement collision check logic (From Collision detection and bounding boxes)
        return collidableA.Intersects(collidableB);
    }

    // Handle what happens when two objects collide
    private void HandleCollision(Collidable collidableA, Collidable collidableB, params object[] metadata)
    {
        if (_collisionCommandDictionary.ContainsKey((collidableA.type, collidableB.type)))
        {
            _commandManager.Execute(_collisionCommandDictionary[(collidableA.type, collidableB.type)], metadata);
        }
        else if (_collisionCommandDictionary.ContainsKey((collidableB.type, collidableA.type)))
        {
            _commandManager.Execute(_collisionCommandDictionary[(collidableB.type, collidableA.type)], metadata);
        }
    }

    private object[] GetMetadata(Collidable collidableA, Collidable collidableB)
    {
        // We need to implement this method to give back information about the collision
        return null;
    }

    internal void Clear()
    {
        _gameObjects.Clear();
    }
}

