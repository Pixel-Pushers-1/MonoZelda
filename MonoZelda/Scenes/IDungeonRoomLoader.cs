using MonoZelda.Dungeons;
using MonoZelda.Save;

namespace MonoZelda.Scenes
{
    public interface IDungeonRoomLoader
    {
        public IDungeonRoom LoadRoom(string roomName);
        public void AddRandomRoom(string roomName, DungeonRoom room);
        public void Save(SaveState save);
        public void Load(SaveState save);
    }
}
