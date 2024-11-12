// PlayerDeathCommand.cs
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Commands;
using MonoZelda.Link;
using MonoZelda.Scenes;

namespace MonoZelda.Commands.GameCommands
{
    public class PlayerDeathCommand : ICommand
    {
        private MonoZeldaGame game;
        public PlayerDeathCommand()
        {

        }
        public PlayerDeathCommand(MonoZeldaGame game)
        {
            this.game = game; 
        }

        public void Execute(params object[] parameters)
        {
            game?.ResetGame();



        }
        public void UnExecute()
        {
            //empty
        }
    }
}