using MonoZelda.Controllers;
using MonoZelda.Link.Equippables;
using MonoZelda.Link.Projectiles;
using System.Collections.Generic;

namespace MonoZelda.Link;

public class EquippableManager
{
    private CollisionController collisionController;
    private ProjectileManager projectileManager;
    private int cyclingIndex;
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

    public int CyclingIndex
    {
        set { cyclingIndex = value; }
    }

    public bool IsPaused
    {
        set { isPaused = value; }
    }

    public EquippableManager(CollisionController collisionController)
    {
        cyclingIndex = 0;
        this.collisionController = collisionController;
        projectileManager = new ProjectileManager(collisionController);
        swordEquippable = new SwordEquippable();
    }

    public void CycleEquippedUtility()
    {
        List<EquippableType> equippables = PlayerState.EquippableInventory;
        if ((isPaused) && (cyclingIndex <= equippables.Count))
        {
            if (cyclingIndex != (equippables.Count))
            {
                EquippedItem = PlayerState.EquippableInventory[cyclingIndex++];
            }
            else
            {
                EquippedItem = EquippableType.None;
                cyclingIndex = 0;
            }    
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
            equippableObjects[PlayerState.EquippedItem].Use(projectileManager,this);
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
