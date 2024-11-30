using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Sound;
using MonoZelda.Sprites;

namespace MonoZelda.Scenes;

public class MainMenuScene : Scene
{
    enum MenuSprite { title }
    private GraphicsDevice _graphicsDevice;

    public MainMenuScene(GraphicsDevice graphicsDevice)
    {
        _graphicsDevice = graphicsDevice;
    }

    public override void Update(GameTime gameTime)
    {
        // To-do: animate waterfall
    }

    public override void LoadContent(ContentManager contentManager)
    {
        var dict = new SpriteDict(SpriteType.Title, 0, new Point(0,0));
        dict.SetSprite(nameof(MenuSprite.title));

        // Make sure the shader isn't shadowing anything
        MonoZeldaGame.Shader.SetNumLineSegments(0);

        SoundManager.PlaySound("LOZ_Intro", true);
    }
}