using MonoZelda.Link;

namespace MonoZelda.Commands.GameCommands
{
    public class PlayerStandingCommand : ICommand
    {
        private PlayerSpriteManager player;

        public PlayerStandingCommand()
        {
            //empty
        }

        public PlayerStandingCommand(PlayerSpriteManager player)
        {
            this.player = player;
        }

        public void Execute(params object[] metadata)
        {
            // call player standing method
            if (player != null)
            {
                player.StandStill(this);
            }
        }

        public void UnExecute()
        {
            //empty
        }
    }
}
