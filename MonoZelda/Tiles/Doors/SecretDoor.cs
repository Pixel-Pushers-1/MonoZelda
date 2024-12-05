
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using MonoZelda.Collision;
using MonoZelda.Commands;
using MonoZelda.Controllers;
using MonoZelda.Dungeons;
using MonoZelda.Link;
using MonoZelda.Scenes;
using MonoZelda.Sound;
using MonoZelda.Dungeons.Parser.Data;
using MonoZelda.Enemies;
using MonoZelda.Trigger;
using System;

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
