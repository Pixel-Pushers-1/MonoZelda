using MonoZelda.Enemies;
using MonoZelda.Link;
using System.Collections.Generic;

namespace MonoZelda.Items;

public class ItemCollisionManager
{
    private PlayerSpriteManager playerSpriteManager;
    private List<IEnemy> enemyList; 

    public ItemCollisionManager(PlayerSpriteManager playerSpriteManager, List<IEnemy> enemyList)
    {
        this.playerSpriteManager = playerSpriteManager; 
        this.enemyList = enemyList;
    }
}
