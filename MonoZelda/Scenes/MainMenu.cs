using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Sound;
using MonoZelda.Sprites;

namespace MonoZelda.Scenes;

public class MainMenu : IScene
{
    GraphicsDevice _graphicsDevice;
    private SoundEffect soundEffect;

    enum MenuSprite { title }

    public MainMenu(GraphicsDevice graphicsDevice)
    {
        _graphicsDevice = graphicsDevice;
    }


    public void Update(GameTime gameTime)
    {
        // To-do: animate waterfall
    }

    public void LoadContent(ContentManager contentManager)
    {
        var dict = new SpriteDict(contentManager.Load<Texture2D>(TextureData.Title), SpriteCSVData.Title, 0, new Point(0,0));
        dict.SetSprite(nameof(MenuSprite.title));

        SoundManager.PlaySound("LOZ_Intro", true);
    }
}