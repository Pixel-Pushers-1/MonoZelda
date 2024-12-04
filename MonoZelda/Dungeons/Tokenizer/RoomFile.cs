using System.Collections.Generic;

namespace MonoZelda.Dungeons.Loader
{
    public class RoomFile
    {
        public string RoomSprite { get; set; }
        public bool IsLit { get; set; }
        public string NorthDoor { get; set; }
        public string SouthDoor { get; set; }
        public string EastDoor { get; set; }
        public string WestDoor { get; set; }

        public List<List<string>> Content { get; set; }
    }
}
