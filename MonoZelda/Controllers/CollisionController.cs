using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoZelda.Collision;
using MonoZelda.Commands;
using MonoZelda.Link;

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
        //convention: collidableA, collidableB, CollisionController, direction, intersection area
        var metadata = new object[5];
        metadata[0] = collidableA;
        metadata[1] = collidableB;
        metadata[2] = this;

        //calculate intersection direction
        Direction direction;
        Rectangle intersect = collidableA.GetIntersectionArea(collidableB);
        Point aCenter = collidableA.Bounds.Center;
        Point bCenter = collidableB.Bounds.Center;

        if (intersect.Width <= intersect.Height) {
            //left-right collision
            int dx = bCenter.X - aCenter.X;
            direction = dx < 0 ? Direction.Left : Direction.Right;
        }
        else {
            //up-down collision
            int dy = bCenter.Y - aCenter.Y;
            direction = dy < 0 ? Direction.Up : Direction.Down;
        }

        //set direction and intersection information
        metadata[3] = direction;
        metadata[4] = intersect;

        return metadata;
    }

    internal void Clear()
    {
        _gameObjects.Clear();
    }
}

