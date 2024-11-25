using MonoZelda.Controllers;
using MonoZelda.Link.Equippables;
using MonoZelda.Link.Projectiles;
using System.Collections.Generic;

namespace MonoZelda.Link;

public class EquippableManager
{
    private CollisionController collisionController;
    private ProjectileManager projectileManager;
    private int CyclingIndex;
    private const int NUM_EQUIPPABLES = 6;
    private bool isPaused;
    private readonly SwordEquippable swordEquippable;

    private readonly Dictionary<EquippableType, IEquippable> equippableObjects = new()
    {
        { EquippableType.Bow, new BowEquippable() },
        { EquippableType.Boomerang, new BoomerangEquippable() },
        { EquippableType.Bomb, new BombEquippable() },
        { EquippableType.CandleBlue, new CandleBlueEquippable() },
        { EquippableType.BluePotion, new BluePotionEquippable() },
        { EquippableType.RedPotion, new RedPotionEquippable() },
    };

    public EquippableType EquippedItem
    {
        get => PlayerState.EquippedItem;
        private set => PlayerState.EquippedItem = value;
    }

    public bool IsPaused
    {
        set { isPaused = value; }
    }

    public EquippableManager(CollisionController collisionController)
    {
        CyclingIndex = 0;
        EquippedItem = EquippableType.None;
        this.collisionController = collisionController;
        projectileManager = new ProjectileManager(collisionController);
        swordEquippable = new SwordEquippable();
    }

    public void CycleEquippedUtility()
    {
        List<EquippableType> equippables = PlayerState.EquippableInventory;
        if (CyclingIndex < equippables.Count)
        {
            CyclingIndex = CyclingIndex + 1;
            if (CyclingIndex == NUM_EQUIPPABLES)
            {
                CyclingIndex = 0;
            }
            EquippedItem = equippables[CyclingIndex - 1];
        }
    }

    public void UseEquippedItem()
    {
        if (PlayerState.EquippedItem == EquippableType.None)
        {
            return;
        }
        else
        {
            equippableObjects[PlayerState.EquippedItem].Use(projectileManager);
        }
    }

    public void UseSwordEquippable()
    {
        swordEquippable.Use(projectileManager);
    }

    public void Update()
    {
        projectileManager.Update();
    }
}
