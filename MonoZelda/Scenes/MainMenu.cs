using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Sprites;

namespace MonoZelda.Scenes;

public class MainMenu : IScene
{
    enum MenuSprite { title }

    public void Update(GameTime gameTime)
    {
        // TODO: Animate the waterfall
    }

    public void LoadContent()
    {
        var dict = new SpriteDict(SpriteType.Title, 0, new Point(0,0));
        dict.SetSprite(nameof(MenuSprite.title));
    }
}