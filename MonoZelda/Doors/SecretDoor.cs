
using System.Collections.Generic;
using MonoZelda.Commands;
using MonoZelda.Controllers;
using MonoZelda.Dungeons;
using MonoZelda.Sound;
using MonoZelda.Enemies;
using MonoZelda.Trigger;
using MonoZelda.Doors;

namespace MonoZelda.Tiles
{
    internal class SecretDoor : DiamondDoor
    {
        private PushBlockTrigger trigger;
        private DoorSpawn door;

        public SecretDoor(DoorSpawn door, ICommand roomTransitionCommand, CollisionController c, PushBlockTrigger trigger) 
            : base(door, roomTransitionCommand, c, new List<Enemy>())
        {
            this.door = door;
            this.trigger = trigger;
        }

        protected override void OpenDoor()
        {
            if(door.Type == Dungeon1Sprite.door_closed_west)
            { 
                SoundManager.PlaySound("LOZ_Secret", false);
                base.OpenDoor();
            }
        }

        protected override bool CheckForEnemies()
        {
            if(trigger == null)
            {
                return base.CheckForEnemies();
            }

            return !trigger.IsPushed;
        }
    }
}
