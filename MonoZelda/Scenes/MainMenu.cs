using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PixelPushers.MonoZelda.Scenes;
using PixelPushers.MonoZelda.Sprites;

namespace PixelPushers.MonoZelda;

public class MainMenu : IScene
{
    GraphicsDevice _graphicsDevice;

    enum MenuSprite { title }

    public MainMenu(GraphicsDevice graphicsDevice)
    {
        _graphicsDevice = graphicsDevice;
    }

    public void Update(GameTime gameTime)
    {
        // TODO: Animate the waterfall
    }

    public void LoadContent(ContentManager contentManager)
    {
        var dict = new SpriteDict(contentManager.Load<Texture2D>(TextureData.Title), SpriteCSVData.Title, 0, new Point(0,0));
        dict.SetSprite(nameof(MenuSprite.title));
    }
}