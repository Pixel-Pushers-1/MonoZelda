using System;
using PixelPushers.MonoZelda.Link;
using Microsoft.Xna.Framework.Input;

namespace PixelPushers.MonoZelda.Commands
{
    public class PlayerStandingCommand : ICommand
    {
        private Player player;
        private Direction lastDirection;

        public MonoZeldaGame Game { get; set; }

        public PlayerStandingCommand()
        {
            
        }

        public PlayerStandingCommand(Player player)
        {
            this.player = player;   
        }

        public Direction PlayerDirection
        {
            get { return lastDirection; }
        }

        public void Execute(Keys PressedKey)
        {
            // call player standing method
            if (player != null)
            {
                lastDirection = player.PlayerDirection;
                player.StandingPlayer(this);
            }
        }

        public void UnExecute()
        {
            // Implement if you need to reverse this command
            throw new NotImplementedException();
        } 
    }
}
