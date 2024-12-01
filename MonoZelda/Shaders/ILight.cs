
using Microsoft.Xna.Framework;

namespace MonoZelda.Shaders
{
    internal interface ILight
    {
        public Point Position { get; set; }
        public Color Color { get; set; }
        public float Radius { get; set; }
        public float Intensity { get; set; }
    }
}