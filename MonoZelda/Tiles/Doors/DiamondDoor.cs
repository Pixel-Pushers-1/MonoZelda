
using MonoZelda.Commands;
using MonoZelda.Controllers;
using MonoZelda.Dungeons;

namespace MonoZelda.Tiles
{
    internal class DiamondDoor : DungeonDoor
    {
        public DiamondDoor(DoorSpawn door, ICommand roomTransitionCommand, CollisionController c) 
            : base(door, roomTransitionCommand, c)
        {
        }
    }
}
