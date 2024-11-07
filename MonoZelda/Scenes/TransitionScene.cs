using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using MonoZelda.Commands;
using MonoZelda.Dungeons;

namespace MonoZelda.Scenes
{
    internal class TransitionScene : Scene
    {
        private ICommand loadCommand;

        private IDungeonRoom currentRoom;
        private IDungeonRoom nextRoom;

        public TransitionScene(IDungeonRoom currentRoom, IDungeonRoom nextRoom, ICommand loadCommand)
        {
            this.loadCommand = loadCommand;
            this.currentRoom = currentRoom;
            this.nextRoom = nextRoom;
        }

        public override void LoadContent(ContentManager contentManager)
        {
            // TODO: Load some backgrounds
        }

        public override void Update(GameTime gameTime)
        {
            // TODO: Move some backgrounds

            // TODO: If done, 
            loadCommand.Execute(nextRoom.RoomName);
        }
    }
}
