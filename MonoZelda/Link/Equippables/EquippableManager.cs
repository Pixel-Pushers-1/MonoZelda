﻿using Microsoft.Xna.Framework;
using MonoZelda.Controllers;
using MonoZelda.Link.Equippables;
using MonoZelda.Link.Projectiles;
using MonoZelda.Sprites;
using MonoZelda.UI.NavigableMenus;
using System.Collections.Generic;

namespace MonoZelda.Link;

public class EquippableManager : INavigableGrid
{
    private ProjectileManager projectileManager;
    private readonly SwordEquippable swordEquippable;

    private EquippableType[,] equippablesGrid;
    private Point selectedEquippable;

    private readonly Dictionary<EquippableType, IEquippable> equippableObjects = new()
    {
        { EquippableType.Bow, new BowEquippable() },
        { EquippableType.Boomerang, new BoomerangEquippable() },
        { EquippableType.Bomb, new BombEquippable() },
        { EquippableType.CandleBlue, new CandleBlueEquippable() },
        { EquippableType.BluePotion, new BluePotionEquippable() },
        { EquippableType.RedPotion, new RedPotionEquippable() },
    };

    public EquippableManager(CollisionController collisionController)
    {
        projectileManager = new ProjectileManager(collisionController);
        swordEquippable = new SwordEquippable();

        //set up grid of items
        equippablesGrid = new EquippableType[5, 2];
        for (int y = 0; y < equippablesGrid.GetLength(1); y++) {
            for (int x = 0; x < equippablesGrid.GetLength(0); x++) {
                equippablesGrid[x, y] = EquippableType.None;
            }
        }
    }

    public void MoveSelection(Point movement)
    {
        //set current selection and clamp to stay inside of grid
        selectedEquippable += movement;
        selectedEquippable.X = MathHelper.Clamp(selectedEquippable.X, 0, equippablesGrid.GetLength(0) - 1);
        selectedEquippable.Y = MathHelper.Clamp(selectedEquippable.Y, 0, equippablesGrid.GetLength(1) - 1);
    }

    public void ExecuteSelection()
    {
        //empty (nothing happens when you press enter on a selected item)
    }

    public void AddEquippable(EquippableType type, bool allowDuplicates)
    {
        //check that item isn't already in inventory
        if (!allowDuplicates) {
            foreach (EquippableType equippable in equippablesGrid) {
                if (equippable == type) {
                    return;
                }
            }
        }

        //navigate grid row-wise until empty slot is found
        for (int y = 0; y < equippablesGrid.GetLength(1); y++) {
            for (int x = 0; x < equippablesGrid.GetLength(0); x++) {
                if (equippablesGrid[x, y] == EquippableType.None) {
                    equippablesGrid[x, y] = type;
                    return;
                }
            }
        }
    }

    public void RemoveEquippable(EquippableType type)
    {
        //navigate grid row-wise and remove the first instance of type
        for (int y = 0; y < equippablesGrid.GetLength(1); y++) {
            for (int x = 0; x < equippablesGrid.GetLength(0); x++) {
                if (equippablesGrid[x, y] == type) {
                    equippablesGrid[x, y] = EquippableType.None;
                    return;
                }
            }
        }
    }

    public void UseEquippedItem()
    {
        EquippableType selection = equippablesGrid[selectedEquippable.X, selectedEquippable.Y];
        if (selection != EquippableType.None)
        {
            equippableObjects[selection].Use(projectileManager, this);
        }
    }

    public void UseSwordEquippable()
    {
        swordEquippable.Use(projectileManager);
    }

    public EquippableType GetEquippedItem()
    {
        return equippablesGrid[selectedEquippable.X, selectedEquippable.Y];
    }

    public EquippableType[,] GetGrid()
    {
        return equippablesGrid;
    }

    public Point GetSelection()
    {
        return selectedEquippable;
    }

    public void Update()
    {
        projectileManager.Update();
    }
}