
using Microsoft.Xna.Framework;

namespace MonoZelda.Shaders
{
    internal class Light : ILight
    {
        public virtual Point Position { get; set; }
        public float Radius { get; set; } = 300f;
        public float Intensity { get; set; } = 1.0f;
        public Color Color { get; set; } = Color.White;
    }
}