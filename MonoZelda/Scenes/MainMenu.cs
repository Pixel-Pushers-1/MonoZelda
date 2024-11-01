using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
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

    public void StopSound()
    {
        soundEffect.Dispose();
    }

    public void Update(GameTime gameTime)
    {
        soundEffect.Play();
    }

    public void LoadContent(ContentManager contentManager)
    {
        var dict = new SpriteDict(contentManager.Load<Texture2D>(TextureData.Title), SpriteCSVData.Title, 0, new Point(0,0));
        dict.SetSprite(nameof(MenuSprite.title));

        soundEffect = contentManager.Load<SoundEffect>("Sound/LOZ_Intro");
    }
}