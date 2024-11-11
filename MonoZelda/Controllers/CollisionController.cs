using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoZelda.Collision;
using MonoZelda.Commands;
using MonoZelda.Link;
using MonoZelda.Tiles;

namespace MonoZelda.Controllers;

public class CollisionController : IController
{

    private List<ICollidable> _gameObjects;
    private Queue<ICollidable> _removeQueue;
    private Queue<ICollidable> _addQueue;
    private CommandManager _commandManager;

    private Dictionary<(CollidableType, CollidableType), CommandType>_collisionCommandDictionary;

    public CollisionController(CommandManager commandManager)
    {
        _commandManager = commandManager;

        _collisionCommandDictionary = new Dictionary<(CollidableType, CollidableType), CommandType>
        {
            {(CollidableType.Player, CollidableType.Item), CommandType.PlayerItemCollisionCommand},
            {(CollidableType.Player, CollidableType.Enemy), CommandType.PlayerEnemyCollisionCommand},
            {(CollidableType.Player, CollidableType.EnemyProjectile), CommandType.PlayerEnemyProjectileCollisionCommand},
            {(CollidableType.Player, CollidableType.StaticRoom), CommandType.PlayerStaticCollisionCommand},
            {(CollidableType.Player, CollidableType.StaticBoundary), CommandType.PlayerStaticCollisionCommand},
            {(CollidableType.Player, CollidableType.Trigger), CommandType.PlayerTriggerCollisionCommand},
            {(CollidableType.Enemy, CollidableType.PlayerProjectile), CommandType.EnemyPlayerProjectileCollisionCommand},
            {(CollidableType.Enemy, CollidableType.StaticRoom), CommandType.EnemyStaticRoomCollisionCommand},
            {(CollidableType.Enemy, CollidableType.StaticBoundary), CommandType.EnemyStaticBoundaryCollisionCommand},
            {(CollidableType.EnemyProjectile, CollidableType.StaticBoundary), CommandType.EnemyProjectileStaticBoundaryCollisionCommand},
            {(CollidableType.PlayerProjectile, CollidableType.StaticRoom), CommandType.PlayerProjectileStaticRoomCollisionCommand},
            {(CollidableType.PlayerProjectile, CollidableType.StaticBoundary), CommandType.PlayerProjectileStaticBoundaryCollisionCommand},
            {(CollidableType.PlayerProjectile, CollidableType.Door), CommandType.PlayerProjectileDoorCollisionCommand},
        };

        _gameObjects = new List<ICollidable>();
        _removeQueue = new Queue<ICollidable>();
        _addQueue = new Queue<ICollidable>();
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
                if(collidableA is BombableWall && collidableB is PlayerProjectileCollidable)
                {
                    int devbug = 1;
                }
                if(collidableA is BombableWall)
                {
                    int devbug = 1;
                }
                
                if (IsColliding(collidableA, collidableB))
                {
                    // Grab the metadata we need to know about the collision
                    object[] metadata = GetMetadata(collidableA, collidableB);

                    // Handle the collision response for both objects
                    HandleCollision(collidableA, collidableB, metadata);
                }
            }
        }
        
        // Remove any objects that need to be removed
        while (_removeQueue.Count > 0)
        {
            _gameObjects.Remove(_removeQueue.Dequeue());
        }
        
        // Add any objects that need to be added
        while (_addQueue.Count > 0)
        {
            _gameObjects.Add(_addQueue.Dequeue());
        }
    }

    public void AddCollidable(ICollidable collidable)
    {
        _addQueue.Enqueue(collidable);
    }

    public void RemoveCollidable(ICollidable collidable)
    {
        _removeQueue.Enqueue(collidable);
    }

    public void Reset()
    {
        _gameObjects = new List<ICollidable>();
    }

    // Check if two objects are colliding (AABB collision detection)
    public bool IsColliding(ICollidable collidableA, ICollidable collidableB)
    {
        // Implement collision check logic (From Collision detection and bounding boxes)
        return collidableA.Intersects(collidableB);
    }

    // Handle what happens when two objects collide
    private void HandleCollision(ICollidable collidableA, ICollidable collidableB, params object[] metadata)
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

    private object[] GetMetadata(ICollidable collidableA, ICollidable collidableB)
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

