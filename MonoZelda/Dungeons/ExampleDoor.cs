using Microsoft.Xna.Framework;
using System;

namespace MonoZelda.Dungeons
{
    internal class ExampleDoor : IDoor
    {
        public string Destination => destination;
        public Rectangle Bounds { get; set; }
        public Point Position { get; set; }
        public Dungeon1Sprite DoorSprite { get; set; }

        private string destination;

        public ExampleDoor(string destination, Point position, int width, int height, Dungeon1Sprite doorSprite)
        {
            this.destination = destination;
            Bounds = new Rectangle(position, new Point(width, height));
            Position = position;
            DoorSprite = doorSprite;
        }

        public void Open()
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }
    }
}
