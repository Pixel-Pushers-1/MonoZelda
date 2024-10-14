using MonoZelda.Dungeons;

namespace MonoZelda.Scenes
{
    internal interface IDungeonRoomLoader
    {
        public IDungeonRoom LoadRoom(string roomName);
    }
}
