using MonoZelda.Commands;
using MonoZelda.Commands.GameCommands;
using MonoZelda.Dungeons;
using MonoZelda.Enemies;
using MonoZelda.Items.ItemClasses;
using MonoZelda.Link;
using System.Collections.Generic;

namespace MonoZelda.Items;

public class ItemManager
{
    private List<ItemSpawn> roomSpawnList;
    private List<Item> itemUpdateList;
    private List<Enemy> roomEnemyList;
    private PlayerCollisionManager playerCollision;
    private ICommand levelCompleteAnimationCommand;
    private GameType gameMode;

    public ItemManager(GameType gameMode, ICommand levelCompleteAnimationCommand, List<ItemSpawn> roomSpawnList, List<Enemy> roomEnemyList, PlayerCollisionManager playerCollision)
    {
        this.gameMode = gameMode;
        this.roomSpawnList = roomSpawnList;
        this.roomEnemyList = roomEnemyList;
        this.playerCollision = playerCollision;
        this.levelCompleteAnimationCommand = levelCompleteAnimationCommand;
        itemUpdateList = new List<Item>();
    }

    public GameType GameMode
    {
        get { return gameMode; }
    }

    public List<ItemSpawn> RoomSpawnList
    {
        get { return roomSpawnList; }
    }

    public List<Enemy> RoomEnemyList
    {
        get { return roomEnemyList; }
    }

    public PlayerCollisionManager PlayerCollision
    {
        get { return playerCollision; }
    }

    public void TriggerLevelCompleteAnimation()
    {
        levelCompleteAnimationCommand.Execute();    
    }

    public void AddRoomSpawnItem(ItemSpawn itemSpawn)
    {
        roomSpawnList.Add(itemSpawn);   
    }

    public void RemoveRoomSpawnItem(ItemSpawn itemSpawn)
    {
        roomSpawnList.Remove(itemSpawn);
    }

    public void AddUpdateItem(Item item)
    {
        itemUpdateList.Add(item);
    }

    public void RemoveUpdateItem(Item item)
    {
        itemUpdateList.Remove(item);
    }

    public void Update()
    {
        for(int i = 0; i < itemUpdateList.Count; i++)
        {
            itemUpdateList[i].Update();
        }
    }
}

