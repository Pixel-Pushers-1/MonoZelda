using Microsoft.Xna.Framework;
using System;

namespace MonoZelda.Dungeons
{
    internal class ExampleDoor : IDoor
    {
        public string Destination => destination;
        public Rectangle Bounds { get; set; }

        private string destination;

        public ExampleDoor(string destination, Point position, int width, int height)
        {
            this.destination = destination;
            Bounds = new Rectangle(position, new Point(width, height));
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
