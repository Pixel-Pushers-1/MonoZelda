using Microsoft.Xna.Framework;

namespace MonoZelda.Dungeons
{
    public interface IDoor
    {
        string Destination { get; }
        public Rectangle Bounds { get; set; }
        public Point Position { get; set; }
        public Dungeon1Sprite DoorSprite { get; set; }

        public void Open();
        public void Close();
    }
}
