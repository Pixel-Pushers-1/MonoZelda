using MonoZelda.Controllers;
using MonoZelda.Link.Equippables;
using MonoZelda.Link.Projectiles;
using System.Collections.Generic;

namespace MonoZelda.Link;

public class EquippableManager
{
    private CollisionController collisionController;
    private ProjectileManager projectileManager;
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

    public EquippableManager(CollisionController collisionController)
    {
        EquippedItem = EquippableType.None;
        this.collisionController = collisionController;
        projectileManager = new ProjectileManager(collisionController);
        swordEquippable = new SwordEquippable();
    }

    public void CycleEquippedUtility()
    {
        if (EquippedItem == EquippableType.RedPotion)
        {
            EquippedItem = EquippableType.None;
        }
        else
        {
            EquippedItem = EquippedItem + 1;
        }
    }

    public void UseEquippedItem()
    {
        equippableObjects[PlayerState.EquippedItem].Use(projectileManager);
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
