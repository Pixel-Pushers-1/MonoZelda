using MonoZelda.Commands;
using MonoZelda.Controllers;
using MonoZelda.Dungeons;

namespace MonoZelda.Tiles
{
    internal class KeyDoor : DungeonDoor
    {
        public KeyDoor(DoorSpawn spawnPoint, ICommand roomTransitionCommand, CollisionController c) 
            : base(spawnPoint, roomTransitionCommand, c)
        {
        }
    }
}
