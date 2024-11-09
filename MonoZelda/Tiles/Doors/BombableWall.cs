using MonoZelda.Commands;
using MonoZelda.Controllers;
using MonoZelda.Dungeons;

namespace MonoZelda.Tiles
{
    internal class BombableWall : DungeonDoor
    {
        public BombableWall(DoorSpawn spawnPoint, ICommand roomTransitionCommand, CollisionController c)
            : base(spawnPoint, roomTransitionCommand, c)
        {
            
        }
    }
}
