
using Microsoft.Xna.Framework;

namespace MonoZelda.Shaders
{
    public class Light : ILight
    {
        public virtual Point Position { get; set; }
        public virtual float Radius { get; set; } = 300f;
        public virtual float Intensity { get; set; } = 1.0f;
        public virtual Color Color { get; set; } = Color.White;
    }
}