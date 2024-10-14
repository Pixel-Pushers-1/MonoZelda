using System.Collections.Generic;
using PixelPushers.MonoZelda.Commands;
using Microsoft.Xna.Framework;
using System;
using MonoZelda.Collision;

namespace PixelPushers.MonoZelda.Controllers;

public class CollisionController : IController
{

    private List<Collidable> _gameObjects;
    private CommandManager _commandManager;

    public CollisionController(CommandManager commandManager)
    {
        _commandManager = commandManager;

        _gameObjects = new List<Collidable>();
    }

    public void Update(GameTime gameTime)
    {
        for (int i = 0; i < _gameObjects.Count; i++)
        {
            for (int j = i + 1; j < _gameObjects.Count; j++)
            {
                Collidable collidableA = _gameObjects[i];
                Collidable collidableB = _gameObjects[j];

                // Check for a collision between objA and objB
                if (IsColliding(collidableA, collidableB))
                {
                    // Handle the collision response for both objects
                    HandleCollision(collidableA, collidableB);
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
        if (collidableA.Intersects(collidableB)) {
            return true;
        }
        return false;
    }

    // Handle what happens when two objects collide
    private void HandleCollision(Collidable collidableA, Collidable collidableB)
    {
        // Example collision response: print a message
        Console.WriteLine($"{collidableA} collided with {collidableB}");

        // Handle all different types of collision (Method will be rather large)
    }
}

