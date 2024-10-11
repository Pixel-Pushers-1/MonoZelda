using System;
using PixelPushers.MonoZelda.Link;
using PixelPushers.MonoZelda.Controllers;
using Microsoft.Xna.Framework.Input;

namespace PixelPushers.MonoZelda.Commands
{
    public class PlayerStandingCommand : ICommand
    {
        private IController controller;
        private Player player;
        private Direction lastDirection;

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

        public GameState Execute(Keys PressedKey)
        {
            // call player standing method
            if (player != null)
            {
                lastDirection = player.PlayerDirection;
                player.StandingPlayer(this);
            }

            return controller.GameState;
        }

        public GameState UnExecute()
        {
            // Implement if you need to reverse this command
            throw new NotImplementedException();
        }

        public void SetController(IController controller)
        {
            this.controller = controller;
        }  
    }
}
