using MonoZelda.Dungeons;

namespace MonoZelda.Scenes
{
    public interface IDungeonRoomLoader
    {
        public IDungeonRoom LoadRoom(string roomName);
    }
}
