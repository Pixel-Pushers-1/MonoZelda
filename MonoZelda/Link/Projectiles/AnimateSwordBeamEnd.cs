using Microsoft.Xna.Framework;
using MonoZelda.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoZelda.Link.Projectiles;

public class AnimateSwordBeamEnd
{
    private Point animationOrigin;
    private readonly SpriteDict spriteDictNorthWest;
    private readonly SpriteDict spriteDictNorthEast;    
    private readonly SpriteDict spriteDictSouthWest;
    private readonly SpriteDict spriteDictSouthEast;

    // Private constructor to prevent instantiation from outside
    private AnimateSwordBeamEnd()
    {
    }

    // Static method to allow only specific classes to instantiate TrackReturn
    public static AnimateSwordBeamEnd CreateInstance(object caller)
    {
        // Check if the caller is BoomerangBlue or BoomerangGreen
        if (caller is BoomerangBlue || caller is Boomerang)
        {
            return new AnimateSwordBeamEnd();
        }
        throw new UnauthorizedAccessException("Access denied.");
    }

    public void animate(Point projectilePosition)
    {

    }
}
