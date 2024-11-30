using MonoZelda.Dungeons;
using MonoZelda.Save;

namespace MonoZelda.Scenes
{
    public interface IDungeonRoomLoader
    {
        public IDungeonRoom LoadRoom(string roomName);
        public void Save(SaveState save);
        public void Load(SaveState save);
    }
}
