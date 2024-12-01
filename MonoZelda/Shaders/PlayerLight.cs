
using Microsoft.Xna.Framework;
using MonoZelda.Link;

namespace MonoZelda.Shaders
{
    internal class PlayerLight : Light
    {
        public const int MIN_RADIUS = 150;
        public const int MAX_RADIUS = 300;

        public override Point Position => PlayerState.Position;
    }
}